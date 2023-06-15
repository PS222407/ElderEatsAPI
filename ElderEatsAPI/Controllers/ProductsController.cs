using Microsoft.AspNetCore.Mvc;
using ElderEatsAPI.Models;
using ElderEatsAPI.Interfaces;
using AutoMapper;
using ElderEatsAPI.Dto;
using ElderEatsAPI.Middleware;
using ElderEatsAPI.Requests;
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
    
    [AuthFilter]
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

    [AccountAuthFilter("account")]
    [HttpGet("Account")]
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
    
        PaginateDto<Product> paginateDto = _productRepository.GetActiveProductsFromAccount(take * (page - 1), take);

        PaginateDto<ProductViewModel> paginateDtoViewModel = new PaginateDto<ProductViewModel>();
        var productViewModels = _mapper.Map<List<ProductViewModel>>(paginateDto.Items);

        paginateDtoViewModel.Items = productViewModels;
        paginateDtoViewModel.Count = paginateDto.Count;

        PaginatedViewModel<ProductViewModel> paginatedViewModel = new PaginatedViewModel<ProductViewModel>(
            paginateDtoViewModel,
            page,
            take,
            null,
            HttpContext,
            Url
        );
        
        if (page > paginatedViewModel.Paginate.LastPage)
        {
            return BadRequest("Given page is larger than MaxPage");
        }

        return Ok(paginatedViewModel);
    }
    
    // products from account with count of products ordered by expiration date paginate 4 with search
    [HttpGet("/abc/def/ghi/{name}", Name = "SearchProductsFromAccountPaginated")]
    public IActionResult SearchProductsFromAccountPaginated([FromRoute] string? name, int take, int page)
    {
        if (page <= 0)
        {
            return BadRequest("Page cannot be 0 or less");
        }

        if (take <= 0)
        {
            return BadRequest("Take cannot be 0 or less");
        }
        
        PaginateDto<ProductGroupedDto> paginateDto = _productRepository.GetProductsFromAccountPaginated(name, take * (page - 1), take);

        PaginateDto<ProductGroupedViewModel> paginateDtoViewModel = new PaginateDto<ProductGroupedViewModel>();
        List<ProductGroupedViewModel> productGroupedViewModels = new List<ProductGroupedViewModel>();
        foreach (ProductGroupedDto item in paginateDto.Items)
        {
            ProductGroupedViewModel productGroupedViewModel = new ProductGroupedViewModel
            {
                Product = _mapper.Map<ProductViewModel>(item.Product),
                Count = item.Count,
            };
            productGroupedViewModels.Add(productGroupedViewModel);
        }
        paginateDtoViewModel.Items = productGroupedViewModels;
        paginateDtoViewModel.Count = paginateDto.Count;
        
        PaginatedViewModel<ProductGroupedViewModel> paginatedViewModel = new PaginatedViewModel<ProductGroupedViewModel>(
            paginateDtoViewModel,
            page,
            take,
            name,
            HttpContext,
            Url
        );
        
        if (page > paginatedViewModel.Paginate.LastPage)
        {
            return BadRequest("Given page is larger than MaxPage");
        }

        return Ok(paginatedViewModel);
    }

    [AuthFilter]
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

    [AuthFilter]
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
    
        PaginateDto<Product> paginateDto = _productRepository.SearchProductsByNamePaginated(name, take * (page - 1), take);

        PaginateDto<ProductViewModel> paginateDtoViewModel = new PaginateDto<ProductViewModel>();
        var productViewModels = _mapper.Map<List<ProductViewModel>>(paginateDto.Items);
        
        paginateDtoViewModel.Items = productViewModels;
        paginateDtoViewModel.Count = paginateDto.Count;

        PaginatedViewModel<ProductViewModel> paginatedViewModel = new PaginatedViewModel<ProductViewModel>(
            paginateDtoViewModel,
            page,
            take,
            null,
            HttpContext,
            Url
        );

        return Ok(paginatedViewModel);
    }

    [AuthFilter] 
    [HttpGet("Product/Barcode/{barcode}")]
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
    [HttpGet("Product/Connection/{connectionID}")]
    public IActionResult GetProductByConnectionID(int connectionID)
    {
        ProductViewModel productDto = _mapper.Map<ProductViewModel>(_productRepository.GetProductByConnectionID(connectionID));

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

    [AuthFilter]
    [HttpPost("Product/Image")]
    public IActionResult StoreImageOfProduct(ProductImageRequest pir)
    {   
        if (!_productRepository.StoreProductImageLink(pir.Id,pir.Image))
        {
            ModelState.AddModelError("", "Error while creating product to database");

            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [AuthFilter]
    [HttpPost]
    public IActionResult StoreProduct(ProductStoreRequest productStoreRequest)
    {
        Product product = _mapper.Map<Product>(productStoreRequest);

        if (!_productRepository.StoreProduct(product))
        {
            ModelState.AddModelError("", "Error while creating product to database");

            return StatusCode(500, ModelState);
        }

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    [AuthFilter]
    [HttpPut("Account/{accountProductId:int}/ExpirationDate")]
    public IActionResult UpdateProductExpirationDateFromAccountById([FromRoute] int accountProductId, [FromBody] AccountProductUpdateRequest accountProductUpdateRequest)
    {
        if (!_productRepository.UpdateProductExpirationDateFromAccountById(accountProductId, accountProductUpdateRequest.ExpirationDate))
        {
            return BadRequest("Error while updating product, check if the accountProductId is valid");
        }

        return NoContent();
    }
    
    [AuthFilter]
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