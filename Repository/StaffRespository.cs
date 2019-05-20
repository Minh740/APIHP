using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Repository
{
    public class StaffRespository : IRepository<Staff>
    {
        private string connectionString;
        private IConfiguration config;
        public StaffRespository(IConfiguration configuration)
        {
            config = configuration;

        }

        public int? Add(Staff item)
        {
            int? staffId = null;
           
                Base.Connection.Open();
                staffId = Base.Connection.Insert<Staff>(item);
            
            return staffId;

        }
        public IEnumerable<Staff> FindbyRoleId(int id)
        {
          
                Base.Connection.Open();
                IEnumerable<Staff> staffs = Base.Connection.GetList<Staff>(" where \"Staff\".roleid =" + id.ToString());

                List<StoreRole> role = Base.Connection.GetList<StoreRole>(new { status = 1 }).ToList();


                foreach (Staff st in staffs)
                {
                    if (role.Find(c => c.Id == st.RoleId) != null)

                        st.RoleName = role.Find(c => c.Id == st.RoleId).Name;
                }
                return staffs.ToList();
            

        }

        public IEnumerable<Staff> FindByRoleId(int id)
        {
            IEnumerable<Staff> liststaff;

            Base.Connection.Open();
                liststaff = Base.Connection.GetList<Staff>("Where roleid ='" + id + "' and status = 1 ");
            Base.Connection.Close();
            

            return liststaff;


        }

        public Staff FindByID(int id)
        {

            Base.Connection.Open();
                return Base.Connection.Get<Staff>(id);
            
        }

        public List<Staff> FindByStoreID(int id)
        {

            Base.Connection.Open();
                return Base.Connection.GetList<Staff>(new { status = 1, store_id = id }).ToList();
            
        }

        public void Remove(int id)
        {

            Base.Connection.Open();
                Staff staff = Base.Connection.Get<Staff>(id);
                staff.Status = -1;
            Base.Connection.Update<Staff>(staff);
           
        }

        public void Update(Staff item)
        {

            Base.Connection.Open();
            Base.Connection.Update<Staff>(item);
            
        }



        public IEnumerable<Staff> FindAll()
        {

            Base.Connection.Open();
                

                return Base.Connection.GetList<Staff>(new { status = 1 }).ToList();
            
        }
        public IEnumerable<Staff> FindName(string Name)
        {
            IEnumerable<Staff> liststaff;

            Base.Connection.Open();
                liststaff = Base.Connection.GetList<Staff>("Where first_name like '" + Name + "%' and status = 1").ToList();
            Base.Connection.Close();
            
            return liststaff;
        }

        public bool LoginByPin(string pin, ref Staff staff, int storeid)
        {

           
                //string varidayPIN = UTIL.Encrypt(pin, true, config);
                staff = Base.Connection.GetList<Staff>("where pin = '" + pin + "' and store_id = " + storeid).FirstOrDefault();

                if (staff != null)
                    return true;
                else
                    return false;
            
        }
    }
}
