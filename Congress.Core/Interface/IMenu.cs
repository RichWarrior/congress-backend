using Congress.Core.Entity;
using System.Collections.Generic;

namespace Congress.Core.Interface
{
    public interface IMenu
    {
        List<Menu> GetUserMenu(int loginType, int userType);
    }
}
