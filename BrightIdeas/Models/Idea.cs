using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BrightIdeas.Models
{
    public class Idea
    {
        [Key]
        public int IdeaId { get; set; }

        public string Text { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User Creator { get; set; }

        public List<Like> LikedBy { get; set; }

        public int TotalLikes
        {
            get
            {
                return LikedBy.Sum<Like>(l => l.Count);
            }
        }

        public Idea()
        {
            LikedBy = new List<Like>();
        }
    }
}
