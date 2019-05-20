using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class PaymentRespository : IRepository<Payment>
    {
       
        public PaymentRespository(IConfiguration configuration)
        {
           
        }

        public int? Add(Payment item)
        {
            int? paymentId = null;

            Base.Connection.Open();
                paymentId = Base.Connection.Insert<Payment>(item);
            
            return paymentId;
        }


        public Payment FindByID(int id)
        {
            Base.Connection.Open();
            return Base.Connection.Get<Payment>(id);
            
        }

        public void Remove(int id)
        {

            Base.Connection.Open();
                Payment payment = Base.Connection.Get<Payment>(id);
                payment.Status = -1;
            Base.Connection.Update<Payment>(payment);

            
        }

        public void Update(Payment item)
        {

            Base.Connection.Open();
            Base.Connection.Update<Payment>(item);
            
        }



        public IEnumerable<Payment> FindAll()
        {
           
                Base.Connection.Open();

            return Base.Connection.GetList<Payment>(new { status = 1 });
            
        }
    }
}
