using Pokemon_Review_System.Data;
using Pokemon_Review_System.Models;
using PokemonReviewApp.Interfaces;

namespace PokemonReviewApp.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly PokemonContext _context;

        public OwnerRepository(PokemonContext context)
        {
            _context = context;
        }
        public ICollection<Owner> GetAllOwners()
        {
            return _context.Owners.OrderBy(o => o.Id).ToList();
        }

        public Owner GetOwner(int ownerId)
        {
          return _context.Owners.FirstOrDefault(o=>o.Id==ownerId);
        }

        public ICollection<Owner> GetOwnerOfAPokemon(int pokeId)
        {
            return _context.PokemonOwners.Where(p => p.PokemonId == pokeId)
                .Select(o => o.Owner).ToList();
        }

        public ICollection<Pokemon> GetPokemonByOwner(int ownerId)
        {
            return _context.PokemonOwners.Where(p => p.OwnerId == ownerId).Select(p => p.Pokemon).ToList();
        }

        public bool IsOwnerExist(int ownerId)
        {
            return _context.Owners.Any(p => p.Id == ownerId);
        }

       public bool CreateOwner(Owner owner)
        {
            _context.Owners.Add(owner);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
