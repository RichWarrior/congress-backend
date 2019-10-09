using Congress.Api.Filters;
using Congress.Api.Models;
using Congress.Core.Entity;
using Congress.Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Congress.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {
        ICategory _SCategory;
        public CategoryController(IMethod _SMethod, ICategory _SCategory)
            : base(_SMethod)
        {
            this._SCategory = _SCategory;
        }

        /// <summary>
        /// Bütün Kategorileri Getirir.
        /// </summary>
        /// <returns></returns>
        [HttpPost("getcategories")]
        [DoctorValidation]
        public IActionResult GetCategories()
        {
            BaseResult<CategoryModel> baseResult = new BaseResult<CategoryModel>();
            baseResult.data.categories = _SCategory.GetCategories();
            return Json(baseResult);
        }
        /// <summary>
        /// Kategori Silme İşlemleri İçin Kullanılır
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost("deletecategory")]
        [DoctorValidation]
        public IActionResult DeleteCategory([FromBody]Category category)
        {
            BaseResult<CategoryModel> baseResult = new BaseResult<CategoryModel>();
            baseResult.data.category = category;
            bool isSuccess = false;
            baseResult.data.category.statusId = 1;
            if (category.parentCategoryId == 0)
            {
                baseResult.data.categories = _SCategory.GetSubCategories(category.id);
                foreach (var item in baseResult.data.categories)
                {
                    item.statusId = 1;
                }
            }
            if (_SCategory.CategoryUpdate(baseResult.data.category))
            {
                if (baseResult.data.categories != null)
                {
                    if (_SCategory.BulkUpdateCategory(baseResult.data.categories))
                    {
                        isSuccess = true;
                    }
                }
                else
                {
                    isSuccess = true;
                }
            }
            if (isSuccess)
            {
                return Json(baseResult);
            }
            else
            {
                baseResult.errMessage = "Silme İşleminiz Yapılamadı!";
                return new NotFoundObjectResult(baseResult);
            }
        }
        /// <summary>
        /// Ana Kategorileri Getirir.
        /// </summary>
        /// <returns></returns>
        [HttpPost("getmaincategory")]
        public IActionResult GetMainCategory()
        {
            BaseResult<CategoryModel> baseResult = new BaseResult<CategoryModel>();
            baseResult.data.categories = _SCategory.GetMainCategory();
            return Json(baseResult);
        }
        /// <summary>
        /// Yeni Kategori Oluşturmak İçin Kullanılır
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost("newcategory")]
        [DoctorValidation]
        public IActionResult NewCategory([FromBody]Category category)
        {
            BaseResult<CategoryModel> baseResult = new BaseResult<CategoryModel>();
            bool isSuccess = false;
            int userId = Convert.ToInt32(HttpContext.User.Identity.Name);
            category.creatorId = userId;
            isSuccess = _SCategory.Insert(category) > 0 ? true : false;
            if (isSuccess)
            {
                return Json(baseResult);
            }
            else
            {
                baseResult.errMessage = "Kategori Oluşturulamadı!";
                return new NotFoundObjectResult(baseResult);
            }
        }
        /// <summary>
        /// Kategori Güncellemek İçin Kullanılır.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost("editcategory")]
        [DoctorValidation]
        public IActionResult UpdateCategory([FromBody]Category category)
        {
            BaseResult<EventModel> baseResult = new BaseResult<EventModel>();
            bool isSuccess = false;
            isSuccess = _SCategory.CategoryUpdate(category);
            if (isSuccess)
            {
                return Json(baseResult);
            }
            else
            {
                baseResult.errMessage = "İşleminiz Tamamlanamadı!";
                return new NotFoundObjectResult(baseResult);
            }
        }        

        [HttpPost("getparentcategory")]
        public IActionResult GetParentCategory([FromBody]Category category)
        {
            BaseResult<CategoryModel> baseResult = new BaseResult<CategoryModel>();
            baseResult.data.categories = _SCategory.GetSubCategories(category.id);
            return Json(baseResult);
        }        
    }
}