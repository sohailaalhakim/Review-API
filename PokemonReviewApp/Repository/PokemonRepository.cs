using Pokemon_Review_System.Data;
using Pokemon_Review_System.Models;
using PokemonReviewApp.Interfaces;
using System.Diagnostics.Metrics;

namespace PokemonReviewApp.Repository
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly PokemonContext _context;
        public PokemonRepository(PokemonContext context) 
        {
         _context = context;
        }

        public ICollection<Pokemon> GetAllPokemon()
        {
            return _context.Pokemon.OrderBy(p=>p.Id).ToList();
        }

        //public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        //{
        //    var pokemonOwnerEntity = _context.Owners.Where(u => u.Id == ownerId).FirstOrDefault();
        //    var pokemonCategoryEntity = _context.Categories.Where(c => c.Id == categoryId).FirstOrDefault();

        //    var pokemonOwner = new PokemonOwner()
        //    {
        //        Owner = pokemonOwnerEntity,
        //        Pokemon = pokemon,
        //    };
        //    _context.Add(pokemonOwner);

        //    var pokemonCategory = new PokemonCategory()
        //    {
        //        Category = pokemonCategoryEntity,
        //        Pokemon = pokemon,
        //    };  
        //    _context.Add(pokemonCategory);

        //    _context.Add(pokemon);

        //    return Save();

        //}


        public bool CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            var pokemonOwnerEntity = _context.Owners.Where(a => a.Id == ownerId).FirstOrDefault();
            var category = _context.Categories.Where(a => a.Id == categoryId).FirstOrDefault();

            var pokemonOwner = new PokemonOwner()
            {
                Owner = pokemonOwnerEntity,
                Pokemon = pokemon,
            };

            _context.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory()
            {
                Category = category,
                Pokemon = pokemon,
            };

            _context.Add(pokemonCategory);

            _context.Add(pokemon);

            return Save();
        }
        public Pokemon GetPokemonById(int pokemonId)
        {
           return _context.Pokemon.Where(p => p.Id == pokemonId).FirstOrDefault();
        }

        public Pokemon GetPokemonByName(string pokemonName)
        {
           return _context.Pokemon.Where(p => p.Name == pokemonName).FirstOrDefault();
        }

        public decimal GetPokemonRating(int pokemonId)
        {
            var reviews = _context.Reviews.Where(r => r.Pokemon.Id == pokemonId);
            if(reviews.Count() <= 0)
                return 0;
            //return the average of all the ratings
            return ((decimal)reviews.Sum(r => r.Rating) / reviews.Count());
              
        }
        public bool UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon)
        {
            _context.Update(pokemon);
            return Save();
        }
        public bool DeletePokemon(Pokemon pokemon)
        {
            _context.Pokemon.Remove(pokemon);
            return Save();
        }
        public bool IsPokemonExist(int pokemonId)
        {
            return _context.Pokemon.Any(p => p.Id == pokemonId);
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
