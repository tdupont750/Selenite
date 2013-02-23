using System.Collections.Generic;
using Selenite.Models;

namespace Selenite.Services
{
    public interface ICategoryService
    {
        IList<string> GetCategoryNames();
        Category GetCategory(string categoryName);
        void SaveCategory(Category category);
        IList<Category> GetCategories(Manifest manifest);
    }
}