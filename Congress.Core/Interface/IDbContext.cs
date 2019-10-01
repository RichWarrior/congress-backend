using System.Collections.Generic;

namespace Congress.Core.Interface
{
    public interface IDbContext
    {
        /// <summary>
        /// Id Değerine Göre İlgili Değeri Döndürür.
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="id">Entity Id</param>
        /// <returns></returns>
        T GetById<T>(object id) where T : class, new();
        /// <summary>
        /// Sorguya Göre İlgili Entity Geri Döndürür.
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="query">Sql Sorgusu</param>
        /// <param name="parameters">Sorgu Parametreleri</param>
        /// <returns></returns>
        T GetByQuery<T>(string query, object parameters) where T : class, new();
        /// <summary>
        /// İlgili Entity Ait Tüm Verileri Getirir.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        ICollection<T> GetAll<T>() where T : class, new();
        /// <summary>
        /// İlgili Sorguya Göre Entitiy ICollection Tipinde Döndürür.
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="query">Sql Sorgusu</param>
        /// <param name="parameters">Sorgu Parametresi</param>
        /// <returns></returns>
        ICollection<T> GetByQueryAll<T>(string query, object parameters) where T : class, new();
        /// <summary>
        /// Boolean Türünde Sonuca İhtiyacın Varsa Kullanabilirsin.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        bool ExecuteQuery(string query, object parameters);
        /// <summary>
        /// Veritabanına İlgili Modeli Kaydetme İşini Yapar.
        /// /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="entity">Eklenecek Değer</param>
        /// <returns></returns>
        int Insert<T>(T entity) where T : class, new();
        /// <summary>
        /// Veritabanında İlgili Modeli Günceller.
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="entity">Model</param>
        /// <returns></returns>
        bool Update<T>(T entity) where T : class, new();
        /// <summary>
        /// Veritabanında İlgili Modeli Siler
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="entity">Model</param>
        /// <returns></returns>

        bool Delete<T>(T entity) where T : class, new();
        /// <summary>
        /// Veritabanında İlgili Veriyi Siler
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="id">Entity Id</param>
        /// <returns></returns>
        bool Delete<T>(object id) where T : class, new();
        /// <summary>
        /// Çoklu Insert İçin Kullanılır.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item"></param>
        /// <returns></returns>
        bool BulkInsert<T>(List<T> item) where T : class, new();

        bool BulkUpdate<T>(List<T> items) where T : class, new();
    }
}
