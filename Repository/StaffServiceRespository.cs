using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class StaffServiceRespository : IRepository<StaffService>
    {
        private string connectionString;
        public StaffServiceRespository(IConfiguration configuration)
        {
            
        }
     
        public int? Add(StaffService item)
        {
            int? id = null;
            
                Base.Connection.Open();
                id = Base.Connection.Insert<StaffService>(item);
            
            return id;
        }


        public StaffService FindByID(int id)
        {

            Base.Connection.Open();
            return Base.Connection.Get<StaffService>(id);
            
        }

        public void Remove(int id)
        {

            Base.Connection.Open();
                StaffService staffService = Base.Connection.Get<StaffService>(id);
                staffService.Status = -1;
            Base.Connection.Update<StaffService>(staffService);
           
        }

        public void Update(StaffService item)
        {

            Base.Connection.Open();
            Base.Connection.Update<StaffService>(item);
            
        }



        public IEnumerable<StaffService> FindAll()
        {

            Base.Connection.Open();
            return Base.Connection.GetList<StaffService>(new { status = 1 });
            
        }


        public List<StaffService> FindByStaff(int staffId)
        {

            Base.Connection.Open();
            return Base.Connection.GetList<StaffService>(new { status = 1, staff_id = staffId }).ToList();
            
        }
    }
}
