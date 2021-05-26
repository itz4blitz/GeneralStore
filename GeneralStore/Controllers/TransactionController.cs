using GeneralStore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace GeneralStore.Controllers
{
    public class TransactionController : ApiController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        [HttpPost]
        public async Task<IHttpActionResult> Post(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                Product product = await _context.Products.FindAsync(transaction.ProductId);
                if (product == null)
                {
                    return BadRequest("Invalid product Id");
                }

                Customer customer = await _context.Customers.FindAsync(transaction.CustomerId);
                if (customer == null)
                {
                    return BadRequest("Invalid Customer Id");
                }

                if (transaction.PurchaseQuantity > product.Quantity)
                    return BadRequest($"There are only {product.Quantity} left in stock!");
            }
            transaction.DateOfTransaction = DateTime.Now;
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Transaction> transactions = await _context.Transactions.ToListAsync();
            return Ok(transactions);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetById([FromUri] int Id)
        {
            Transaction transaction = await _context.Transactions.FindAsync(Id);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

        [HttpPut]
        public async Task<IHttpActionResult> UpdateTransaction([FromUri] int id, [FromBody] Transaction newTransaction)
        {
            if (ModelState.IsValid)
            {
                Product product = await _context.Products.FindAsync(newTransaction.ProductId);
                if (product == null)
                {
                    return BadRequest("Invalid product Id");
                }

                Customer customer = await _context.Customers.FindAsync(newTransaction.CustomerId);
                if (customer == null)
                {
                    return BadRequest("Invalid Customer Id");
                }

                Transaction oldTransaction = await _context.Transactions.FindAsync(id);
                if (oldTransaction != null)
                {
                    int difference = oldTransaction.PurchaseQuantity - newTransaction.PurchaseQuantity;
                    if (difference > product.Quantity) return BadRequest($"There are only {product.Quantity} left in stock");

                    product.Quantity += difference;
                    oldTransaction.ProductId = newTransaction.ProductId;
                    oldTransaction.PurchaseQuantity = newTransaction.PurchaseQuantity;
                    oldTransaction.CustomerId = newTransaction.CustomerId;
                    if(newTransaction.DateOfTransaction != null)
                    {
                        oldTransaction.DateOfTransaction = newTransaction.DateOfTransaction;
                    }
                    await _context.SaveChangesAsync();
                    return Ok(oldTransaction);
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }
    }
}
