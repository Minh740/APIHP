using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class StateRespository : IRepository<State>
    {
        private string connectionString;
        public StateRespository(IConfiguration configuration)
        {
           
        }
    
        public int? Add(State item)
        {
            int? stateId = null;
           
                Base.Connection.Open();
                stateId = Base.Connection.Insert<State>(item);
            
            return stateId;
        }


        public State FindByID(int id)
        {

            Base.Connection.Open();
            return Base.Connection.Get<State>(id);
            
        }

        public void Remove(int id)
        {

            Base.Connection.Open();
                State state = Base.Connection.Get<State>(id);
                state.Status = -1;
            Base.Connection.Update<State>(state);
            
        }

        public void Update(State item)
        {

            Base.Connection.Open();
            Base.Connection.Update<State>(item);
            
        }



        public IEnumerable<State> FindAll()
        {
            
                Base.Connection.Open();

            return Base.Connection.GetList<State>(new { status = 1 });

            
        }
    }
}
