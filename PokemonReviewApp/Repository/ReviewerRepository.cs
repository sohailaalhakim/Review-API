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

        public bool CreateReviewer(Reviewer reviewer)
        {
            _context.Reviewers.Add(reviewer);
            return Save();
        }

        public ICollection<Reviewer> GetAllReviewer()
        {
            return _context.Reviewers.OrderBy(p => p.Id).ToList();
        }

        public Reviewer GetReviewer(int id)
        {
            return _context.Reviewers.FirstOrDefault(p => p.Id == id);
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerId)
        {
            return _context.Reviews.Where(r=>r.Reviewer.Id== reviewerId).ToList();
        }
        public bool UpdateReviewer(Reviewer reviewer)
        {
            _context.Reviewers.Update(reviewer);
            return Save();
        }
        public bool DeleteReviewer(Reviewer reviewer)
        {
            _context.Reviewers.Remove(reviewer);
            return Save();
        }

        public bool IsReviewerExist(int reviewerId)
        {
            return _context.Reviewers.Any(p => p.Id == reviewerId);

        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved >= 0 ? true : false;
        }
    }
}
