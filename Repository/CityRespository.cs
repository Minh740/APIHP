using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class CityRespository : IRepository<City>
    {

        public CityRespository(IConfiguration configuration)
        {
       
        }
        public int? Add(City item)
        {
            int? cityId = null;

            Base.Connection.Open();
                cityId = Base.Connection.Insert<City>(item);
            
            return cityId;
        }


        public City FindByID(int id)
        {

            Base.Connection.Open();
            return Base.Connection.Get<City>(id);
            
        }

        public void Remove(int id)
        {

            Base.Connection.Open();
                City city = Base.Connection.Get<City>(id);
                city.Status = -1;
            Base.Connection.Update<City>(city);
            
        }

        public void Update(City item)
        {

            Base.Connection.Open();
            Base.Connection.Update<City>(item);
            
        }



        public IEnumerable<City> FindAll()
        {
            
                Base.Connection.Open();
            return Base.Connection.GetList<City>(new { staus = 1 });
            
        }
    }
}
