using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics;
using web_product.Entities;
using web_product.Models;
using web_product.Repository;



namespace web_product.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {

        private IRepository<Products> _repository { get; }

        public AdminController(IRepository<Products> repository)
        {

            _repository = repository;

        }
        
        public IActionResult Index() 
        {

            return View(_repository.GetProductsList());
        }

    
        public ActionResult Create()
        {

            return View();
        }

      
        [HttpPost]
        public ActionResult Create(Products item) 
        {

            if (ModelState.IsValid)
            {
                _repository.Create(item);
                _repository.Save();
                return RedirectToAction("Index");
            }

            return View("Edit");
        }

       
        public ActionResult Edit(int id)
        {
            Products editProducts = _repository.GetProductsId(id);

            return View(editProducts);
        }

        [HttpPost]
        public ActionResult Edit(Products edit)
        {
            if (ModelState.IsValid)
            {
                _repository.Update(edit);
                _repository.Save();

            }

            return RedirectToAction("Index");
        }


    }

}


