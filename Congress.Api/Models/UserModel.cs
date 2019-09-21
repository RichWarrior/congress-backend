using Congress.Core.Entity;
using System.Collections.Generic;

namespace Congress.Api.Models
{
    public class UserModel
    {
        public User user { get; set; }
        public List<User> users { get; set; }
        public List<Menu> menus{ get; set; }
        public string token { get; set; }
    }
}
