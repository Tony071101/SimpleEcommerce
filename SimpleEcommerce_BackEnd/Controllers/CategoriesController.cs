using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleEcommerce_BackEnd.Models.Dtos.Category;
using SimpleEcommerce_BackEnd.Models.Entities;
using SimpleEcommerce_BackEnd.Services.Interfaces;

namespace SimpleEcommerce_BackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAllCategories()
        {
            var categories = await categoryService.GetAllCategoriesAsync();
            var categoryDtos = mapper.Map<IEnumerable<CategoryResponseDto>>(categories);
            return Ok(categoryDtos);
        }

        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryResponseDto>> GetCategoryById(Guid id)
        {
            var category = await categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var categoryDto = mapper.Map<CategoryResponseDto>(category);
            return Ok(categoryDto);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryResponseDto>> AddCategory([FromBody] CreateCategoryDto createCategoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryToAdd = mapper.Map<Category>(createCategoryDto); // Ánh xạ từ DTO input sang Entity

            var newCategory = await categoryService.AddCategoryAsync(categoryToAdd);
            var newCategoryDto = mapper.Map<CategoryResponseDto>(newCategory); // Ánh xạ từ Entity sang DTO output

            return CreatedAtAction(nameof(GetCategoryById), new { id = newCategory.CategoryID }, newCategoryDto);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CreateCategoryDto updateCategoryDto) // Có thể tái sử dụng CreateCategoryDto nếu logic cập nhật giống hệt
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryToUpdate = mapper.Map<Category>(updateCategoryDto);
            categoryToUpdate.CategoryID = id; // Đảm bảo ID được giữ nguyên

            var updatedCategory = await categoryService.UpdateCategoryAsync(id, categoryToUpdate);

            if (updatedCategory == null)
            {
                return NotFound();
            }

            var updatedCategoryDto = mapper.Map<CategoryResponseDto>(updatedCategory);
            return Ok(updatedCategoryDto);
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var success = await categoryService.DeleteCategoryAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}