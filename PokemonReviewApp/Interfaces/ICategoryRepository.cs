﻿using Pokemon_Review_System.Models;

namespace PokemonReviewApp.Interfaces
{
    public interface ICategoryRepository
    {
        ICollection<Category> GetAllCategories();
        Category GetCategory(int id);   
        ICollection<Pokemon> GetPokemonByCategory(int categoryId);
        bool IsCategoryExist(int id);  
    }
}
