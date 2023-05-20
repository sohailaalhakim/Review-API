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
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;

        private readonly IMapper _mapper;


        public OwnerController(IOwnerRepository ownerRepository,ICountryRepository countryRepository, IMapper mapper)
        {
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        //get all owners
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]

        public IActionResult GetAllOwners()
        {
            var owners = _mapper.Map<List<OwnerDTO>>(
                _ownerRepository.GetAllOwners());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(owners);
        }

        //get category by id
        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetOwner(int ownerId)
        {
            if (!_ownerRepository.IsOwnerExist(ownerId))
                return NotFound();

            var owner = _mapper.Map<OwnerDTO>(
                _ownerRepository.GetOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owner);
        }

        // get pokemon by its owner
        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemonByOwner (int ownerId)
        {
            if (!_ownerRepository.IsOwnerExist(ownerId))
                return NotFound();

            var owner = _mapper.Map<List<PokemonDTO>>(
                  _ownerRepository.GetPokemonByOwner(ownerId)
                );

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owner);
        }

        //create owner
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromQuery] int countryId , [FromBody] OwnerDTO ownerToCreate)
        {
            if (ownerToCreate == null)
                return BadRequest(ModelState);

            var country = _ownerRepository.GetAllOwners()
                .Where(c => c.LastName.Trim().ToUpper() == ownerToCreate.LastName.Trim().ToUpper())
                .FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", $"owner {ownerToCreate.FirstName} {ownerToCreate.LastName} already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerObj = _mapper.Map<Owner>(ownerToCreate);
            //owner has country so we need to assign the owner to a country
            ownerObj.Country = _countryRepository.GetCountry(countryId);

            if (!_ownerRepository.CreateOwner(ownerObj))
            {
                ModelState.AddModelError("", $"Something went wrong saving the owner " +
                                              $"{ownerObj.FirstName} {ownerObj.LastName}");
                return StatusCode(500, ModelState);
            }
            return Ok($"{ownerObj.FirstName} {ownerObj.LastName} added successfully");
        }
        //update owner
        [HttpPut("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDTO ownerToUpdate)
        {
            if (ownerToUpdate == null)
                return BadRequest(ModelState);

            if (ownerId != ownerToUpdate.Id)
                return BadRequest(ModelState);

            if (!_ownerRepository.IsOwnerExist(ownerId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var ownerObj = _mapper.Map<Owner>(ownerToUpdate);

            if (!_ownerRepository.UpdateOwner(ownerObj))
            {
                ModelState.AddModelError("", $"Something went wrong updating the owner " +
                                                    $"{ownerObj.FirstName} {ownerObj.LastName}");
                return StatusCode(500, ModelState);
            }
            return Ok($"{ownerObj.FirstName} {ownerObj.LastName} updated successfully");
        }

        //delete owner
        [HttpDelete("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOwner(int ownerId)
        {
            if (!_ownerRepository.IsOwnerExist(ownerId))
            {
                return NotFound();
            }

            var ownerToDelete = _ownerRepository.GetOwner(ownerId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_ownerRepository.DeleteOwner(ownerToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting the owner " +
                                      $"{ownerToDelete.FirstName} + {ownerToDelete.LastName}");
            }
            return Ok($"{ownerToDelete.FirstName}{ownerToDelete.LastName} deleted successfully");
        }


    }
}
