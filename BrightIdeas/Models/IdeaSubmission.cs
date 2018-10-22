using System;
using System.ComponentModel.DataAnnotations;

namespace BrightIdeas.Models
{
    public class IdeaSubmission
    {
        [Required(ErrorMessage = "You must enter text!")]
        [MinLength(2)]
        public string Text { get; set; }
    }
}
