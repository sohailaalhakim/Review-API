using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokemon_Review_System.Models;
using PokemonReviewApp.DTO;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;


        public PokemonController(IPokemonRepository pokemonRepository,
            IReviewRepository reviewRepository,

            IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _reviewRepository = reviewRepository;
            _mapper = mapper;
        }
        //get all pokemons
        [HttpGet]
        [ProducesResponseType(200, Type=typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]

        public IActionResult GetPokemons()
        {
            var pokemons = _mapper.Map<List<PokemonDTO>>(_pokemonRepository.GetAllPokemon());
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(pokemons);
        }
        //get pokemon by id
        [HttpGet("{pokemonId}")]
        [ProducesResponseType(200, Type=typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon (int pokemonId)
        {
            if(!_pokemonRepository.IsPokemonExist(pokemonId))
                return NotFound();

            var pokemon = _mapper.Map<PokemonDTO>(_pokemonRepository.GetPokemonById(pokemonId));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(pokemon);
        }
        // get pokemon rating
        [HttpGet("{pokemonId}/rating")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonRating(int pokemonId)
        {
            if (!_pokemonRepository.IsPokemonExist(pokemonId))
                return NotFound();

            var rating = _pokemonRepository.GetPokemonRating(pokemonId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok($" Rating of this pokemon {rating}");
        }

        //create pokemon
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int catId, [FromBody] PokemonDTO pokemonToCreate)
        {
            if (pokemonToCreate == null)
                return BadRequest(ModelState);

            var pokemon = _pokemonRepository.GetAllPokemon()
                .Where(c => c.Name.Trim().ToUpper() == pokemonToCreate.Name.Trim().ToUpper())
                .FirstOrDefault();

            if (pokemon != null)
            {
                ModelState.AddModelError("", $"Pokemon {pokemonToCreate.Name} already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var pokemonObj = _mapper.Map<Pokemon>(pokemonToCreate);

            if (!_pokemonRepository.CreatePokemon(ownerId, catId, pokemonObj))
            {
                ModelState.AddModelError("", $"Something went wrong saving the pokemon " +
                                                              $"{pokemonObj.Name}");
                return StatusCode(500, ModelState);
            }
            return Ok($"{pokemonObj.Name} added successfully");
        }
        //update pokemon

        [HttpPut("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon(int pokeId,
            [FromQuery] int ownerId, [FromQuery] int catId,
            [FromBody] PokemonDTO pokemonToUpdate)
        {
            if (pokemonToUpdate == null)
                return BadRequest(ModelState);

            if (pokeId != pokemonToUpdate.Id)
                return BadRequest(ModelState);

            if (!_pokemonRepository.IsPokemonExist(pokeId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var pokemonObj = _mapper.Map<Pokemon>(pokemonToUpdate);

            if (!_pokemonRepository.UpdatePokemon(pokeId,catId,pokemonObj))
            {
                ModelState.AddModelError("", $"Something went wrong updating the pokemon " +
                                                    $"{pokemonObj.Name}");
                return StatusCode(500, ModelState);
            }

            return Ok($"{pokemonObj.Name} updated successfully");
        }
        //delete pokemon
        [HttpDelete("{pokeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokemon(int pokeId)
        {
            if (!_pokemonRepository.IsPokemonExist(pokeId))
            {
                return NotFound();
            }

            var reviewsToDelete = _reviewRepository.GetReviewsOfPokemon(pokeId);
            var pokemonToDelete = _pokemonRepository.GetPokemonById(pokeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            if (!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
            {
                ModelState.AddModelError("", $"Something went wrong deleting these reviews " +
                                      $"{reviewsToDelete.ToList()}");
            }

            if (!_pokemonRepository.DeletePokemon(pokemonToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting the pokemon " +
                                      $"{pokemonToDelete.Name}");
            }
            return Ok($"{pokemonToDelete.Name} deleted successfully");

        }

    }
}
