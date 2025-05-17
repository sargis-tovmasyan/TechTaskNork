using Microsoft.AspNetCore.Mvc;
using TechTask.Api.Interfaces;
using TechTask.Api.Models;

namespace TechTask.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SuppliersController : ControllerBase
    {
        private readonly ISuppliersService _suppliersService;

        public SuppliersController(ISuppliersService suppliersService)
        {
            _suppliersService = suppliersService;
        }

        // GET: api/Suppliers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> Get()
        {
            var suppliers = await _suppliersService.GetAllAsync();

            return Ok(suppliers);
        }

        // GET: api/Suppliers/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplierById(int id)
        {
            var supplier = await _suppliersService.GetByIdAsync(id);

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
            var posted = await _suppliersService.PostAsync(supplier);
            if (!posted) return Problem("Could not create supplier."); // something went wrong

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

            var updated = await _suppliersService.UpdateAsync(updSupplier);
            if (!updated) return NotFound(); // nothing to update

            return NoContent();
        }

        // DELETE: api/Suppliers/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSupplierById(int id)
        {
            var deleted = await _suppliersService.DeleteByIdAsync(id);
            if (!deleted) return NotFound(); // nothing to delete

            return NoContent();
        }
    }
}
