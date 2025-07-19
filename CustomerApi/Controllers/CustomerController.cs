using CustomerApi.Data;
using CustomerApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Restrict access to authenticated users
    public class CustomersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CustomersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/customers
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PostCustomer(Customer customer)
        {
            // Validate that CustomerNumber is unique
            if (_context.Customers.Any(c => c.CustomerNumber == customer.CustomerNumber))
            {
                return BadRequest("Customer Number must be unique.");
            }

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }

        // GET: api/customers/5
        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }


        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> PutCustomer(int id, Customer updatedCustomer)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            // Update fields
            customer.CustomerNumber = updatedCustomer.CustomerNumber;
            customer.CustomerName = updatedCustomer.CustomerName;
            customer.DateOfBirth = updatedCustomer.DateOfBirth;
            customer.Gender = updatedCustomer.Gender;

            await _context.SaveChangesAsync();
            return NoContent();
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return NotFound();

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}
