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
    public class CustomersController : ApiController
    {
        private ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }

        // GET /api/customers
        public IEnumerable<CustomerData> GetCustomers()
        {
            var customerQuery = _context.Customers
                .Include(c => c.MembershipType);

            var queries = Request.GetQueryNameValuePairs();
            var query = queries.FirstOrDefault(q => q.Key.Equals(QueryKeys.Query));
            
            if (!string.IsNullOrWhiteSpace(query.Value))
                customerQuery = customerQuery.Where(c => c.Name.Contains(query.Value));

            var customers = customerQuery
                .ToList()
                .Select(Mapper.Map<Customer, CustomerData>);

            return customers;
        }

        // GET /api/customers/{id}
        public IHttpActionResult GetCustomer(int id)
        {
            var customer = _context.Customers
                .Include(c => c.MembershipType)
                .SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return NotFound();

            var customerData = Mapper.Map<Customer, CustomerData>(customer);

            return Ok(customerData);
        }

        // POST /api/customers
        [HttpPost]
        public IHttpActionResult CreateCustomer(CustomerData customerData)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var customer = Mapper.Map<CustomerData, Customer>(customerData);

            _context.Customers.Add(customer);
            _context.SaveChanges();

            customerData.Id = customer.Id;

            return Created(new Uri(Request.RequestUri + "/" + customer.Id), customerData);
        }

        // PUT /api/customers/{id}
        [HttpPut]
        public void UpdateCustomer(int id, CustomerData customerData)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            if (id != customerData.Id)
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest) {
                    ReasonPhrase = "Cannot update customer ID."
                });

            var existingCustomer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (existingCustomer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            Mapper.Map(customerData, existingCustomer);

            _context.SaveChanges();
        }

        // DELETE /api/customers/{id}
        [HttpDelete]
        public void DeleteCustomer(int id)
        {
            var existingCustomer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (existingCustomer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            _context.Customers.Remove(existingCustomer);
            _context.SaveChanges();
        }

        public static class QueryKeys
        {
            public static readonly string Query = "query";
            public static readonly string AvailableOnly = "available";
        }
    }
}