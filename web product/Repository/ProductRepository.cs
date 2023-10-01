using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Diagnostics.Metrics;
using web_product.Entities;
using web_product.Repository;
namespace web_product.Repository
{
    public class ProductRepository : IRepository<Products> //реализация методов IRepository для работы с базой данных
    {
        private MsqlContext MsqlContext { get; }

        public ProductRepository(MsqlContext msqlContext) => MsqlContext = msqlContext;

        public IEnumerable<Products> GetProductsList()
        {
            return MsqlContext.Products.ToList();
        }
        
        public Products GetProductsId(int id)
        {            
            return MsqlContext.Products.Find(id);
        }
        
        public void Create(Products item)
        {          
            Products? products = MsqlContext.Products.FirstOrDefault(x => x.Id == item.Id);
            MsqlContext.Products.Add(item);
        }

        public void Update(Products item)
        {
            MsqlContext.Entry(item).State = EntityState.Modified;
        }

      
        public void Save()
        {
            MsqlContext.SaveChanges();
        }



        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    MsqlContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

}

