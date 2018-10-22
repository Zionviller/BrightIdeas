using System;
using System.ComponentModel.DataAnnotations;

namespace BrightIdeas.Models
{
    public class Login
    {
        public Login()
        {
        }

        [Required(ErrorMessage = "Enter an email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
