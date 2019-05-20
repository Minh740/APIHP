using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Repository
{
    public class UserFavoriteStaffRespository : IRepository<UserFavoriteStaff>
    {
        private IConfiguration config;
        public UserFavoriteStaffRespository(IConfiguration configuration)
        {
            config = configuration;

        }
        public int? Add(UserFavoriteStaff item)
        {
            int? userFavoriteStaffId = 0;
           
                Base.Connection.Open();
                userFavoriteStaffId = Base.Connection.Insert<UserFavoriteStaff>(item);
           
            return userFavoriteStaffId;
        }

        public IEnumerable<UserFavoriteStaff> FindAll()
        {

                 Base.Connection.Open();
                return Base.Connection.GetList<UserFavoriteStaff>(new { status = 1 });
            
        }

        public UserFavoriteStaff FindByID(int id)
        {

            Base.Connection.Open();
                return Base.Connection.Get<UserFavoriteStaff>(id);
           
        }

        public void Remove(int id)
        {

            Base.Connection.Open();
                UserFavoriteStaff userFavoriteStaff = Base.Connection.Get<UserFavoriteStaff>(id);
                userFavoriteStaff.Status = -1;
            Base.Connection.Get<UserFavoriteStaff>(userFavoriteStaff);
            
        }

        public void Update(UserFavoriteStaff item)
        {

            Base.Connection.Open();
            Base.Connection.Update<UserFavoriteStaff>(item);
            
        }
    }
}
