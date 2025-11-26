
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MvcTask3.DataAccses.Configration;
using MvcTask3.Models;
using MvcTask3.ViewModels;

namespace MvcTask3.DataAccses
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { 
        }

        public DbSet<Cinema> Cinemas { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieSubImage> MovieSubImages { get; set; }
        public DbSet<Cart> Carts { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CartEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Cart>()
            //      .HasKey(c => new { c.ApplicationUserId, c.MovieId });

            //modelBuilder.Entity<Cart>()
            //    .HasOne(c => c.ApplicationUser)
            //    .WithMany()
            //    .HasForeignKey(c => c.ApplicationUserId);

            //modelBuilder.Entity<Cart>()
            //    .HasOne(c => c.Movie)
            //    .WithMany()
            //    .HasForeignKey(c => c.MovieId);








        }
        // Seed Roles



    }
}
