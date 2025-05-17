using Microsoft.AspNetCore.Mvc;
using TechTask.Api.Interfaces;
using TechTask.Api.Models;

namespace TechTask.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _productService;

        public ProductsController(IProductsService productService)
        {
            _productService = productService;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> Get()
        {
            var products = await _productService.GetAllAsync();

            return Ok(products);
        }

        // GET: api/Products/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productService.GetByIdAsync(id);

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
            var posted = await _productService.PostAsync(product);
            if (!posted) return Problem("Could not create product."); // something went wrong

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        // PUT: api/Products/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product updProduct)
        {
            if (id != updProduct.Id) return BadRequest(); // ID mismatch between URL and body

            var updated = await _productService.UpdateAsync(updProduct);
            if (!updated) return NotFound(); // nothing to update

            return NoContent();
        }

        // DELETE: api/Products/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductById(int id)
        {
            var deleted = await _productService.DeleteByIdAsync(id);
            if (!deleted) return NotFound(); // nothing to delete

            return NoContent();
        }
    }
}
