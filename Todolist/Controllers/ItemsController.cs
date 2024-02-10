using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class ItemsController : Controller
    {
        




        // The path now includes the ID of the Category we're adding a new Item to. Because it's in curly braces, we can grab this in our route's parameter to locate the Category object and pass it into the corresponding view.
        [HttpGet("/categories/{categoryId}/items/new")]
        public ActionResult New(int categoryId)
        {
            Category category = Category.Find(categoryId);
            return View(category);
        }


        
        
        [HttpGet("/categories/{categoryId}/items/{itemId}")]
        public ActionResult Show(int categoryId, int itemId)
        {
            Item item = Item.Find(itemId);
            Category category = Category.Find(categoryId);
            Dictionary<string, object> model = new Dictionary<string, object>();
            model.Add("item", item);
            model.Add("category", category);
            return View(model);
        }


    }
}