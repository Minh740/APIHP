using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class BusinessbankRespository : IRepository<Businessbank>
    {
        public BusinessbankRespository(IConfiguration configuration)
        {
           
        }

        public int? Add(Businessbank item)
        {
            int? giftId = null;

            Base.Connection.Open();
                giftId = Base.Connection.Insert<Businessbank>(item);
            
            return giftId;
        }


        public Businessbank FindByID(int id)
        {

            Base.Connection.Open();
                return Base.Connection.Get<Businessbank>(id);
            
        }

        public void Remove(int id)
        {

            Base.Connection.Open();
                Businessbank Businessbank = Base.Connection.Get<Businessbank>(id);
            Base.Connection.Update<Businessbank>(Businessbank);
            
        }

        public void Update(Businessbank item)
        {

            Base.Connection.Open();
            Base.Connection.Update<Businessbank>(item);
            
        }



        public IEnumerable<Businessbank> FindAll()
        {
          
                Base.Connection.Open();
                return Base.Connection.GetList<Businessbank>(new { status = 1 });
            
        }
    }
}
