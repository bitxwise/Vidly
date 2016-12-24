using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        // GET: Customers
        public ActionResult Index()
        {
            var customers = GetCustomers();

            return View(customers);
        }

        [Route("customers/details/{id}")]
        public ActionResult Details(int id)
        {
            var customers = GetCustomers();
            var customer = customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return HttpNotFound();

            return View(customer);
        }

        private List<Customer> GetCustomers()
        {
            var customers = new List<Customer>() {
                new Customer() { Id = 1,  Name = "John Smith" },
                new Customer() { Id = 2, Name = "Jane Smith" }
            };

            return customers;
        }
    }
}