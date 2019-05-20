using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public interface IRepository<T> where T : BaseEntity
    {
        int? Add(T item);
        void Remove(int id);
        void Update(T item);
        T FindByID(int id);
        IEnumerable<T> FindAll();

    }
}
