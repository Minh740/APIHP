using Dapper;
using Entities;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class CategoryRespository : IRepository<Category>
    {
        private IConfiguration config;
        public CategoryRespository(IConfiguration configuration)
        {
            config = configuration;
        }

        public int? Add(Category item)
        {
            int? categoryId = null;

            Base.Connection.Open();
                categoryId = Base.Connection.Insert<Category>(item);
            
            return categoryId;
        }


        public Category FindByID(int id)
        {

            Base.Connection.Open();
                return Base.Connection.Get<Category>(id);
            
        }

        public void Remove(int id, ref string message)
        {
            try
            {

                Base.Connection.Open();
                    Category category = Base.Connection.Get<Category>(id);
                Base.Connection.Query<Service>("update \"Service\"set category_id = null where category_id = " + category.Id.ToString());
                Base.Connection.Query<Service>("update \"Product\"set category_id = null where category_id = " + category.Id.ToString());

                    category.Status = -1;
                Base.Connection.Update<Category>(category);
                    message = "Success";
                
            }

            catch (Exception ex)
            {
                message = ex.ToString();
            }
        }

        public bool Update(Category item, ref string message)
        {
            try
            {

                Base.Connection.Open();
                Base.Connection.Query<Service>("update \"Service\"set category_id = null where category_id = " + item.Id.ToString());
                Base.Connection.Query<Service>("update \"Product\"set category_id = null where category_id = " + item.Id.ToString());
                Base.Connection.Update<Category>(item);
                    message = "Success";
                    return true;
                
            }

            catch (Exception ex)
            {
                message = ex.ToString();
                return false;
            }
        }



        public IEnumerable<Category> FindByStoreID(int id, ref List<Extra> exlist)
        {

            Base.Connection.Open();
                using (var connect = Base.Connection.BeginTransaction())
                {

                    IEnumerable<Category> Categorys = Base.Connection.GetList<Category>(new { store_id = id, status = 1 });
                    IEnumerable<Service> Services = Base.Connection.GetList<Service>(new { status = 1 });
                    IEnumerable<Extra> Extras = Base.Connection.GetList<Extra>(new { store_id = id, status = 1 });
                    IEnumerable<Product> Products = Base.Connection.GetList<Product>(new { status = 1 });
                    IEnumerable<Service_Extra> SE = Base.Connection.GetList<Service_Extra>(new { status = 1 });



                    foreach (Category cat in Categorys.ToList())
                    {
                        cat.Service = new List<Service>();
                        cat.Products = new List<Product>();
                        if (Products.ToList().Count > 0)
                        {
                            foreach (Product PRO in Products.ToList())
                            {
                                if (PRO.CategoryId == cat.Id)
                                    cat.Products.Add(PRO);
                            }

                        }
                        if (Services.ToList().Count > 0)
                        {
                            foreach (Service ser in Services.ToList())
                            {
                                ser.ListExtra = new List<Extra>();
                                if (ser.CategoryId == cat.Id)
                                {
                                    cat.Service.Add(ser);

                                    List<Service_Extra> ListSE = SE.ToList().FindAll(c => c.ServiceId == ser.Id);
                                    if (ListSE.Count > 0)
                                    {
                                        if (Extras.ToList().Count > 0)
                                        {
                                            foreach (Service_Extra se in ListSE)
                                            {
                                                Extra temex = Extras.ToList().Find(c => c.Id == se.ExtraId);
                                                if (temex != null)
                                                    ser.ListExtra.Add(temex);
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                    exlist = Extras.ToList();
                    connect.Commit();
                    return Categorys.ToList();


                }
            

        }
        public IEnumerable<Category> FindAll()
        {
            Base.Connection.Open();

                return Base.Connection.GetList<Category>(new { status = 1 });
            
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Category item)
        {
            throw new NotImplementedException();
        }
        public bool InsertListService(List<Category> Category)
        {

            
                var json = JsonConvert.SerializeObject(Category);

                bool resuilt = Base.Connection.Query<bool>("select\" InsertIntoCategory\"('" + json + "')").First();

                return resuilt;
            
        }
    }
}
