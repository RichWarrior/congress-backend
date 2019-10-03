using Congress.Core.Entity;
using System.Collections.Generic;

namespace Congress.Core.Interface
{
    public interface ICategory
    {
        /// <summary>
        /// Bütün Kategorileri Listeler
        /// </summary>
        /// <returns></returns>
        List<Category> GetCategories();
        /// <summary>
        /// Kategori Günceller
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        bool CategoryUpdate(Category category);
        /// <summary>
        /// Alt Kategorileri Getirir
        /// </summary>
        /// <param name="mainCategoryId"></param>
        /// <returns></returns>
        List<Category> GetSubCategories(int mainCategoryId);
        /// <summary>
        /// Çoklu Kategori Update Eder
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        bool BulkUpdateCategory(List<Category> category);
        /// <summary>
        /// Ana Kategorileri Getirir
        /// </summary>
        /// <returns></returns>
        List<Category> GetMainCategory();
        /// <summary>
        /// Kategori Ekler
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        int Insert(Category category);
        /// <summary>
        /// Etkinliklere Tanımlanabilecek Alt Kategorileri Getirir.
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="mainMenuId"></param>
        /// <returns></returns>
        List<Category> GetEventAvailableCategories(int eventId, int mainMenuId);
    }
}
