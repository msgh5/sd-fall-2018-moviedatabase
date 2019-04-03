using Microsoft.AspNet.Identity;
using MovieDatabase.Models;
using MovieDatabase.Models.Domain;
using MovieDatabase.Models.Helpers;
using MovieDatabase.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace MovieDatabase.Controllers
{
    public class MovieController : Controller
    {
        private ApplicationDbContext DbContext;
        private MoviesHelper MoviesHelper;
        
        public MovieController()
        {
            DbContext = new ApplicationDbContext();
            MoviesHelper = new MoviesHelper(DbContext);
        }

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();

            var model = MoviesHelper.GetUsersMovies(userId)
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
            
            if (MoviesHelper.CheckIfMovieAlreadyExists(id, 
                formData.MovieName, 
                userId))
            {
                ModelState.AddModelError(nameof(RegisterEditMovieViewModel.MovieName),
                    "Movie name should be unique");

                PopulateViewBag();
                return View();
            }

            string fileExtension;

            //Validating file upload
            if (formData.Media != null)
            {
                fileExtension = Path.GetExtension(formData.Media.FileName);

                if (!Constants.AllowedFileExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("", "File extension is not allowed.");
                    PopulateViewBag();
                    return View();
                }
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
                movie = MoviesHelper.GetMovieById(id.Value, userId);

                if (movie == null)
                {
                    return RedirectToAction(nameof(MovieController.Index));
                }
            }

            movie.Name = formData.MovieName;
            movie.Rating = formData.Rating.Value;
            movie.Category = formData.Category;
            movie.Description = formData.Description;

            //Handling file upload
            if (formData.Media != null)
            {
                if (!Directory.Exists(Constants.MappedUploadFolder))
                {
                    Directory.CreateDirectory(Constants.MappedUploadFolder);
                }

                var fileName = formData.Media.FileName;
                var fullPathWithName = Constants.MappedUploadFolder + fileName;

                formData.Media.SaveAs(fullPathWithName);

                movie.MediaUrl = Constants.UploadFolder + fileName;
            }

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

            var movie = MoviesHelper.GetMovieById(id.Value, userId);

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

            var movie = MoviesHelper.GetMovieById(id.Value, userId);

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

            var movie = MoviesHelper.GetMovieById(id.Value, userId);

            if (movie == null)
                return RedirectToAction(nameof(MovieController.Index));

            var model = new DetailsMovieViewModel();
            model.Category = movie.Category;
            model.Description = movie.Description;
            model.MovieName = movie.Name;
            model.Rating = movie.Rating;
            model.MediaUrl = movie.MediaUrl;

            return View(model);
        }

        [HttpGet]
        [Route("mymovies/{name}")] // Creates a custom route to this action
        public ActionResult DetailsByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return RedirectToAction(nameof(MovieController.Index));
            }

            var userId = User.Identity.GetUserId();

            var movie = MoviesHelper.GetMovieByName(name, userId);

            if (movie == null)
            {
                return RedirectToAction(nameof(MovieController.Index));
            }

            var model = new DetailsMovieViewModel();
            model.Category = movie.Category;
            model.Description = movie.Description;
            model.MovieName = movie.Name;
            model.Rating = movie.Rating;
            model.MediaUrl = movie.MediaUrl;

            return View("Details", model);
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