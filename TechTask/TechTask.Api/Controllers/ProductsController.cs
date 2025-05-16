using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechTask.Api.Database;
using TechTask.Api.Models;
using TechTask.Api.Services;

namespace TechTask.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly ProductService _productService;

        public ProductsController(AppDbContext dbContext, ProductService productService)
        {
            _dbContext = dbContext;
            _productService = productService;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var products = await _productService.GetProductsAsync();

            return Ok(products);
        }

        // GET: api/Products/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

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
            var posted = await _productService.PostProductAsync(product);
            if (!posted) return Problem("Could not create product."); // something went wrong

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        // PUT: api/Products/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product updProduct)
        {
            if (id != updProduct.Id) return BadRequest(); // ID mismatch between URL and body

            var updated = await _productService.UpdateProductAsync(updProduct);
            if (!updated) return NotFound(); // nothing to update

            return NoContent();
        }

        // DELETE: api/Products/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductById(int id)
        {
            var deleted = await _productService.DeleteProductByIdAsync(id);
            if (!deleted) return NotFound(); // nothing to delete

            return NoContent();
        }
    }
}
