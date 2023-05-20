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

        public bool CreateCountry(Country country)
        {
            _context.Countries.Add(country);
            return Save();
        }

        public ICollection<Country> GetAllCountries()
        {
            return _context.Countries.OrderBy(c => c.Id).ToList();
        }

        public Country GetCountry(int id)
        {
            return _context.Countries.Where(c => c.Id == id).FirstOrDefault();
        }

        public Country GetCountryByOwner(int ownerId)
        {
            return _context.Owners.Where(o => o.Id == ownerId).Select(c => c.Country).FirstOrDefault();
        }
        public ICollection<Owner> GetOwnersFromACountry(int countryId)
        {
            return _context.Owners.Where(c=>c.Country.Id == countryId).ToList();
        }

        public bool IsCountryExist(int id)
        {
            return _context.Countries.Any(c => c.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
