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
            if (!ModelState.IsValid)
                return BadRequest();

            // TODO: Create rentals without first retrieving customer and movie data from database
            //          Ssupplying IDs only results in validation errors for Customer and Movie for required fields
            var customer = _context.Customers.Single(c => c.Id == newRentalData.CustomerId);

            foreach(var movieId in newRentalData.MovieIds)
            {
                var movie = _context.Movies.Single(m => m.Id == movieId);

                var rental = new Rental() {
                    Customer = customer,
                    Movie = movie,
                    DateRented = DateTime.Now
                };
                _context.Rentals.Add(rental);
            }

            _context.SaveChanges();

            return Created(Request.RequestUri, newRentalData);
        }
    }
}
