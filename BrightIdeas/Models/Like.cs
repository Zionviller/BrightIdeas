using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrightIdeas.Models
{
    public class Like
    {
        [Key]
        public int LikeId { get; set; }

        public int Count { get; set; }

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User Liker { get; set; }

        public int IdeaId { get; set; }
        [ForeignKey("IdeaId")]
        public Idea Liked { get; set; }

        public Like()
        {
            Liker = new User();
            Liked = new Idea();
            Count = 1;
        }
    }
}
