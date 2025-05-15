using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechTask.Api.Database;
using TechTask.Api.Models;

namespace TechTask.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public TransactionsController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> Get()
        {
            var transactions = await _dbContext.Transactions
                .AsNoTracking()
                .Include(t => t.Product)
                .ToListAsync();

            return Ok(transactions);
        }

        // GET: api/Transactions/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransactionById(int id)
        {
            var transaction = await _dbContext.Transactions
                .AsNoTracking()
                .Include(t => t.Product)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        // POST: api/Transactions
        [HttpPost]
        public async Task<ActionResult<Transaction>> PostTransaction([FromBody] Transaction transaction)
        {
            var product = await _dbContext.Products.FindAsync(transaction.ProductId);
            if (product == null)
            {
                return BadRequest($"Invalid {nameof(transaction.ProductId)}");
            }

            switch (transaction.Type)
            {
                case TransactionType.Sale:
                    if (product.StockQuantity < transaction.Quantity)
                    {
                        return BadRequest($"Not enough stock for product {product.Name}");
                    }

                    product.StockQuantity -= transaction.Quantity;
                    break;
                case TransactionType.Purchase:
                    product.StockQuantity += transaction.Quantity;
                    break;
                default:
                    return BadRequest($"Invalid {nameof(transaction.Type)}");
            }

            try
            {
                await _dbContext.Transactions.AddAsync(transaction);
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                return Problem(exception.Message);
            }

            return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
        }

        // DELETE: api/Transactions/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTransactionById(int id)
        {
            var transaction = await _dbContext.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            _dbContext.Transactions.Remove(transaction);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
