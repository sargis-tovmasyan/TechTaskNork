using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechTask.Api.Database;
using TechTask.Api.Models;

namespace TechTask.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public SuppliersController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Suppliers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> Get()
        {
            var suppliers = await _dbContext.Suppliers
                .AsNoTracking()
                .Include(s => s.Products)
                .ToListAsync();

            return Ok(suppliers);
        }

        // GET: api/Suppliers/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplierById(int id)
        {
            var supplier = await _dbContext.Suppliers
                .AsNoTracking()
                .Include(s => s.Products)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (supplier == null)
            {
                return NotFound();
            }

            return Ok(supplier);
        }

        // POST: api/Suppliers
        [HttpPost]
        public async Task<ActionResult<Supplier>> PostSupplier([FromBody] Supplier supplier)
        {
            try
            {
                await _dbContext.AddAsync(supplier);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                return Problem(exception.Message);
            }

            return CreatedAtAction(nameof(GetSupplierById), new { id = supplier.Id }, supplier);
        }

        // PUT: api/Suppliers/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSupplier(int id, [FromBody] Supplier updSupplier)
        {
            if (id != updSupplier.Id)
            {
                return BadRequest(); // ID mismatch between URL and body
            }

            var existingEntity = await _dbContext.Suppliers.FindAsync(updSupplier.Id);
            if (existingEntity == null) return NotFound(); //Nothing to update

            _dbContext.Entry(existingEntity).CurrentValues.SetValues(updSupplier);

            //Not using this because its update all the properties even they are not changed
            //Context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                return Problem(exception.Message);
            }

            return NoContent();
        }

        // DELETE: api/Suppliers/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSupplierById(int id)
        {
            var supplier = await _dbContext.Suppliers.FindAsync(id);

            if (supplier == null)
            {
                return NotFound();
            }

            _dbContext.Suppliers.Remove(supplier);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
