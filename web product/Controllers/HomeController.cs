using Microsoft.AspNetCore.Mvc;
using web_product.Entities;
using System.Linq;
using web_product.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using web_product.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http.Json;
using System.Net.Http.Json;
using System.Xml;
using Microsoft.AspNetCore.Authorization;

namespace web_product.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        private IRepository<Products> _repository { get; }


        public HomeController(IRepository<Products> repository)
        {
            _repository = repository;
        }


       [HttpGet] //Получить все товары с подробнм описанием
        public IActionResult GetAllProducts()
        {
            List<Products> result = _repository.GetProductsList()
                                               .ToList();


            JsonSerializerOptions options = new()
            {
                
                WriteIndented = true, //добавляем пробелы
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping //не экранируем символы в строках
            };

            var json = JsonSerializer.Serialize(result, options);
            return Ok(json);
        }

        [HttpGet("name/{name}")] //Получить товары с фильтрацией по названию и диапазону цен

        public IActionResult GetProductsFilter(string name,decimal? price=null)
        {
            
                List<Products> result = _repository.GetProductsList()
                                                   .Where(x => x.Name == name &&
                                                   x.Price == price && 
                                                   x.Price <= x.Price && 
                                                   x.Price >= x.Price)
                                                   .ToList();
            var list = result.Select(w => new Products()
            {
                Id = w.Id,
                Name = w.Name,
                Price = w.Price

            }).ToList();
            
            JsonSerializerOptions options = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, //Не выводить параметры с null

                WriteIndented = true, //Добавление пробелов
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, //Не экранировать символы в строках            
            };

            
            var json = JsonSerializer.Serialize(list, options);
            return Ok(json);
        }
    }
}

