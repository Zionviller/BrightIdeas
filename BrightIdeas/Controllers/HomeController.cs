using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BrightIdeas.Models;

namespace BrightIdeas.Controllers
{
    public class HomeController : Controller
    {
        private readonly BrightIdeasContext _context;

        public HomeController(BrightIdeasContext con)
        {
            _context = con;
        }

        [HttpGet("")]
        public IActionResult LoginReg()
        {
            Utils.Log("Login Reg!");
            if (HttpContext.Session.GetObjectFromJson<bool>("LoggedIn"))
            {
                Utils.Log("Already Logged in >> Bright Ideas");
                return RedirectToAction("BrightIdeas");
            }

            Utils.Log("Not logged in. Displaying LoginReg.");
            return View();
        }

        [HttpPost("Login")]
        public IActionResult Login(Login login)
        {
            Utils.Log("Logging in...");
            if (ModelState.IsValid)
            {
                Utils.Log("VALID! Now let's check our db...");
                // Is there a user with this email?
                var dbUser = _context.Users.FirstOrDefault<User>(u => u.Email == login.Email);
                if (dbUser == null)
                {
                    Utils.Log("Bad email.");
                    ModelState.AddModelError("Email", "INvalid email..");
                    return View("LoginReg");
                }

                // Does the password hash?
                PasswordHasher<Login> hasher = new PasswordHasher<Login>();
                PasswordVerificationResult result = hasher.VerifyHashedPassword(login, dbUser.Password, login.Password);
                if (result == PasswordVerificationResult.Failed)
                {
                    Utils.Log("Bad Password.");
                    ModelState.AddModelError("Password", "Badpass! That's what the cool kids say when your password is wrong, or something.");
                    return View("LoginReg");
                }
                // YEAH?!?
                // update session
                HttpContext.Session.SetObjectAsJson("UserId", dbUser.UserId);
                HttpContext.Session.SetObjectAsJson("LoggedIn", true);

                return RedirectToAction("BrightIdeas");
            }
            else
            {
                Utils.Log("INVALID...");
                return View("LoginReg");
            }
        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUser(User user)
        {
            Utils.Log("Create User");
            if (ModelState.IsValid)
            {
                Utils.Log("VALID!");

                // Check for duplicate email
                if (_context.Users.Any(u => u.Email == user.Email))
                {
                    ModelState.AddModelError("Email", "Email already in use.");
                    return View("LoginReg");
                }

                // hash password
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                user.Password = hasher.HashPassword(user, user.Password);

                _context.Users.Add(user);
                _context.SaveChanges();

                // update session
                HttpContext.Session.SetObjectAsJson("UserId", user.UserId);
                HttpContext.Session.SetObjectAsJson("LoggedIn", true);

                return RedirectToAction("BrightIdeas");
            }
            else
            {
                Utils.Log("INVALID...");
                return View("LoginReg");
            }
        }

        [HttpGet("BrightIdeas")]
        public IActionResult BrightIdeas()
        {
            Utils.Log("Bright Ideas!!");
            BrightIdeasViewModel viewModel = new BrightIdeasViewModel
            {
                AllIdeas = _context.Ideas
                    .Include(i => i.LikedBy)
                    .Include(i => i.Creator)
                    .OrderByDescending(i => i.LikedBy.Sum(lb => lb.Count))
                    .ToList(),
                UserId = HttpContext.Session.GetObjectFromJson<int>("UserId")
            };

            if (HttpContext.Session.GetObjectFromJson<bool>("LoggedIn"))
            {
                Utils.Log("Logged In! Displaying the Ideas!");
                return View(viewModel);
            }
            else
            {
                Utils.Log("Not Logged in.. LOGIN OR REGIster!");
                return RedirectToAction("LoginReg");
            }
        }

        [HttpGet("Logout")]
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("LoginReg");
        }

        [HttpPost("CreateIdea")]
        public IActionResult CreateIdea(BrightIdeasViewModel viewModel)
        {
            Utils.Log("CreateIdea!!");

            if (ModelState.IsValid)
            {
                Utils.Log("VALID!");

                Idea newIdea = new Idea
                {
                    Text = viewModel.IdeaSub,
                    Creator = _context.Users.FirstOrDefault<User>(u => u.UserId == viewModel.UserId),
                };

                _context.Ideas.Add(newIdea);
                _context.SaveChanges();

                return RedirectToAction("BrightIdeas");
            }
            else
            {
                Utils.Log("INVALID.. try again.");
                viewModel.AllIdeas = _context.Ideas
                    .Include(i => i.LikedBy)
                    .Include(i => i.Creator)
                    .OrderByDescending(i => i.LikedBy.Sum(lb => lb.Count))
                    .ToList();
                return View("BrightIdeas", viewModel);
            }
        }

        [HttpGet("/Idea/{id}")]
        public IActionResult Idea(int id)
        {
            Idea idea = _context.Ideas
                                .Where(i => i.IdeaId == id)
                                .Include(i => i.LikedBy)
                                    .ThenInclude(lb => lb.Liker)
                                .Include(i => i.Creator)
                                .FirstOrDefault();

            return View(idea);
        }

        [HttpGet("DeleteIdea/{id}")]
        public IActionResult DeleteIdea(int id)
        {
            Idea IdeaToDelete = _context.Ideas
                                        .Include(i => i.Creator)
                                        .FirstOrDefault(i => i.IdeaId == id);

            if (IdeaToDelete.Creator.UserId == HttpContext.Session.GetObjectFromJson<int>("UserId"))
            {
                Utils.Log("Proper User.. Deleting.");
                _context.Ideas.Remove(IdeaToDelete);
                _context.SaveChanges();
            }
            else
            {
                Utils.Log("Wrong user.. AINT DOIN NUFFFFFFFIN!");
            }

            return RedirectToAction("BrightIdeas");
        }

        public IActionResult Like(int id)
        {
            Utils.Log("Like!!");
            Idea idea = _context.Ideas.FirstOrDefault<Idea>(i => i.IdeaId == id);
            User user = _context.Users.FirstOrDefault<User>(u => u.UserId == HttpContext.Session.GetObjectFromJson<int>("UserId"));
            // Grab the like if it exists
            var like = _context.Likes
                                .Where(l => l.UserId == user.UserId)
                                .Where(l => l.IdeaId == idea.IdeaId)
                                .FirstOrDefault<Like>();
            // if not create it
            if (like == null)
            {
                Utils.Log("Like is NULL!");
                like = new Like
                {
                    Liker = user,
                    Liked = idea,
                    Count = 1
                };

                _context.Likes.Add(like);
                _context.SaveChanges();

            }
            else
            {
                Utils.Log("Adding one!");
                like.Count += 1;
                _context.SaveChanges();
            }
            return RedirectToAction("BrightIdeas");
        }

        [HttpGet("BrightMind/{id}")]
        public IActionResult BrightMind(int id)
        {
            Utils.Log("Bright Mind!");
            User user = _context.Users
                                .Where(u => u.UserId == id)
                                .Include(u => u.Likes)
                                .Include(u => u.Ideas)
                                .FirstOrDefault();

            return View(user);
        }
    }
}
