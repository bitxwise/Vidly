using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.Data;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    public class MoviesController : ApiController
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

        // GET /api/movies
        public IEnumerable<MovieData> GetMovies()
        {
            return _context.Movies
                .Include(m => m.Genre)
                .ToList()
                .Select(Mapper.Map<Movie, MovieData>);
        }

        // GET /api/movies/{id}
        public IHttpActionResult GetMovie(int id)
        {
            var movie = _context.Movies
                .Include(m => m.Genre)
                .SingleOrDefault(m => m.Id == id);

            if (movie == null)
                return NotFound();

            var movieData = Mapper.Map<Movie, MovieData>(movie);

            return Ok(movieData);
        }

        // POST /api/movies
        [HttpPost]
        [Authorize(Roles = RoleName.CanManageMovies)]
        public IHttpActionResult CreateMovie(MovieData movieData)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var movie = Mapper.Map<MovieData, Movie>(movieData);

            _context.Movies.Add(movie);
            _context.SaveChanges();

            movieData.Id = movie.Id;

            return Created(new Uri(Request.RequestUri + "/" + movie.Id), movieData);
        }

        // PUT /api/movies/{id}
        [HttpPut]
        [Authorize(Roles = RoleName.CanManageMovies)]
        public void UpdateMovie(int id, MovieData movieData)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            if (id != movieData.Id)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    ReasonPhrase = "Cannot update movie ID."
                });

            var existingmovie = _context.Movies.SingleOrDefault(m => m.Id == id);

            if (existingmovie == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Mapper.Map(movieData, existingmovie);

            _context.SaveChanges();
        }

        // DELETE /api/movies/{id}
        [HttpDelete]
        [Authorize(Roles = RoleName.CanManageMovies)]
        public void DeleteMovie(int id)
        {
            var existingMovie = _context.Movies.SingleOrDefault(m => m.Id == id);

            if (existingMovie == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.Movies.Remove(existingMovie);
            _context.SaveChanges();
        }
    }
}
