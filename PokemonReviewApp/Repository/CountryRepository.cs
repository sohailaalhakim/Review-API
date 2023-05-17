using AutoMapper;
using Pokemon_Review_System.Data;
using Pokemon_Review_System.Models;
using PokemonReviewApp.Interfaces;

namespace PokemonReviewApp.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly PokemonContext _context;
        public CountryRepository(PokemonContext context)
        {
            _context = context;
        }


        ICollection<Country> ICountryRepository.GetAllCountries()
        {
            return _context.Countries.OrderBy(c => c.Id).ToList();
        }

        Country ICountryRepository.GetCountry(int id)
        {
            return _context.Countries.Where(c => c.Id == id).FirstOrDefault();
        }

        Country ICountryRepository.GetCountryByOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefault();
        }
        ICollection<Owner> ICountryRepository.GetOwnersFromACountry(int countryId)
        {
            return _context.Owners.Where(c=>c.Country.Id == countryId).ToList();
        }

        bool ICountryRepository.IsCountryExist(int id)
        {
            return _context.Countries.Any(c => c.Id == id);
        }
    }
}
