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
    public class ProductController : ApiController
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        [HttpPost]
        public async Task<IHttpActionResult> Post(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest(ModelState);
        }

        [HttpGet]

        public async Task<IHttpActionResult> GetAllProducts()
        {
            List<Product> products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetProductById([FromUri]int id)
        {
            Product product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IHttpActionResult> UpdateProduct([FromUri] int id, [FromBody] Product newProduct)
        {
            if (ModelState.IsValid)
            {
                Product oldProduct = await _context.Products.FindAsync(id);
                if(oldProduct != null)
                {
                    oldProduct.Name = newProduct.Name;
                    oldProduct.Price = newProduct.Price;
                    oldProduct.Quantity = newProduct.Quantity;
                    oldProduct.UPC = newProduct.UPC;
                    await _context.SaveChangesAsync();
                    return Ok(oldProduct);
                }
                return NotFound();
            }
            return BadRequest(ModelState);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> Delete([FromUri] int id)
        {
            Product product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                return NotFound();
            }
            _context.Products.Remove(product);

            await _context.SaveChangesAsync();
            return Ok("Product deleted successfully!");
        }

    }
}
