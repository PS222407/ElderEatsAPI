using Microsoft.AspNetCore.Mvc;
using ElderEatsAPI.Models;
using ElderEatsAPI.Interfaces;
using AutoMapper;
using ElderEatsAPI.Dto;
using ElderEatsAPI.Middleware;
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

    [AccountAuthFilter("employee")]
    [HttpGet("Account/{id:int}")]
    public IActionResult GetActiveProductsFromAccount(int take, int page)
    {
        if (page <= 0)
        {
            return BadRequest("Page cannot be 0 or less");
        }

        if (take <= 0)
        {
            return BadRequest("Take cannot be 0 or less");
        }

        ProductPaginateDto productPaginateDto =
            _productRepository.GetActiveProductsFromAccount(take * (page - 1), take);

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

    [HttpGet("{id:int}")]
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

    [HttpGet("Search/{name}")]
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

        ProductPaginateDto productPaginateDto = _productRepository.SearchProductsByNamePaginated(name, take * (page - 1), take);

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

    [HttpGet("Product/{barcode}")]
    public IActionResult GetProductByBarcode(string barcode)
    {
        ProductViewModel productViewModel = _mapper.Map<ProductViewModel>(_productRepository.GetProductByBarcode(barcode));

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

    [HttpPut("Account/{accountProductId:int}/ExpirationDate")]
    public IActionResult UpdateProductExpirationDateFromAccountById([FromRoute] int accountProductId, [FromBody] AccountProductUpdateRequest accountProductUpdateRequest)
    {
        if (!_productRepository.UpdateProductExpirationDateFromAccountById(accountProductId, accountProductUpdateRequest.ExpirationDate))
        {
            return BadRequest("Error while updating product, check if the accountProductId is valid");
        }

        return NoContent();
    }


    [HttpDelete("Account/{accountProductId:int}")]
    public IActionResult DeleteProductFromAccountById(int accountProductId)
    {
        if (!_productRepository.DeleteProductFromAccountById(accountProductId))
        {
            return BadRequest("Error while deleting product, check if the accountProductId is valid");
        }

        return NoContent();
    }
}