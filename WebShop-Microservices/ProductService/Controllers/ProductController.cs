using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Models;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductDbContext _productDbContext;

        public ProductController(ProductDbContext customerDbContext)
        {
            _productDbContext = customerDbContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Product>> GetCustomers()
        {
            return _productDbContext.Products;
        }

        [HttpGet("{productId:int}")]
        public async Task<ActionResult<Product>> GetById(int productId)
        {
            var product = await _productDbContext.Products.FindAsync(productId);
            return product;
        }

        [HttpPost]
        public async Task<ActionResult> Create(Product product)
        {
            _productDbContext.Products.AddAsync(product);
            await _productDbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Update(Product product)
        {
            _productDbContext.Products.Update(product);
            await _productDbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{productId:int}")]
        public async Task<ActionResult> Delete(int productId)
        {
            var product = await _productDbContext.Products.FindAsync(productId);
            _productDbContext.Products.Remove(product);
            await _productDbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
