using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{

    public class GiftCardRespository : IRepository<GiftCard>
    {
        private string connectionString;
        public GiftCardRespository(IConfiguration configuration)
        {
        
        }
        public int? Add(GiftCard item)
        {
            int? giftId = null;

            Base.Connection.Open();
                giftId = Base.Connection.Insert<GiftCard>(item);
            
            return giftId;
        }


        public GiftCard FindByID(int id)
        {

            Base.Connection.Open();
            return Base.Connection.Get<GiftCard>(id);
            
        }

        public void Remove(int id)
        {

            Base.Connection.Open();
                GiftCard giftCard = Base.Connection.Get<GiftCard>(id);
            Base.Connection.Update<GiftCard>(giftCard);
            
        }

        public void Update(GiftCard item)
        {

            Base.Connection.Open();
            Base.Connection.Update<GiftCard>(item);
            
        }



        public IEnumerable<GiftCard> FindAll()
        {
           
                Base.Connection.Open();
            return Base.Connection.GetList<GiftCard>(new { status = 1 });
            
        }
    }
}
