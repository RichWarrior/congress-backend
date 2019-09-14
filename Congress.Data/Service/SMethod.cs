using Congress.Core.Interface;
using Congress.Core.Tags;
using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Congress.Data.Service
{
    public class SMethod : IMethod
    {
        public string StringToMd5(string password)
        {
            StringBuilder sb = new StringBuilder();
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] byteArr = Encoding.UTF8.GetBytes(password);
            foreach (byte _byte in byteArr)
            {
                sb.Append(_byte.ToString("x2").ToLower());
            }
            return sb.ToString();
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
    }
}
