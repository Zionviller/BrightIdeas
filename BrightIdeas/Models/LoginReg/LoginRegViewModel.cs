using System;
using BrightIdeas.Models;

namespace BrightIdeas.Models
{
    public class LoginRegViewModel
    {
        public User User { get; set; }
        public Login Login { get; set; }

        public LoginRegViewModel()
        {
            User = new User();
            Login = new Login();
        }
    }
}
