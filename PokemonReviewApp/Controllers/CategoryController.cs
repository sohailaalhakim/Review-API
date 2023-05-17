﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokemon_Review_System.Models;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;


        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        //get all categories
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(400)]

        public IActionResult GetAllCategories()
        {
            var categories = _mapper.Map<List<CategoryDTO>>(
                _categoryRepository.GetAllCategories());
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(categories);
        }

        //get category by id
        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type=typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId)
        {
            if(!_categoryRepository.IsCategoryExist(categoryId))
                return NotFound();

            var category = _mapper.Map<CategoryDTO>(
                _categoryRepository.GetCategory(categoryId));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(category);
        }

        //get pokemon by category
        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type=typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByCategory(int categoryId)
        {
            if(!_categoryRepository.IsCategoryExist(categoryId))
                return NotFound();

            var pokemons = _mapper.Map<List<PokemonDTO>>(
                _categoryRepository.GetPokemonByCategory(categoryId));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemons);
        }
    }
}