using Congress.Core.Entity;
using System.Collections.Generic;

namespace Congress.Core.Interface
{
    public interface ICategory
    {
        List<Category> GetCategories();
        bool CategoryUpdate(Category category);
        List<Category> GetSubCategories(int mainCategoryId);
        bool BulkUpdateCategory(List<Category> category);
        List<Category> GetMainCategory();
        int Insert(Category category);
    }
}
