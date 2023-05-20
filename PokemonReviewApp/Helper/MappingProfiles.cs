using AutoMapper;
using Pokemon_Review_System.Models;
using PokemonReviewApp.DTO;

namespace PokemonReviewApp.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Pokemon, PokemonDTO>();
            CreateMap<PokemonDTO, Pokemon>();

            CreateMap<Category, CategoryDTO>();
            CreateMap<CategoryDTO, Category>();

            CreateMap<Country, CountryDTO>();
            CreateMap<CountryDTO, Country>();

            CreateMap<Owner, OwnerDTO>();
            CreateMap<OwnerDTO, Owner>();

            CreateMap<Review, ReviewDTO>();
            CreateMap<ReviewDTO, Review>();

            CreateMap<Reviewer, ReviewerDTO>();
            CreateMap<ReviewerDTO, Reviewer>();
        }
    }
}
