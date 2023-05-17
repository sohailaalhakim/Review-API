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


    }
} 
