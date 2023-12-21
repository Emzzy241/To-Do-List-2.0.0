using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ItemsController : Controller
    {
        [HttpGet("/items")]
        public ActionResult Index()
        {
            List<Item> myItemList = Item.GetAll();
            return View(myItemList);
        }



        [HttpGet("/items/new")]
        public ActionResult New()
        {
            return View();
        }


        [HttpPost("/items")]
        public ActionResult Create(string description)
        {
        Item myItem = new Item(description);
        return RedirectToAction("Index");
        }
        
        [HttpGet("/items/{id}")]
        public ActionResult Show(int id)
        {
            Item foundItem = Item.Find(id);
            return View(foundItem);
        }

        [HttpGet("/items/delete")]
        public ActionResult Delete()
        {
            return View();
        }
        
        [HttpPost("items/deleteall")]
        public ActionResult DeleteAll()
        {
            Item.ClearAll();
            return RedirectToAction("Index");
        }

        


    }
}