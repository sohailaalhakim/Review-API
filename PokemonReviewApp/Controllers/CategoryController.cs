using AutoMapper;
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

        //create category
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody]CategoryDTO categoryToCreate)
        {
            if(categoryToCreate == null)
                return BadRequest(ModelState);
            
            //check if category already exists
            var category = _categoryRepository.GetAllCategories()
                .Where(c => c.Name.Trim().ToUpper() == categoryToCreate.Name.Trim().ToUpper())
                .FirstOrDefault();

            if(category != null)
            {
                ModelState.AddModelError("", $"Category {categoryToCreate.Name} already exists");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryObj = _mapper.Map<Category>(categoryToCreate);

            if(!_categoryRepository.CreateCategory(categoryObj))
            {
                ModelState.AddModelError("", $"Something went wrong saving the category " +
                                                              $"{categoryObj.Name}");
                return StatusCode(500, ModelState);
            }
            return Ok($"{categoryObj.Name} added successfully");
            //return CreatedAtRoute("GetCategory", new { categoryId = categoryToCreate.Id }, categoryToCreate);
        }


        //update category
        [HttpPut("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, 
            [FromBody] CategoryDTO categoryToUpdate)
        {
            if(categoryToUpdate == null)
                return BadRequest(ModelState);
            if(categoryId != categoryToUpdate.Id)
                return BadRequest(ModelState);
            if(!_categoryRepository.IsCategoryExist(categoryId))
                return NotFound();  
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryObj = _mapper.Map<Category>(categoryToUpdate);

            if(!_categoryRepository.UpdateCategory(categoryObj))
            {
                ModelState.AddModelError("", $"Something went wrong updating the category " +
                                            $"{categoryObj.Name}");
                return StatusCode(500, ModelState);
            }

            return Ok($"{categoryObj.Name} updated successfully");

        }

        //delete category
        [HttpDelete("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if(!_categoryRepository.IsCategoryExist(categoryId))
                return NotFound();

            var categoryToDelete = _categoryRepository.GetCategory(categoryId);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);  

            if(!_categoryRepository.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting the category " +
                                         $"{categoryToDelete.Name}");
                return StatusCode(500, ModelState);
            }
            return Ok($"{categoryToDelete.Name} deleted successfully");
        }
    }

}
