using Congress.Core.Entity;

namespace Congress.Core.Interface
{
    public interface IUser
    {
        /// <summary>
        /// Kullanıcı Kayıt Olurken E-Posta ve Kimlik Kontrolü Yapar.
        /// </summary>
        /// <param name="email">E-Posta Adresi</param>
        /// <param name="identityNr">Pasaport veya T.C No</param>
        /// <returns></returns>
        User CheckUser(string email,string identityNr);
        /// <summary>
        /// Kullanıcı Eklemek İçin Kullanılır.
        /// </summary>
        /// <param name="user">Kullanıcı Modeli</param>
        /// <returns></returns>
        int InsertUser(User user);
        /// <summary>
        /// Kullanıcı Giriş İşlemi Yapmak İçin Kullanılır.
        /// </summary>
        /// <param name="email">E-Posta Adresi</param>
        /// <param name="password">Şifre</param>
        /// <returns></returns>
        User Login(string email, string password);
    }
}
