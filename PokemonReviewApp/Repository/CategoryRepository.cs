using Pokemon_Review_System.Data;
using Pokemon_Review_System.Models;
using PokemonReviewApp.Interfaces;

namespace PokemonReviewApp.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly PokemonContext _context;

        public CategoryRepository(PokemonContext context) 
        {
            _context = context;
        }
        public ICollection<Category> GetAllCategories()
        {
            return _context.Categories.OrderBy(c => c.Id).ToList();
        }
        public Category GetCategory(int id)
        {
            return _context.Categories.Where(c => c.Id == id).FirstOrDefault();
        }
        public ICollection<Pokemon> GetPokemonByCategory(int categoryId)
        {
           return _context.PokemonCategories.Where(c=>c.CategoryId == categoryId).Select(p=>p.Pokemon).ToList();
        }
        public bool IsCategoryExist(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public bool CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
