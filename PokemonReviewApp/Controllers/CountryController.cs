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
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        //get all countries 
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(400)]
        public IActionResult GetAllCountries()
        {
            var countries = _mapper.Map<List<CountryDTO>>(
                _countryRepository.GetAllCountries()
                );

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(countries);
        }

        //get country by id
        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountry(int countryId)
        {
            if(!_countryRepository.IsCountryExist(countryId)) 
                return BadRequest(ModelState);

            var country = _mapper.Map<CountryDTO>(
                _countryRepository.GetCountry(countryId));

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(country);
        }
        //get country by owner
        //[HttpGet("/{ownerId}/owner")]
        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public IActionResult GetCountryByOwner(int ownerId)
        {
            var country = _mapper.Map<CountryDTO>(
                _countryRepository.GetCountryByOwner(ownerId));

            if (!ModelState.IsValid)
                return BadRequest();

            return Ok(country);
        }

        //get owners from country

        //[HttpGet("{countryId}")]
        //[ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        //[ProducesResponseType(400)]
        //public IActionResult GetOwnersFromCountry(int countryId)
        //{
        //    var owners = _mapper.Map<OwnerDTO>(
        //        _countryRepository.GetOwnersFromACountry(countryId)
        //        );

        //    if(!ModelState.IsValid)
        //        return BadRequest();

        //    return Ok(owners);
        //}

        //create country
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        
        public IActionResult CreateCountry([FromBody] CountryDTO countryToCreate)
        {
            if(countryToCreate == null)
                return BadRequest(ModelState);  

            var country = _countryRepository.GetAllCountries()
                .Where(c => c.Name.Trim().ToUpper() == countryToCreate.Name.Trim().ToUpper())
                .FirstOrDefault();

            if(country != null)
            {
                ModelState.AddModelError("", $"Country {countryToCreate.Name} already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryObj = _mapper.Map<Country>(countryToCreate);


            if (!_countryRepository.CreateCountry(countryObj))
            {
                ModelState.AddModelError("", $"Something went wrong saving the country " +
                                                              $"{countryObj.Name}");
                return StatusCode(500, ModelState);
            }
            return Ok("Country added Successfully");
        }

        //update country
        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int countryId, [FromBody] CountryDTO updatedCountry)
        {
            if (updatedCountry == null)
                return BadRequest(ModelState);

            if (countryId != updatedCountry.Id)
                return BadRequest(ModelState);

            if (!_countryRepository.IsCountryExist(countryId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var countryObj = _mapper.Map<Country>(updatedCountry);

            if (!_countryRepository.UpdateCountry(countryObj))
            {
                ModelState.AddModelError("", $"Something went wrong updating the country " +
                                                    $"{countryObj.Name}");
                return StatusCode(500, ModelState);
            }

            return Ok($"{countryObj.Name} updated successfully");
        }
        //delete country
        [HttpDelete("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_countryRepository.IsCountryExist(countryId))
            {
                return NotFound();
            }

            var countryToDelete = _countryRepository.GetCountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryRepository.DeleteCountry(countryToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting the country " +
                                      $"{countryToDelete.Name}");
            }

            return Ok($"{countryToDelete.Name} deleted successfully");
        }

    }
} 
