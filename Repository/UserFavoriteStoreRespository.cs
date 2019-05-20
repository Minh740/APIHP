using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class UserFavoriteStoreRespository : IRepository<UserFavoriteStore>
    {
        private string connectionString;
        private IConfiguration config;
        public UserFavoriteStoreRespository(IConfiguration configuration)
        {
            config = configuration;

        }
        public int? Add(UserFavoriteStore item)
        {
            int? userFavoriteStoreId = null;
         
                Base.Connection.Open();
                userFavoriteStoreId = Base.Connection.Insert<UserFavoriteStore>(item);
            return userFavoriteStoreId;
        }
        public IEnumerable<UserFavoriteStore> FindAll()
        {

                   Base.Connection.Open();
                return Base.Connection.GetList<UserFavoriteStore>(new { status = 1 });
            
        }

        public UserFavoriteStore FindByID(int id)
        {

                  Base.Connection.Open();
                return Base.Connection.Get<UserFavoriteStore>(id);
            
        }

        public void Remove(int id)
        {

                Base.Connection.Open();
                UserFavoriteStore userFavoriteStore = Base.Connection.Get<UserFavoriteStore>(id);
                userFavoriteStore.Status = -1;
                 Base.Connection.Update<UserFavoriteStore>(userFavoriteStore);
        }

        public void Update(UserFavoriteStore item)
        {
            Base.Connection.Open();
            Base.Connection.Update<UserFavoriteStore>(item);
        }
    }
}
