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

       public bool CreateReview(Review review)
        {
            _context.Reviews.Add(review);
            return Save();
        }

        public ICollection<Review> GetAllReviews()
        {
            return _context.Reviews.OrderBy(review => review.Id).ToList();
        }

        public Review GetReview(int reviewId)
        {
            return _context.Reviews.SingleOrDefault(r=>r.Id== reviewId);
        }

        public ICollection<Review> GetReviewsOfPokemon(int pokeId)
        {
            return _context.Reviews.Where(r => r.Pokemon.Id == pokeId).ToList();
        }

        public bool IsReviewExist(int reviewId)
        {
            return _context.Reviews.Any(r => r.Id==reviewId);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved >= 0 ? true : false;
        }
    }
}
