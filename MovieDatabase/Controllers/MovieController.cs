using MovieDatabase.Models.Domain;
using MovieDatabase.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MovieDatabase.Controllers
{
    public class MovieController : Controller
    {
        private static Random Random = new Random();
        private static List<Movie> MoviesDatabase = new List<Movie>();

        public ActionResult Index()
        {
            var model = MoviesDatabase.Select(p => new IndexMovieViewModel
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

            if (MoviesDatabase.Any(p =>
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
                movie.Id = Random.Next(1, 1001);
                MoviesDatabase.Add(movie);
            }
            else
            {
                movie = MoviesDatabase.FirstOrDefault(
               p => p.Id == id);

                if (movie == null)
                {
                    return RedirectToAction(nameof(MovieController.Index));
                }
            }

            movie.Name = formData.MovieName;
            movie.Rating = formData.Rating.Value;
            movie.Category = formData.Category;

            return RedirectToAction(nameof(MovieController.Index));
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(MovieController.Index));
            }

            var movie = MoviesDatabase.FirstOrDefault(
                p => p.Id == id);

            if (movie == null)
            {
                return RedirectToAction(nameof(MovieController.Index));
            }

            PopulateViewBag();

            var model = new RegisterEditMovieViewModel();
            model.Category = movie.Category;
            model.Rating = movie.Rating;
            model.MovieName = movie.Name;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, RegisterEditMovieViewModel formData)
        {
            return SaveMovie(id, formData);
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(MovieController.Index));
            }

            var movie = MoviesDatabase.FirstOrDefault(p => p.Id == id);

            if (movie != null)
            {
                MoviesDatabase.Remove(movie);
            }

            return RedirectToAction(nameof(MovieController.Index));
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