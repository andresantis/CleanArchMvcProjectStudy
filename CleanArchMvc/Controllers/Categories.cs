using CleanArchMvc.Application.DTOs;
using CleanArchMvc.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchMvc.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class Categories : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public Categories(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> Get()
        {
            var categories = await _categoryService.GetCategories();
            if (categories == null)
            {
                return NotFound("Categories not found");
            }
            return Ok(categories);
        }

        [HttpGet("{id}", Name = "Getcategory")]
        public async Task<ActionResult<CategoryDTO>> Get(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound("Categories not found");
            }
            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult> Create(CategoryDTO category)
        {
            if (category == null)
            {
                return BadRequest("Invalid Data");
            }
            await _categoryService.Add(category);
            return new CreatedAtRouteResult("GetCategory", new {id = category.Id});
        }

        [HttpPut]
        public async Task<ActionResult<CategoryDTO>> Put([FromBody]CategoryDTO category)
        {
            if (category == null)
            {
                return BadRequest("Invalid Data");
            }
            await _categoryService.Update(category);
            return Ok(category);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoryDTO>> Delete(int id)
        {
            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound("Category not found");
            }

            await _categoryService.Remove(id);

            return Ok(category);

        }
    }
}
