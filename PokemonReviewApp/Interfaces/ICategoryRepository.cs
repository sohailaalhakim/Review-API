using Pokemon_Review_System.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetAllCategories();
        Category GetCategory(int id);   
        ICollection<Pokemon> GetPokemonByCategory(int categoryId);
        bool IsCategoryExist(int id);  
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
        bool Save();
    }
}
