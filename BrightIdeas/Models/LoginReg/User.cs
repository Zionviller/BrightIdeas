using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using BrightIdeas.Models;

namespace BrightIdeas.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please enter a name.")]
        [MinLength(2, ErrorMessage = "Name must be longer.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter an alias.")]
        [MinLength(2, ErrorMessage = "Alias must be longer.")]
        public string Alias { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Please enter an email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [MinLength(8, ErrorMessage = "Password must be 8 characters or more.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [NotMapped]
        [Compare("Password", ErrorMessage = "Passwords must match.")]
        [DataType(DataType.Password)]
        public string Confirm { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [InverseProperty("Creator")]
        public List<Idea> Ideas { get; set; }

        [InverseProperty("Liker")]
        public List<Like> Likes { get; set; }

        public User()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Ideas = new List<Idea>();
            Likes = new List<Like>();
        }
    }
}
