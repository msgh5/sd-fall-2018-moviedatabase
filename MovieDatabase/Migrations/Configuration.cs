namespace MovieDatabase.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using MovieDatabase.Models;
    using MovieDatabase.Models.Domain;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MovieDatabase.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "MovieDatabase.Models.ApplicationDbContext";
        }

        protected override void Seed(MovieDatabase.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            //if (!context.Movies.Any())
            //{
            //    var movie = new Movie();
            //    movie.Category = "Drama";
            //    movie.Rating = 1;
            //    movie.Name = "movie";

            //    context.Movies.Add(movie);
            //    context.SaveChanges();
            //}

            //Seeding data

            var movie = new Movie();
            movie.Category = "Drama";
            movie.Rating = 2;
            movie.Name = "movie";

            context.Movies.AddOrUpdate(p => p.Name, movie);

            //Seeding Users and Roles

            //RoleManager, used to manage roles
            var roleManager = 
                new RoleManager<IdentityRole>(
                    new RoleStore<IdentityRole>(context));

            //UserManager, used to manage users
            var userManager = 
                new UserManager<ApplicationUser>(
                        new UserStore<ApplicationUser>(context));

            //Adding admin role if it doesn't exist.
            if (!context.Roles.Any(p => p.Name == "Admin"))
            {
                var adminRole = new IdentityRole("Admin");
                roleManager.Create(adminRole);
            }

            //Creating the adminuser
            ApplicationUser adminUser;

            if (!context.Users.Any(
                p => p.UserName == "guilherme.guizado@mitt.ca"))
            {
                adminUser = new ApplicationUser();
                adminUser.UserName = "guilherme.guizado@mitt.ca";
                adminUser.Email = "guilherme.guizado@mitt.ca";

                userManager.Create(adminUser, "Password-1");
            }
            else
            {
                adminUser = context
                    .Users
                    .First(p => p.UserName == "guilherme.guizado@mitt.ca");
            }

            //Make sure the user is on the admin role
            if (!userManager.IsInRole(adminUser.Id, "Admin"))
            {
                userManager.AddToRole(adminUser.Id, "Admin");
            }
        }
    }
}
