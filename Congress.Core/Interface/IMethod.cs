using Congress.Core.Entity;
using System;
using System.Collections.Generic;

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
        /// <summary>
        /// E-Posta Onaylama
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        bool ValidateEmail(string email);

        T SystemParameterToObject<T>(List<SystemParameter> systemParameters);
    }
}
