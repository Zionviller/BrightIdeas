using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BrightIdeas.Models
{
    public class BrightIdeasViewModel
    {
        public List<Idea> AllIdeas { get; set; }
        [Required]
        public string IdeaSub { get; set; }
        public int UserId { get; set; }

        public BrightIdeasViewModel()
        {
            AllIdeas = new List<Idea>();
            UserId = 0;
        }
    }
}

