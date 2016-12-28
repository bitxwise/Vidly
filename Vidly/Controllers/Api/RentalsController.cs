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
    public class RentalsController : ApiController
    {
        private ApplicationDbContext _context;

        public RentalsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }

        // GET /api/rentals
        public IEnumerable<NewRentalData> GetRentals()
        {
            return _context.Rentals
                .ToList()
                .Select(Mapper.Map<Rental, NewRentalData>);
        }

        // POST /api/rentals
        [HttpPost]
        public IHttpActionResult CreateRental(NewRentalData newRentalData)
        {
            // Optimistically assume data will exist for internal API
            var customer = _context.Customers.Single(c => c.Id == newRentalData.CustomerId);
            var movies = _context.Movies.Where(m => newRentalData.MovieIds.Contains(m.Id)).ToList();

            // Edge Case Excluded: Customer rents multiple copies of the same movie is not considered here

            foreach(var movie in movies)
            {
                if (movie.NumberAvailable == 0)
                    return BadRequest(string.Format("There are no copies of [{0}] available for rent.", movie.Name));

                movie.NumberAvailable--;

                var rental = new Rental() {
                    Customer = customer,
                    Movie = movie,
                    DateRented = DateTime.Now
                };
                _context.Rentals.Add(rental);
            }

            _context.SaveChanges();

            return Ok();
        }
    }
}
