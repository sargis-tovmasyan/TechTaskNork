using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechTask.Api.Database;
using TechTask.Api.Models;

namespace TechTask.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public CategoriesController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> Get()
        {
            var categories = await _dbContext.Categories
                .AsNoTracking()
                .Include(c => c.Products)
                .ToListAsync();

            return Ok(categories);
        }

        // GET: api/Categories/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            var category = await _dbContext.Categories
                .AsNoTracking()
                .Include(c => c.Products)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory([FromBody] Category category)
        {
            try
            {
                await _dbContext.AddAsync(category);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                return Problem(exception.Message);
            }

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        // PUT: api/Categories/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category updCategory)
        {
            if (id != updCategory.Id)
            {
                return BadRequest(); // ID mismatch between URL and body
            }

            var existingEntity = await _dbContext.Categories.FindAsync(updCategory.Id);
            if (existingEntity == null) return NotFound(); //Nothing to update

            _dbContext.Entry(existingEntity).CurrentValues.SetValues(updCategory);

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

        // DELETE: api/Categories/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategoryById(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
