using AutoMapper;
using System;
using System.Collections.Generic;
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
            return _context.Movies.ToList().Select(Mapper.Map<Movie, MovieData>);
        }

        // GET /api/movies/{id}
        public IHttpActionResult GetMovie(int id)
        {
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);

            if (movie == null)
                return NotFound();

            var movieData = Mapper.Map<Movie, MovieData>(movie);

            return Ok(movieData);
        }

        // POST /api/movies
        [HttpPost]
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
    }
}
