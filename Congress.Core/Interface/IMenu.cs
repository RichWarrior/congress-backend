using Congress.Core.Entity;
using System.Collections.Generic;

namespace Congress.Core.Interface
{
    public interface IMenu
    {
        /// <summary>
        /// Kullanıcı Menülerini Getirir.
        /// </summary>
        /// <param name="loginType"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        List<Menu> GetUserMenu(int loginType, int userType);
    }
}
