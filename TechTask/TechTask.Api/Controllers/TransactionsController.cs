using Microsoft.AspNetCore.Mvc;
using TechTask.Api.Interfaces;
using TechTask.Api.Models;

namespace TechTask.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsService _transactionsService;

        public TransactionsController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> Get()
        {
            var transactions = await _transactionsService.GetAllAsync();

            return Ok(transactions);
        }

        // GET: api/Transactions/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransactionById(int id)
        {
            var transaction = await _transactionsService.GetByIdAsync(id);

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
            var result = await _transactionsService.PostAsync(transaction);
            if (!result) return Problem("Could not create Transaction."); // something went wrong

            return CreatedAtAction(nameof(GetTransactionById), new { id = transaction.Id }, transaction);
        }

        // DELETE: api/Transactions/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTransactionById(int id)
        {
            var deleted = await _transactionsService.DeleteByIdAsync(id);
            if (!deleted) return NotFound(); // nothing to delete

            return NoContent();
        }
    }
}