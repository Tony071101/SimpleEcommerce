using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce.Models;
using SimpleEcommerce.Models.Dtos.Product;
using SimpleEcommerce.Models.Entities;
using SimpleEcommerce.Services.Interfaces;

namespace SimpleEcommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService productService;
        private readonly IMapper mapper;
        public ProductsController(IProductService productService, IMapper mapper)
        {
            this.productService = productService;
            this.mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAllProducts()
        {
            var products = await productService.GetAllProductSync();
            var productDtos = mapper.Map<IEnumerable<ProductResponseDto>>(products);
            return Ok(productDtos);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<ActionResult<ProductResponseDto>> GetProductById(Guid id)
        {
            var product = await productService.GetProductByIdSync(id);
            if (product == null) return NotFound();
            var productDto = mapper.Map<ProductResponseDto>(product);
            return Ok(productDto);
        }

        [HttpPost]
        public async Task<ActionResult<ProductResponseDto>> AddProduct([FromBody] CreateProductRequestDto createProductDto)
        {
            var productToAdd = mapper.Map<Product>(createProductDto);
            productToAdd.ProductID = Guid.NewGuid();

            var newProduct = await productService.AddProductSync(productToAdd);

            var newProductDto = mapper.Map<ProductResponseDto>(newProduct);
            return CreatedAtAction(nameof(GetProductById), new { id = newProduct.ProductID }, newProduct);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProductResponseDto>> UpdateProduct(Guid id, [FromBody] ProductResponseDto productDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            var productToUpdate = mapper.Map<Product>(productDto);
            var updatedProduct = await productService.UpdateProductSync(id, productToUpdate);
            if (updatedProduct == null) return NotFound();

            var updatedProductDto = mapper.Map<ProductResponseDto>(updatedProduct);
            return Ok(updatedProductDto);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var deleteProduct = await productService.DeleteProductSync(id);
            return deleteProduct ? Ok() : NotFound();
        }
    }
}