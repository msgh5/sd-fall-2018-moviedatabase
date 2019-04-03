using MovieDatabase.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieDatabase.Models.Helpers
{   
    public class MoviesHelper
    {
        private ApplicationDbContext DbContext;

        public MoviesHelper(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public Movie GetMovieById(int id, string userId)
        {
            return DbContext.Movies.FirstOrDefault(
                p => p.Id == id && p.UserId == userId);
        }

        public List<Movie> GetUsersMovies(string userId)
        {
            return DbContext.Movies
                .Where(p => p.UserId == userId).ToList();
        }

        public bool CheckIfMovieAlreadyExists(int? id, string movieName, string userId)
        {
            return DbContext.Movies.Any(p => p.UserId == userId
            && p.Name == movieName
            && (!id.HasValue || p.Id != id.Value));
        }

        public Movie GetMovieByName(string name, string userId)
        {
            return DbContext.Movies.FirstOrDefault(p =>
            p.Name == name &&
            p.UserId == userId);
        }
    }
}