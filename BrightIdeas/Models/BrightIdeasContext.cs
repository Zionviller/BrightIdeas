using System;
using Microsoft.EntityFrameworkCore;

namespace BrightIdeas.Models
{
    public class BrightIdeasContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Idea> Ideas { get; set; }
        public DbSet<Like> Likes { get; set; }

        public BrightIdeasContext(DbContextOptions<BrightIdeasContext> options) : base(options)
        {
        }
    }
}
