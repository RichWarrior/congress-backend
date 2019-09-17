using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Congress.Core.Entity
{
    [Table("user")]
    public class User : BaseEntity
    {
        /// <summary>
        /// Kullanıcı Tipi
        /// Doktor = 1
        /// Firma = 2
        /// Kullanıcı = 3
        /// </summary>
        public int userTypeId { get; set; }
        /// <summary>
        /// Kullanıcılara E-Posta Gönderilirken Id Yerine Guid Gönderilecek
        /// </summary>
        public string userGuid { get; set; }
        /// <summary>
        /// Firma veya Kullanıcı Ad Bilgisi
        /// 50 Karakter Uzunuluğunda
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Soyad Bilgisi
        /// 50 Karakter Uzunluğunda
        /// </summary>
        public string surname { get; set; }
        /// <summary>
        /// E-Posta Adresi
        /// 100 Karakter Uzunluğunda
        /// </summary>
        public string email{ get; set; }
        /// <summary>
        /// Şifre 20 Karakter Uzunluğunda Olacak.
        /// </summary>
        public string password{ get; set; }
        /// <summary>
        /// Pasaport veya T.C kimlik Numarası 
        /// 11 Karakter Uzunluğunda
        /// </summary>
        public string identityNr{ get; set; }
        /// <summary>
        /// Cinsiyet
        /// Erkek = 1
        /// Kadın = 2
        /// </summary>
        public int gender{ get; set; }
        /// <summary>
        /// Firma Kuruluş Tarihi veya Kullanıcı Doğum Tarihi
        /// </summary>
        public DateTime birthDate{ get; set; }
        /// <summary>
        /// Meslek Id
        /// </summary>
        public int jobId{ get; set; }
        /// <summary>
        /// Ülke Id
        /// </summary>
        public int countryId{ get; set; }
        /// <summary>
        /// İl Id
        /// </summary>
        public int cityId{ get; set; }
        /// <summary>
        /// Profil Resmi Minio Linki
        /// </summary>
        public string avatarPath{ get; set; }
        /// <summary>
        /// Telefon Numarası
        /// </summary>
        public string phoneNr { get; set; }
        /// <summary>
        /// Vergi Numarası
        /// </summary>
        public string taxNr { get; set; }
        /// <summary>
        /// Açabileceği Event Sayısı
        /// </summary>
        public int eventCount { get; set; }
        /// <summary>
        /// E-Posta Aktiflik Durumu
        /// 1 Aktif Değil
        /// 2 Aktif
        /// </summary>
        public int emailVerification { get; set; }
        /// <summary>
        /// Profil Aktiflik Durumu
        /// 1 Aktif Değil
        /// 2 Aktif
        /// </summary>
        public int profileStatus { get; set; }
        /// <summary>
        /// Bildirim Durumu
        /// 1 Aktif Değil
        /// 2 Aktif
        /// </summary>
        public int notificationStatus { get; set; }  
        
        [Write(false)]
        public List<IFormFile> avatarFile { get; set; }
    }
}
