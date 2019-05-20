using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class CountryRespository : IRepository<Country>
    {
        public CountryRespository(IConfiguration configuration)
        {
           
        }
        public int? Add(Country item)
        {
            int? countryId = null;

            Base.Connection.Open();
                countryId = Base.Connection.Insert<Country>(item);
            
            return countryId;
        }


        public Country FindByID(int id)
        {

            Base.Connection.Open();
                return Base.Connection.Get<Country>(id);
            
        }

        public void Remove(int id)
        {

            Base.Connection.Open();
                Country country = Base.Connection.Get<Country>(id);
                country.Status = -1;
            Base.Connection.Update<Country>(country);
            
        }

        public void Update(Country item)
        {

            Base.Connection.Open();
            Base.Connection.Update<Country>(item);
            
        }



        public IEnumerable<Country> FindAll()
        {
            
                Base.Connection.Open();

                return Base.Connection.GetList<Country>(new { status = 1 });
            
        }
    }
}
