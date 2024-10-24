using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestApp.DAL.Entities;
using TestApp.ModelClasses;
using TestApp.PLL.Interfaces;

namespace TestApp.Controllers
{
    [Authorize]

    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
      
        public CategoryController(ICategoryRepository categoryRepository , IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategories();

            var categoryModels = _mapper.Map<IEnumerable<CategoryModel>>(categories);

            return Ok(categoryModels);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<CategoryModel>>> GetcategoriesById(int id)
        {
            var category = await _categoryRepository.GetCategoriesById(id);
            if (category == null)
            {
                return BadRequest("Not Found");
            }
            var categoryModel = _mapper.Map<CategoryModel>(category);

            return Ok(categoryModel);
        }


        [HttpPost]
        public async Task<ActionResult<List<CategoryModel>>> AddNewCategory([FromBody] CategoryModel category)
        {

            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                return BadRequest("Category name is required.");
            }

            // Map the CategoryModel to the Categories entity
            var categoryEntity = _mapper.Map<Categories>(category);

            // Add category to the database
            await _categoryRepository.Add(categoryEntity);

            // Return a success response
            return CreatedAtAction(nameof(GetcategoriesById), new { id = categoryEntity.ID }, categoryEntity);
        }

        [HttpPut]
        public async Task<ActionResult<List<CategoryModel>>> UpdateCategory([FromBody] CategoryModel category, int id)
        {

            if (id != category.ID)
            {
                return BadRequest("ID mismatch");
            }

            var existingCategory = await _categoryRepository.GetById(id);
            if (existingCategory == null)
            {
                return NotFound("category not found");
            }

            // Update the existing product properties as necessary
            existingCategory.CategoryName = category.CategoryName;
           

            // Add other properties as needed

            await _categoryRepository.Update(existingCategory);
            return Ok();
        }


        [HttpDelete("{id}")]

        public async Task<ActionResult<List<CategoryModel>>> DeleteCategory(int id)
        {
            var result = await _categoryRepository.DeleteById(id);
            if (result == 0)
            {
                return NotFound("category not found");
            }

            return NoContent();
        }

    }
}
