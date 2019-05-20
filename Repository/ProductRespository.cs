using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class ProductRespository : IRepository<Product>
    {
       
        public ProductRespository(IConfiguration configuration)
        {
            
        }
      


        public int? Add(Product item)
        {
            int? productId = null;
           
                Base.Connection.Open();
                productId = Base.Connection.Insert<Product>(item);
            
            return productId;
        }


        public Product FindByID(int id)
        {

            Base.Connection.Open();
            return Base.Connection.Get<Product>(id);
            
        }

        public void Remove(int id)
        {

            Base.Connection.Open();
                Product product = Base.Connection.Get<Product>(id);
            Base.Connection.Update<Product>(product);
            
        }

        public void Update(Product item)
        {

            Base.Connection.Open();
            Base.Connection.Update<Product>(item);
            
        }

        public IEnumerable<Product> FindCatgoryAndStoreId(int Catid, int Storeid)
        {

            Base.Connection.Open();
                IEnumerable<Product> stores = Base.Connection.GetList<Product>(new { status = 1 });

                foreach (Product pro in stores.ToList())
                {
                    if (pro.StoreId != Storeid && pro.CategoryId != Catid)

                    {
                        stores.ToList().Remove(pro);

                    }
                }

                return stores.ToList();
            
        }

        public IEnumerable<Product> FindAll()
        {

            Base.Connection.Open();
            return Base.Connection.GetList<Product>(new { status = 1 });
            
        }
        public IEnumerable<Product> FindByNameOrSku(Key_Name_Sku key)
        {

            Base.Connection.Open();
            return Base.Connection.GetList<Product>("where status=1 and name =" + key.name + " or sku = " + key.Sku);
            
        }

        public IEnumerable<Product> InsertListService(string json)
        {
            IEnumerable<Product> resuilt;

            Base.Connection.Open();
                resuilt = Base.Connection.Query<Product>("select InsertIntoProduct (" + json + ")");
            
            return resuilt;
        }
        public IEnumerable<Product> SearchNameSku(string Name, string Sku)
        {
            IEnumerable<Product> resuilt;

            Base.Connection.Open();
                resuilt = Base.Connection.GetList<Product>("Where status = 1 and name ='" + Name + "' Or sku ='" + Sku + "' ");
            Base.Connection.Close();
            
            return resuilt;
        }
        public IEnumerable<Product> ListByCategory(int id)
        {
            IEnumerable<Product> listbycategory;

            Base.Connection.Open();
                listbycategory = Base.Connection.GetList<Product>("Where status = 1 and category_id ='" + id + "'");
            Base.Connection.Close();
            
            return listbycategory;
        }
    }
}
