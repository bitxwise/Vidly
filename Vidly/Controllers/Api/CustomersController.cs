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
            return _context.Customers.ToList().Select(Mapper.Map<Customer, CustomerData>);
        }

        // GET /api/customers/{id}
        public CustomerData GetCustomer(int id)
        {
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);

            return Mapper.Map<Customer, CustomerData>(customer);
        }

        // POST /api/customers
        [HttpPost]
        public CustomerData CreateCustomer(CustomerData customerData)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            var customer = Mapper.Map<CustomerData, Customer>(customerData);

            _context.Customers.Add(customer);
            _context.SaveChanges();

            customerData.Id = customer.Id;

            return customerData;
        }

        // PUT /api/customers/{id}
        [HttpPut]
        public void UpdateCustomer(int id, CustomerData customerData)
        {
            if (!ModelState.IsValid)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

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
    }
}