using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class CustomerRespository : IRepository<Customer>
    {
        private string connectionString;
        public CustomerRespository(IConfiguration configuration)
        {
        }
      


        public int? Add(Customer item)
        {
            int? customerId = null;
          
                Base.Connection.Open();
                customerId = Base.Connection.Insert<Customer>(item);
            
            return customerId;
        }


        public Customer FindByID(int id)
        {
            
                 Base.Connection.Open();
            return Base.Connection.Get<Customer>(id);
            
        }

        public void Remove(int id)
        {

            Base.Connection.Open();
                Customer customer = Base.Connection.Get<Customer>(id);
                customer.Status = -1;
            Base.Connection.Update<Customer>(customer);
            
        }

        public void Update(Customer item)
        {

            Base.Connection.Open();
            Base.Connection.Update<Customer>(item);
            
        }



        public IEnumerable<Customer> FindAll()
        {

            Base.Connection.Open();
            return Base.Connection.GetList<Customer>(new { status = 1 });
            
        }
        public IEnumerable<Customer> FindByStoreId(int storeid)
        {

            Base.Connection.Open();
            return Base.Connection.GetList<Customer>(new { status = 1, store_id = storeid });
            
        }
        public IEnumerable<Customer> FindByNameAndPhone(string FullName, string Phone)
        {
            IEnumerable<Customer> listcustomer;

            Base.Connection.Open();
                listcustomer = Base.Connection.GetList<Customer>("Where full_name = '" + FullName + "'  Or phone = '" + Phone + "'");
            Base.Connection.Close();
          
            return listcustomer;

        }
    }
}
