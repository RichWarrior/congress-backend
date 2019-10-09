using Congress.Core.Entity;
using System.Collections.Generic;

namespace Congress.Core.Interface
{
    public interface IUserInterest
    {
        List<Category> GetAvailableCategories(int userId, int mainCategoryId);
        bool BulkInsertInterest(List<UserInterest> userInterests);
        List<Category> GetUserInterest(int userId);
        bool DeleteUserInterest(int userId, int interestId);
    }
}
