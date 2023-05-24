using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElderEatsAPI.Models;
using ElderEatsAPI.Data;
using ElderEatsAPI.Interfaces;
using AutoMapper;
using ElderEatsAPI.Dto;

namespace ElderEatsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly DataContext _context;

        private readonly IProductRepository _productRepository;

        private readonly IMapper _mapper;

        public ProductsController(DataContext context, IProductRepository productRepository, IMapper mapper)
        {
            _context = context;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            List<ProductDto> productsDto = _mapper.Map<List<ProductDto>>(_productRepository.GetProducts());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(productsDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            ProductDto productDto = _mapper.Map<ProductDto>(_productRepository.GetProduct(id));

            if (productDto == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(productDto);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct([FromQuery] long id, [FromBody] Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public IActionResult StoreProduct(ProductDto productDto)
        {
            if (productDto == null)
                return BadRequest();

            Product product = _mapper.Map<Product>(productDto);

            bool isSuccess = _productRepository.StoreProduct(product);
            if (!isSuccess)
            {
                ModelState.AddModelError("", "Error while creating product to database");

                return StatusCode(500, ModelState);
            }
            
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            return Ok("Product successfully created");
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(long id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
