using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserProfile.Domain.Entities;

namespace UserProfile.Infrastructure.Persistance
{
    public class UserProfileDbContext : DbContext
    {
        public UserProfileDbContext(DbContextOptions<UserProfileDbContext> options) : base(options) { }

        public DbSet<UserProfileClass> Profiles => Set<UserProfileClass>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfileClass>(b =>
            {
                b.HasKey(x => x.Id);
                b.HasIndex(x => x.UserId).IsUnique();
                b.Property(x => x.Goal).HasMaxLength(100);
                b.Property(x => x.ActivityLevel).HasMaxLength(50);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
