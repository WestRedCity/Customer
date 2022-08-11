using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("/")]
    public class CustomerController : ControllerBase
    {
        private static List<Customer> Customers = new List<Customer>();

        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ILogger<CustomerController> logger)
        {
            _logger = logger;
        }

        [HttpGet("customer/{customerid}/score/{score}")]
        public IActionResult UpdateScore([FromRoute] long customerId, [FromRoute] decimal score)
        {
            Customer customer = Customers.Where(p => p.CustomerId == customerId).First();
            if (customer == null)
            {
                SortCustomer.HalfSort(Customers, new Customer
                {
                    CustomerId = customerId,
                    Score = score,
                    Rank = Customers.Count() + 1//先放到最后
                });
            }
            else
            {
                Customers.RemoveAt(customer.Rank-1);
                customer.Score += score;
                SortCustomer.HalfSort(Customers, customer);
            }
            
            return new JsonResult(new
            {
                CustomerId = customerId,
                Score = customer.Score,
            });
        }

        [HttpGet("leaderboard")]
        public IActionResult GetCustomersByRank([FromQuery] int start, [FromQuery] int end)
        {
            if (start > end)
            {
                return new JsonResult(null);
            }
            var list = Customers.Where(p => p.Rank >= start && p.Rank <= end);
            return new JsonResult(list);
        }

        [HttpGet("leaderboard/{customerid}")]
        public IActionResult GetCustomersByCustomerId([FromRoute] long customerId, [FromQuery] int high, [FromQuery] int low)
        {
            Customer customer = Customers.Where(p => p.CustomerId == customerId).First();
            List<Customer> partCustomers = new List<Customer>();
            if (customer == null)
            {
                return new JsonResult(null);
            }
            else
            {
                int startIndex = (customer.Rank - 1 - high);
                int size = high + low + 1;
                partCustomers = Customers.Skip(startIndex < 0 ? 0 : startIndex).Take(size).ToList();
            }
            return new JsonResult(new
            {
                Customers = partCustomers
            });
        }

    }
}
