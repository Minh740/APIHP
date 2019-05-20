using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class StoreRepository : IRepository<Store>
    {
        public StoreRepository(IConfiguration configuration)
        {
          
        }
        public int? Add(Store item, ref string mesage)
        {
            int? storeId = null;
           
                Base.Connection.Open();

                Store store = Base.Connection.Get<Store>(item.merchantid);
                Merchant merchant = Base.Connection.Get<Merchant>(item.merchantid);
                if (store == null && merchant != null)
                    storeId = Base.Connection.Insert<Store>(item);
                else
                {
                    if (store != null)
                    { mesage = "MerchantId"; return -1; }
                    else
                    {
                        if (merchant == null)
                        {
                            mesage = "Merchant don't exist";
                            return -2;
                        }
                    }
                }
            
            return storeId;
        }


        public Store FindByID(int id)
        {

            Base.Connection.Open();
                return Base.Connection.Get<Store>(id);
            
        }

        public Store FindByMerchantId(int merchantId)
        {

            Base.Connection.Open();
                Store resuilt = Base.Connection.Query<Store>("Select*from \"Store\" Where merchantId =  " + merchantId).FirstOrDefault();
                return Base.Connection.Query<Store>("Select*from \"Store\" Where merchantId =  " + merchantId).FirstOrDefault();
            
        }

        public void Remove(int id)
        {

            Base.Connection.Open();
                Store store = Base.Connection.Get<Store>(id);
                store.Status = -1;
            Base.Connection.Update(store);
            
        }

        public void Update(Store item)
        {

            Base.Connection.Open();
            Base.Connection.Update<Store>(item);
            
        }

        public IEnumerable<Store> FindAll()
        {

            Base.Connection.Open();
                return Base.Connection.GetList<Store>(new { status = 1 }).ToList();
            
        }

        public List<Store> FindNest(Coordinate coordinate)
        {


            Base.Connection.Open();
            //GeoCoordinate myLocation = new GeoCoordinate(coordinate.Latitude, coordinate.Longitude);
            //List<Store> stores = FindAll().Where(ele => myLocation.GetDistanceTo(new GeoCoordinate(ele.Lat, ele.Long)) < coordinate.Distance).ToList();
            return null; /* stores;*/
            
        }

        public int? Add(Store item)
        {
            throw new NotImplementedException();
        }
    }
}
