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
            var movieQuery = _context.Movies
                .Include(m => m.Genre);

            var queries = Request.GetQueryNameValuePairs();
            var query = queries.FirstOrDefault(q => q.Key.Equals(QueryKeys.Query));
            var availablyOnly = queries.FirstOrDefault(q => q.Key.Equals(QueryKeys.AvailableOnly));

            if (!string.IsNullOrWhiteSpace(query.Value))
                movieQuery = movieQuery.Where(m => m.Name.Contains(query.Value));

            // optimistically assume that the query value is internally provided and parseable
            if (!string.IsNullOrWhiteSpace(availablyOnly.Value) && bool.Parse(availablyOnly.Value))
                movieQuery = movieQuery.Where(m => m.NumberAvailable > 0);

            var movies = movieQuery
                .ToList()
                .Select(Mapper.Map<Movie, MovieData>);

            return movies;
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

        public static class QueryKeys
        {
            public static readonly string Query = "query";
            public static readonly string AvailableOnly = "available";
        }
    }
}
