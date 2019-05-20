using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class FlagReposity : IRepository<Flag>
    {
        private string connectionString;
        private IConfiguration config;


        public FlagReposity(IConfiguration configuration)
        {
            config = configuration;

        }

        public int? Add(Flag item)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Flag> FindAll()
        {
            throw new NotImplementedException();
        }

        public Flag FindByID(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Flag item)
        {
            throw new NotImplementedException();
        }

        public Flag FindByToken(string token)
        {

            Base.Connection.Open();
            try
            {
                Flag fl = Base.Connection.GetList<Flag>("where token='" + token + "'").First();
                Base.Connection.Close();
                return fl;
            }
            catch
            {
                Base.Connection.Close();
                return null;
            }

        }
        public void AddFlag(Flag flag)
        {


            Base.Connection.Open();
            Flag fl = FindByToken(flag.Token);
            if (fl == null)
            {
                Base.Connection.Insert<Flag>(flag);
            }
            else
            {

                fl.StoreId = flag.StoreId;
                Base.Connection.Update<Flag>(fl);
            }
            Base.Connection.Close();
        }



        public void DoneFlag(string playerid)
        {


            Base.Connection.Open();
            Flag fl = FindByToken(playerid);
            if (fl != null)
            {

                fl.flag = 0;
                Base.Connection.Update<Flag>(fl);
            }
            Base.Connection.Close();
        }



        public void StartFlag(int storeid)
        {


            Base.Connection.Open();

            Base.Connection.Execute("Update \"Flag\" set flag = @flag where storeid=@storeid", new { flag = 1, storeid = storeid });

            Base.Connection.Close();


        }
    }
}
