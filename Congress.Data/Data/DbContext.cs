using Congress.Core.Interface;
using Dapper;
using Dapper.Contrib.Extensions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Congress.Data.Data
{
    public class DbContext : IDbContext
    {
        Connection connection = new Connection();       

        public bool Delete<T>(T entity) where T : class, new()
        {
            bool result = true;
            try
            {
                using (MySqlConnection con = new MySqlConnection(connection.mysqlCongress))
                {
                    con.Open();
                    con.Delete(entity);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                result = false;
            }
            return result;
        }

        public bool Delete<T>(object id) where T : class, new()
        {
            T entity = GetById<T>(id);
            return Delete(entity);
        }

        public bool ExecuteQuery(string query, object parameters)
        {
            bool result = true;
            try
            {
                using (MySqlConnection con = new MySqlConnection(connection.mysqlCongress))
                {
                    con.Query(query, parameters);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                result = false;
            }
            return result;
        }

        public ICollection<T> GetAll<T>() where T : class, new()
        {
            try
            {
                using (var con = new MySqlConnection(connection.mysqlCongress))
                {
                    con.Open();

                    return con.GetAll<T>().ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new List<T>();
        }

        public T GetById<T>(object id) where T : class, new()
        {
            try
            {
                using (var con = new MySqlConnection(connection.mysqlCongress))
                {
                    con.Open();

                    return con.Get<T>(id);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return (T)Activator.CreateInstance(typeof(T));
        }

        public T GetByQuery<T>(string query, object parameters) where T : class, new()
        {            
            try
            {
                using (var con = new MySqlConnection(connection.mysqlCongress))
                {
                    con.Open();
                    return con.Query<T>(query, parameters).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return (T)Activator.CreateInstance(typeof(T));
            }
        }

        public ICollection<T> GetByQueryAll<T>(string query, object parameters) where T : class, new()
        {
            try
            {
                using (var con = new MySqlConnection(connection.mysqlCongress))
                {
                    con.Open();
                    return con.Query<T>(query, parameters).ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return new List<T>();
            }
        }

        public int Insert<T>(T entity) where T : class, new()
        {
            try
            {
                using (var con = new MySqlConnection(connection.mysqlCongress))
                {
                    con.Open();

                    return (int)con.Insert(entity);

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return 0;
        }

        public bool Update<T>(T entity) where T : class, new()
        {
            try
            {
                using (var con = new MySqlConnection(connection.mysqlCongress))
                {
                    con.Open();

                    return con.Update(entity);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return false;
        }
        public bool BulkInsert<T>(List<T> item) where T : class, new()
        {
            bool rtn = false;
            try
            {
                using (var con = new MySqlConnection(connection.mysqlCongress))
                {
                    con.Open();
                    foreach (var entity in item)
                    {
                        con.Insert(entity);
                    }
                    rtn = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return rtn;
        }
    }
}
