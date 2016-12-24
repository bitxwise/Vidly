using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        public ActionResult Index()
        {
            var movies = GetMovies();

            return View(movies);
        }

        // GET: Movies
        public ActionResult Random()
        {
            var movie = new Movie() {
                Name = "Shrek!"
            };

            var customers = new List<Customer>() {
                new Customer() { Name = "John Smith" },
                new Customer() { Name = "Jane Smith" }
            };

            var viewModel = new RandomMovieViewModel() {
                Customers = customers,
                Movie = movie
            };

            return View(viewModel );
        }

        [Route("movies/released/{year:regex(\\d{4})}")]
        public ActionResult ByReleaseYear(int year)
        {
            return Content(year + " Releases");
        }

        [Route("movies/details/{id}")]
        public ActionResult Details(int id)
        {
            var movies = GetMovies();
            var movie = movies.SingleOrDefault(c => c.Id == id);

            if (movie == null)
                return HttpNotFound();

            return View(movie);
        }

        private List<Movie> GetMovies()
        {
            var movies = new List<Movie>() {
                new Movie() { Id = 1, Name = "Shrek!" },
                new Movie() { Id = 2, Name = "Wall-e" }
            };

            return movies;
        }
    }
}