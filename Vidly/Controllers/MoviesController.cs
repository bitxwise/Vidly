﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;

        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Index()
        {
            return User.IsInRole(RoleName.CanManageMovies)
                ? View("List")
                : View("ReadOnlyList");
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

        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult New()
        {
            var genres = _context.Genres.ToList();
            var viewModel = new MovieFormViewModel() {
                Genres = genres
            };

            return View("MovieForm", viewModel);
        }

        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult Edit(int id)
        {
            var movie = _context.Movies.Single(c => c.Id == id);

            if (movie == null)
                return HttpNotFound();

            var viewModel = new MovieFormViewModel(movie)
            {
                Genres = _context.Genres.ToList()
            };

            return View("MovieForm", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult Save(Movie movie)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new MovieFormViewModel(movie)
                {
                    Genres = _context.Genres.ToList()
                };

                return View("MovieForm", viewModel);
            }

            if (movie.Id == 0)
            {
                movie.AddedDate = DateTime.Now;
                _context.Movies.Add(movie);
            }
            else
            {
                // TODO: Replace individual property setting with mapping (e.g. AutoMapper)
                var existingMovie = _context.Movies.Single(c => c.Id == movie.Id);
                existingMovie.GenreId = movie.GenreId;
                existingMovie.Name = movie.Name;
                existingMovie.NumberInStock = movie.NumberInStock;
                existingMovie.ReleaseDate = movie.ReleaseDate;
            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Movies");
        }
    }
}