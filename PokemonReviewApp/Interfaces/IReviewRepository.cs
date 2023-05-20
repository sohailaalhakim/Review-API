using Pokemon_Review_System.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review> GetAllReviews();
        Review GetReview(int reviewId);
        ICollection<Review> GetReviewsOfPokemon(int pokeId);
        bool IsReviewExist(int reviewId);
        bool CreateReview(Review review);
        bool Save();
    }
}
