using System;

namespace Congress.Core.Interface
{
    public interface IMethod
    {
        /// <summary>
        /// Şifreyi MD5 Formatına Dönüştürür.
        /// </summary>
        /// <param name="password">Şifre</param>
        /// <returns></returns>
        string StringToMd5(string password);
        /// <summary>
        /// Enum Değerinin EnumValue Tag Değerini Döndürür.
        /// </summary>
        /// <param name="_enum">Enum Değeri</param>
        /// <returns></returns>
        string GetEnumValue(Enum _enum);
        bool ValidateEmail(string email);
    }
}
