using Pokemon_Review_System.Models;
using System.Collections;

namespace PokemonReviewApp.Interfaces
{
    public interface IOwnerRepository
    {
        ICollection<Owner> GetAllOwners();
        Owner GetOwner(int ownerId);
        ICollection<Owner> GetOwnerOfAPokemon(int pokeId);
        ICollection<Pokemon> GetPokemonByOwner(int ownerId);
        bool IsOwnerExist(int ownerId);
        bool CreateOwner(Owner owner);
        bool Save();
    }
}
