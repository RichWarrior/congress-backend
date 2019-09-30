using Congress.Core.Interface;
using Congress.Core.Tags;
using System;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Congress.Data.Service
{
    public class SMethod : IMethod
    {
        public string StringToMd5(string password)
        {
            string rtn = "";
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] byteArr = Encoding.UTF8.GetBytes(password);
            byteArr = provider.ComputeHash(byteArr);
            foreach (byte _byte in byteArr)
            {
                rtn += _byte.ToString("x2").ToLower();
            }
            return rtn;
        }

        public string GetEnumValue(Enum _enum)
        {
            string _rtn = "";
            Type enumType = _enum.GetType();
            MemberInfo[] memberInfos = enumType.GetMember(_enum.ToString());
            MemberInfo enumValueMemberInfo = memberInfos.FirstOrDefault(m => m.DeclaringType == enumType);
            object[] valueAttributes =
                  enumValueMemberInfo.GetCustomAttributes(typeof(EnumValue), false);
            _rtn = ((EnumValue)valueAttributes[0]).value;
            return _rtn;
        }

        public bool ValidateEmail(string email)
        {
            bool rtn = false;
            try
            {
                var addr = new MailAddress(email);
                rtn = addr.Address == email;
            }
            catch (Exception)
            {
            }
            return rtn;
        }
    }
}
