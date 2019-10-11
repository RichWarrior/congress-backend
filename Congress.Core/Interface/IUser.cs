using Congress.Core.Entity;
using System.Collections.Generic;

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
        User CheckUser(string email);
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
        /// <summary>
        /// Firmaları Getirir.
        /// </summary>
        /// <returns></returns>
        List<User> GetBusiness();
        /// <summary>
        /// Kullanıcı Güncellemek İçin Kullanılır.
        /// </summary>
        /// <param name="user">Güncellenecek Kullanıcı Modeli</param>
        /// <returns></returns>
        bool UpdateUser(User user);
        /// <summary>
        /// Katılımcıları Getirir.
        /// </summary>
        /// <returns></returns>
        List<User> GetParticipant();
        /// <summary>
        /// Id Değerine Göre Kullanıcıyı Getirir.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User GetById(int id);
        /// <summary>
        /// Aktif Olan Tüm Kullanıcıları Getirir.
        /// </summary>
        /// <returns></returns>
        List<User> GetAllUser();
        /// <summary>
        /// Çoklu Kullanıcı Oluşturma
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        bool BulkUserInsert(List<User> users);
        /// <summary>
        /// Kullanıcının Katıldığı Etkinlikleri Getirir.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<Event> GetUserParticipantEvents(int userId);
        /// <summary>
        /// İstenilen E-Postaya Ait Kullanıcıyı Getirir.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        User GetByEmail(string email);
    }
}
