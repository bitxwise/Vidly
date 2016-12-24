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
            var movies = new List<Movie>() {
                new Movie() { Name = "Shrek!" },
                new Movie() { Name = "Wall-e" }
            };

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
    }
}