using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechTask.Api.Database;
using TechTask.Api.Models;

namespace TechTask.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public ProductsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var products = await _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .ToListAsync();

            return Ok(products);
        }

        // GET: api/Products/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromBody] Product product)
        {
            try
            {
                await _dbContext.AddAsync(product);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                return Problem(exception.Message);
            }

            return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
        }

        // PUT: api/Products/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product updProduct)
        {
            if (id != updProduct.ProductId)
            {
                return BadRequest(); // ID mismatch between URL and body
            }

            var existingEntity = await _dbContext.Products.FindAsync(updProduct.ProductId);
            if (existingEntity == null) return NotFound(); //Nothing to update

            existingEntity.Name = updProduct.Name;
            existingEntity.Price = updProduct.Price;
            existingEntity.StockQuantity = updProduct.StockQuantity;
            existingEntity.CategoryID = updProduct.CategoryID;
            existingEntity.SupplierId = updProduct.SupplierId;

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

        // DELETE: api/Products/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductById(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
