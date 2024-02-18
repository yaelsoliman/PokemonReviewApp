﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interface;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository,IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type=typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());
           if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(categories);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200,Type =typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId)
        {
            if(!_categoryRepository.CategoryExists(categoryId))
                return NotFound();
            var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(categoryId));
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(category);
        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonsByCategory(int categoryId)
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonsByCategory(categoryId));
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(284)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate)
        {
            if (categoryCreate == null)
                return BadRequest(ModelState);

            var category=_categoryRepository.GetCategories().Where(
             c=>c.Name.Trim().ToUpper() == categoryCreate.Name.TrimEnd().ToUpper())
              .FirstOrDefault();

            if (category != null)
            {
                 ModelState.AddModelError("", "Category already Exists");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(categoryCreate);

            if (!_categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "SomeThing went wrong while saving");
                return StatusCode(500, ModelState);
            }
            return Ok("Created Successfully");
              

        }
        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto categoryUpdate)
        {
            if (categoryUpdate == null)
                return BadRequest(ModelState);

            if(categoryId != categoryUpdate.Id)
                return BadRequest(ModelState);

            if (!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var categoryMap = _mapper.Map<Category>(categoryUpdate);
            if(!_categoryRepository.UpdateCategory(categoryMap))
                return BadRequest(ModelState);
            return NoContent();
        }
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if(!_categoryRepository.CategoryExists(categoryId))
                return NotFound();
             var categoryToDelete=_categoryRepository.GetCategory(categoryId);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong Deleting");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
