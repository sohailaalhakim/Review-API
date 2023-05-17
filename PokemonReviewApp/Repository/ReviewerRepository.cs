using Pokemon_Review_System.Data;
using Pokemon_Review_System.Models;
using PokemonReviewApp.Interfaces;

namespace PokemonReviewApp.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly PokemonContext _context;

        public ReviewerRepository(PokemonContext context)
        {
            _context = context;
        }

        ICollection<Reviewer> IReviewerRepository.GetAllReviewer()
        {
            return _context.Reviewers.OrderBy(p => p.Id).ToList();
        }

        Reviewer IReviewerRepository.GetReviewer(int id)
        {
            return _context.Reviewers.FirstOrDefault(p => p.Id == id);
        }

        ICollection<Review> IReviewerRepository.GetReviewsByReviewer(int reviewerId)
        {
            return _context.Reviews.Where(r=>r.Reviewer.Id== reviewerId).ToList();
        }

        bool IReviewerRepository.IsReviewerExist(int reviewerId)
        {
            return _context.Reviewers.Any(p => p.Id == reviewerId);

        }
    }
}
