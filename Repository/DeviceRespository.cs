using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class DeviceRespository : IRepository<Device>
    {
        private string connectionString;
        public DeviceRespository(IConfiguration configuration)
        {

        }
  

        public int? Add(Device item)
        {
            int? deviceId = null;
         
            
                Base.Connection.Open();
                deviceId = Base.Connection.Insert<Device>(item);
            
            return deviceId;
        }


        public Device FindByID(int id)
        {

            Base.Connection.Open();
                return Base.Connection.Get<Device>(id);
            
        }

        public void Remove(int id)
        {

            Base.Connection.Open();
                Device device = Base.Connection.Get<Device>(id);
                device.Status = -1;
            Base.Connection.Update<Device>(device);
            
        }

        public void Update(Device item)
        {

            Base.Connection.Open();
            Base.Connection.Update<Device>(item);
            
        }

        public IEnumerable<Device> FindAll()
        {

            Base.Connection.Open();

                return Base.Connection.GetList<Device>(new { status = 1 });
            
        }
    }
}
