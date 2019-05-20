using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class GeneralRespository : IRepository<General>
    {
        private string connectionString;
        public GeneralRespository(IConfiguration configuration)
        {
          
        }

        public int? Add(General item)
        {
            int? giftId = null;

            Base.Connection.Open();
                giftId = Base.Connection.Insert<General>(item);
            
            return giftId;
        }


        public General FindByID(int id)
        {

            Base.Connection.Open();
                return Base.Connection.Get<General>(id);
            
        }

        public void Remove(int id)
        {

            Base.Connection.Open();
                General General = Base.Connection.Get<General>(id);
            Base.Connection.Update<General>(General);
            
        }

        public void Update(General item)
        {

            Base.Connection.Open();
            Base.Connection.Update<General>(item);
            
        }



        public IEnumerable<General> FindAll()
        {
           
                Base.Connection.Open();
                return Base.Connection.GetList<General>(new { status = 1 });
            
        }
    }
}
