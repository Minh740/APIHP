using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Entities;


namespace Repository
{
    public static class UserRepository
    {
        public static User FindByEmail(string email)
        {
            Base.Connection.Open();
            User u = Base.Connection.Get<User>(email);
            Base.Connection.Close();
            return u;
        }

        public static User Login(string email, string password)
        {
            Base.Connection.Open();
            string query = "select * from login('@email','@password')";
            query = query.Replace("@email",email);
            query = query.Replace("@password",password);
            string login = Base.Connection.Query<Entities.SQLModels>(query).First().login;

            User u = Newtonsoft.Json.JsonConvert.DeserializeObject <User>(login.Replace("[","").Replace("]",""));
            Base.Connection.Close();
            return u;
        }
        public static int? Add(User item )
        {
            Base.Connection.Open();
            List<User> users = Base.Connection.GetList<User>(new { email = item.email }).ToList();
            if (users.Count > 0)
            {
                return 0;
            }

           int? userid= Base.Connection.Insert<User>(item);
            return userid;
        }

        public static void Update(User item)
        {


                Base.Connection.Open();

                User temp = Base.Connection.Get<User>(item.id);
                if (item.status != 1 && item.status != -1)
                    item.status = temp.status;
                if (item.full_name == null)
                    item.full_name = temp.full_name;
                if (item.first_name == null)
                    item.first_name = temp.first_name;
                if (item.last_name == null)
                    item.last_name = temp.last_name;
                if (item.email == null)
                    item.email = temp.email;
                if (item.phone == null)
                    item.phone = temp.phone;
                if (item.star < 0)
                    item.star = temp.star;
                if (item.birthday == null)
                    item.birthday = temp.birthday;
                if (item.address == null)
                    item.address = temp.address;
                if (item.city_id <= 0)
                    item.city_id = temp.city_id;
                if (item.state_id <= 0)
                    item.state_id = temp.state_id;
                if (item.last_login_time == null)
                    item.last_login_time = temp.last_login_time;
                if (item.biomaticcode == null)
                    item.biomaticcode = temp.biomaticcode;
                if (item.pin == null)
                    item.pin = temp.pin;
                if (item.password == null)
                {
                    item.password = temp.password;
                }

                item.credit = temp.credit;
                item.create_date = temp.create_date;
                Base.Connection.Update<User>(item);
            Base.Connection.Close();
        }
        public static void Remove(int id)
        {
           
                Base.Connection.Open();
                User user = Base.Connection.Get<User>(id);
                user.status = -1;
                Base.Connection.Update<User>(user);
        
        }

        public static IEnumerable<User> FindAll()
        {
           
                Base.Connection.Open();
                return Base.Connection.GetList<User>(new { status = 1 });

        }

        public static User FindByID(int id)
        {

            Base.Connection.Open();
            return Base.Connection.Get<User>(id);
            
        }

        public  static void RemoveAll()
        {
           
                Base.Connection.Open();
                IEnumerable<User> listuser = Base.Connection.GetList<User>(new { status = 1 });
                if (listuser.ToList().Count > 0)
                {
                    foreach (User _user in listuser.ToList())
                    {
                        _user.status = -1;
                    }

                }
            Base.Connection.Update<IEnumerable<User>>(listuser);
         

        }

    }

   
}
