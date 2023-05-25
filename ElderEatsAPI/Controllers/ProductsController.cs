using Microsoft.AspNetCore.Mvc;
using ElderEatsAPI.Models;
using ElderEatsAPI.Interfaces;
using AutoMapper;
using ElderEatsAPI.Dto;
using ElderEatsAPI.ViewModels;

namespace ElderEatsAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    private readonly IMapper _mapper;

    public ProductsController(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetProducts()
    {
        List<ProductViewModel> productsDto = _mapper.Map<List<ProductViewModel>>(_productRepository.GetProducts());

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(productsDto);
    }

    [HttpGet("{id}")]
    public IActionResult GetProduct(int id)
    {
        ProductViewModel productViewModel = _mapper.Map<ProductViewModel>(_productRepository.GetProduct(id));

        if (productViewModel == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return Ok(productViewModel);
    }

    [HttpGet("search/{name}")]
    public IActionResult SearchProductsPaginated(string? name, int take, int page)
    {
        if (page <= 0)
        {
            return BadRequest("Page cannot be 0 or less");
        }
        if (take <= 0)
        {
            return BadRequest("Take cannot be 0 or less");
        }

        var productPaginateDto = _productRepository.SearchProductsByNamePaginated(name, take * (page - 1), take);

        int maxPages = (int)Math.Ceiling(productPaginateDto.Count / (float)take);
        if (page > maxPages)
        {
            return BadRequest("Given page is larger than MaxPage");
        }

        PaginatedViewModel<ProductViewModel> productPaginatedViewModel = new PaginatedViewModel<ProductViewModel>(
            _mapper.Map<List<ProductViewModel>>(productPaginateDto.Products),
            page,
            maxPages
        );

        return Ok(productPaginatedViewModel);
    }
    
    [HttpGet("product/{barcode}")]
    public IActionResult GetProductByBarcode(string barcode)
    {
        ProductDto productDto = _mapper.Map<ProductDto>(_productRepository.GetProductByBarcode(barcode));

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

    // // PUT: api/Products/5
    // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // [HttpPut("{id}")]
    // public async Task<IActionResult> PutProduct([FromQuery] long id, [FromBody] Product product)
    // {
    //     if (id != product.Id)
    //     {
    //         return BadRequest();
    //     }
    //
    //     _context.Entry(product).State = EntityState.Modified;
    //
    //     try
    //     {
    //         await _context.SaveChangesAsync();
    //     }
    //     catch (DbUpdateConcurrencyException)
    //     {
    //         if (!ProductExists(id))
    //         {
    //             return NotFound();
    //         }
    //         else
    //         {
    //             throw;
    //         }
    //     }
    //
    //     return NoContent();
    // }

    [HttpPost]
    public IActionResult StoreProduct(ProductPostDto productPostDto)
    {
        Product product = _mapper.Map<Product>(productPostDto);

        if (!_productRepository.StoreProduct(product))
        {
            ModelState.AddModelError("", "Error while creating product to database");

            return StatusCode(500, ModelState);
        }

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    // [HttpDelete]
    // public IActionResult DeleteProduct(string barcode)
    // {
    //     Product product = _productRepository.GetProductByBarcode(barcode);
    //    
    //     
    //     if (product == null)
    //     {
    //         return NotFound();
    //     }
    //
    //     if (!ModelState.IsValid)
    //     {
    //         return BadRequest(ModelState);
    //     }
    //
    //     if (! _productRepository.DeleteProduct(product))
    //     {
    //         ModelState.AddModelError("", "Error while deleting product to database");
    //         
    //         return StatusCode(500, ModelState);
    //     }
    //
    //     return NoContent();
    //
    // }
 
}