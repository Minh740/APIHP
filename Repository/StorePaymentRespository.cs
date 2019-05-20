using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class StorePaymentRespository : IRepository<StorePayment>
    {
        private string connectionString;
        public StorePaymentRespository(IConfiguration configuration)
        {
          
        }
    

        public int? Add(StorePayment item)
        {
            int? id = null;
          
            
                Base.Connection.Open();
                id = Base.Connection.Insert<StorePayment>(item);
            
            return id;
        }


        public StorePayment FindByID(int id)
        {

            Base.Connection.Open();
            return Base.Connection.Get<StorePayment>(id);
            
        }

        public void Remove(int id)
        {

            Base.Connection.Open();
                StorePayment storePayment = Base.Connection.Get<StorePayment>(id);
                storePayment.Status = -1;
            Base.Connection.Update<StorePayment>(storePayment);
            
        }

        public void Update(StorePayment item)
        {

            Base.Connection.Open();
            Base.Connection.Update<StorePayment>(item);
            
        }



        public IEnumerable<StorePayment> FindAll()
        {

            Base.Connection.Open();

            return Base.Connection.GetList<StorePayment>(new { status = 1 });
            
        }
    }
}
