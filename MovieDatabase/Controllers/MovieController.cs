using Microsoft.AspNet.Identity;
using MovieDatabase.Models;
using MovieDatabase.Models.Domain;
using MovieDatabase.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MovieDatabase.Controllers
{
    public class MovieController : Controller
    {
        private ApplicationDbContext DbContext;

        public MovieController()
        {
            DbContext = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var model = DbContext.Movies
                .Where(p => p.UserId == userId)
                .Select(p => new IndexMovieViewModel
                {
                    Id = p.Id,
                    Category = p.Category,
                    MovieName = p.Name,
                    Rating = p.Rating
                }).ToList();

            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            PopulateViewBag();
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterEditMovieViewModel formData)
        {
            return SaveMovie(null, formData);
        }

        private ActionResult SaveMovie(int? id, RegisterEditMovieViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                PopulateViewBag();
                return View();
            }

            var userId = User.Identity.GetUserId();

            if (DbContext.Movies.Any(p => p.UserId == userId &&
            p.Name == formData.MovieName &&
            (!id.HasValue || p.Id != id.Value)))
            {
                ModelState.AddModelError(nameof(RegisterEditMovieViewModel.MovieName),
                    "Movie name should be unique");

                PopulateViewBag();
                return View();
            }

            Movie movie;

            if (!id.HasValue)
            {
                movie = new Movie();
                movie.UserId = userId;
                DbContext.Movies.Add(movie);
            }
            else
            {
                movie = DbContext.Movies.FirstOrDefault(
               p => p.Id == id);

                if (movie == null)
                {
                    return RedirectToAction(nameof(MovieController.Index));
                }
            }

            movie.Name = formData.MovieName;
            movie.Rating = formData.Rating.Value;
            movie.Category = formData.Category;
            movie.Description = formData.Description;

            DbContext.SaveChanges();

            return RedirectToAction(nameof(MovieController.Index));
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(MovieController.Index));
            }

            var userId = User.Identity.GetUserId();

            var movie = DbContext.Movies.FirstOrDefault(
                p => p.Id == id && p.UserId == userId);

            if (movie == null)
            {
                return RedirectToAction(nameof(MovieController.Index));
            }

            PopulateViewBag();

            var model = new RegisterEditMovieViewModel();
            model.Category = movie.Category;
            model.Rating = movie.Rating;
            model.MovieName = movie.Name;
            model.Description = movie.Description;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, RegisterEditMovieViewModel formData)
        {
            return SaveMovie(id, formData);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(MovieController.Index));
            }

            var userId = User.Identity.GetUserId();

            var movie = DbContext.Movies.FirstOrDefault(p => p.Id == id && p.UserId == userId);

            if (movie != null)
            {
                DbContext.Movies.Remove(movie);
                DbContext.SaveChanges();
            }

            return RedirectToAction(nameof(MovieController.Index));
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction(nameof(MovieController.Index));

            var userId = User.Identity.GetUserId();

            var movie = DbContext.Movies.FirstOrDefault(p => 
            p.Id == id.Value &&
            p.UserId == userId);

            if (movie == null)
                return RedirectToAction(nameof(MovieController.Index));

            var model = new DetailsMovieViewModel();
            model.Category = movie.Category;
            model.Description = movie.Description;
            model.MovieName = movie.Name;
            model.Rating = movie.Rating;

            return View(model);
        }

        private void PopulateViewBag()
        {
            var categories = new SelectList(
                                  new List<string>
                                  {
                                      "Drama",
                                      "Comedy",
                                      "Horror",
                                      "Romance",
                                      "Sci-fi",
                                      "Adventure"
                                  });

            ViewBag.Categories = categories;
        }
    }
}