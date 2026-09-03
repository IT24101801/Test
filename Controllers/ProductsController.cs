using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Models;

namespace ProductApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var products = await _context.Products.ToListAsync();

        return Ok(products);
    }

    // GET: api/products/1
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound(new
            {
                message = "Product not found"
            });
        }

        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        _context.Products.Add(product);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetProduct),
            new { id = product.Id },
            product
        );
    }

    // PUT: api/products/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        if (id != product.Id)
        {
            return BadRequest(new
            {
                message = "Product ID does not match"
            });
        }

        var existingProduct = await _context.Products.FindAsync(id);

        if (existingProduct == null)
        {
            return NotFound(new
            {
                message = "Product not found"
            });
        }

        existingProduct.Name = product.Name;
        existingProduct.Price = product.Price;
        existingProduct.Stock = product.Stock;

        await _context.SaveChangesAsync();

        return Ok(existingProduct);
    }

    // DELETE: api/products/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound(new
            {
                message = "Product not found"
            });
        }

        _context.Products.Remove(product);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            message = "Product deleted successfully"
        });
    }
}