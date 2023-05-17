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
        ICollection<Owner> IOwnerRepository.GetAllOwners()
        {
            return _context.Owners.OrderBy(o => o.Id).ToList();
        }

        Owner IOwnerRepository.GetOwner(int ownerId)
        {
          return _context.Owners.FirstOrDefault(o=>o.Id==ownerId);
        }

        ICollection<Owner> IOwnerRepository.GetOwnerOfAPokemon(int pokeId)
        {
            return _context.PokemonOwners.Where(p => p.PokemonId == pokeId)
                .Select(o => o.Owner).ToList();
        }

        ICollection<Pokemon> IOwnerRepository.GetPokemonByOwner(int ownerId)
        {
            return _context.PokemonOwners.Where(p => p.OwnerId == ownerId).Select(p => p.Pokemon).ToList();
        }

        bool IOwnerRepository.IsOwnerExist(int ownerId)
        {
            return _context.Owners.Any(p => p.Id == ownerId);
        }
    }
}
