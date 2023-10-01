using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Diagnostics.Metrics;
using web_product.Entities;

namespace web_product.Repository
{
  public  interface IRepository<T> : IDisposable // методы для работы с сущностями базы данных:
         where T : class
    { 
        IEnumerable<T> GetProductsList(); // получение всех объектов
        T GetProductsId(int id); // получение одного объекта по id
        void Create(Products item); // создание объекта
        void Update(Products item); // обновление объекта      
        void Save();  // сохранение изменений
    }
}
