using Pokemon_Review_System.Data;
using Pokemon_Review_System.Models;
using PokemonReviewApp.Interfaces;

namespace PokemonReviewApp.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly PokemonContext _context;
        public ReviewRepository(PokemonContext context)
        {
            _context = context;
        }

        ICollection<Review> IReviewRepository.GetAllReviews()
        {
            return _context.Reviews.OrderBy(review => review.Id).ToList();
        }

        Review IReviewRepository.GetReview(int reviewId)
        {
            return _context.Reviews.SingleOrDefault(r=>r.Id== reviewId);
        }

        ICollection<Review> IReviewRepository.GetReviewsOfPokemon(int pokeId)
        {
            return _context.Reviews.Where(r => r.Pokemon.Id == pokeId).ToList();
        }

        bool IReviewRepository.IsReviewExist(int reviewId)
        {
            return _context.Reviews.Any(r => r.Id==reviewId);
        }
    }
}
