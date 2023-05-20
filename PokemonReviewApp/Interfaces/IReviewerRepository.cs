using Pokemon_Review_System.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface IReviewerRepository
    {
        ICollection<Reviewer> GetAllReviewer();
        Reviewer GetReviewer(int id);
        ICollection<Review> GetReviewsByReviewer(int reviewerId);
        bool IsReviewerExist(int reviewerId);
        bool CreateReviewer(Reviewer reviewer);
        bool Save();
    }
}
