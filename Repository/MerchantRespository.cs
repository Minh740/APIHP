using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class MerchantRespository : IRepository<Merchant>
    {

        private IConfiguration config;
        public MerchantRespository(IConfiguration configuration)
        {
            config = configuration;
        }
   
        public int? Add(Merchant merchant, ref string Messageage)
        {
            int? merchantId = 0;

            Base.Connection.Open();
                List<Merchant> merchants = Base.Connection.GetList<Merchant>(new { email = merchant.Email }).ToList();
                if (merchants.Count > 0)
                {
                    Messageage = "Email";
                    return 0;
                }
              
                merchantId = Base.Connection.Insert<Merchant>(merchant);
            
            return merchantId;
        }


        public Merchant FindByID(int id)
        {

            Base.Connection.Open();
                Merchant merchant = Base.Connection.Get<Merchant>(id);
                if (merchant != null)
                    merchant.Password = null;
                return merchant;
            
        }

        public void Remove(int id)
        {

            Base.Connection.Open();
                Merchant merchant = Base.Connection.Get<Merchant>(id);
                merchant.Status = -1;
            Base.Connection.Update<Merchant>(merchant);
            
        }

        public void Update(Merchant merchant)
        {

            Base.Connection.Open();

            Base.Connection.Update<Merchant>(merchant);
            
        }



        public IEnumerable<Merchant> FindAll()
        {

            Base.Connection.Open();
                List<Merchant> merchants = Base.Connection.GetList<Merchant>(new { status = 1 }).ToList();
                merchants.ForEach(ele => { ele.Password = null; });
                return merchants;
            
        }

        public bool Login(string email, string ecryptPass, ref Merchant merchant, ref string Messageage)
        {
            
                Base.Connection.Open();
                merchant = Base.Connection.GetList<Merchant>(new { email = email }).FirstOrDefault();
                if (merchant == null)
                {
                    Messageage = "Your email address is not registered";
                    return false;
                }
                if (merchant.Password != ecryptPass)
                {
                    Messageage = "Password incorrect";
                    return false;
                }
            
            return true;
        }

        public int? Add(Merchant item)
        {
            throw new NotImplementedException();
        }
    }
}
