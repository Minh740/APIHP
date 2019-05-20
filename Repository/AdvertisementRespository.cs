using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public static class AdvertisementRespository
    {
      
        public static int? Add(Advertisement item)
        {
            int? advertisementId = null;

            Base.Connection.Open();
                advertisementId = Base.Connection.Insert<Advertisement>(item);
            
            return advertisementId;
        }


        public static Advertisement FindByID(int id)
        {

            Base.Connection.Open();
            return Base.Connection.Get<Advertisement>(id);
            
        }

        public static void Remove(int id)
        {

            Base.Connection.Open();
                Advertisement advertisement = Base.Connection.Get<Advertisement>(id);
                advertisement.Status = -1;
            Base.Connection.Update<Advertisement>(advertisement);
            
        }

        public static void Update(Advertisement item)
        {

            Base.Connection.Open();

          Base.Connection.Update<Advertisement>(item);
            
        }



        public static IEnumerable<Advertisement> FindAll()
        {
            
                Base.Connection.Open();

            return Base.Connection.GetList<Advertisement>(new { status = 1 });
            
        }
    }
}
