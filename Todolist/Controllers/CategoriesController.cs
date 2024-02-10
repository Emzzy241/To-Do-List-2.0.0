using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
  public class CategoriesController : Controller
  {
    
    [HttpGet("/categories")]
    public ActionResult Index()
    {
      List<Category> allCategories = Category.GetAll();
      return View(allCategories);
    }

    // Working with the RESTful conventions to display a New() view... This is a form that will ask for the category an Item falls under
    [HttpGet("/categories/new")]
    public ActionResult New()
    {
        return View();
    }

    // To process submissions from this form, we'll need a route that handles POST requests with "/categories" paths(since thats where our form from New() view will be submitting the post requests). We'll call it Create() in order to follow RESTful conventions
    [HttpPost("/categories")]
    public ActionResult Create(string categoryName)
    {
        Category newCategory = new Category(categoryName);
        return RedirectToAction("Index");
    }


    // This one creates new Items within a given Category, not new Categories:

     [HttpPost("/categories/{categoryId}/items")]
    public ActionResult Create(int categoryId, string itemDescription)
    {
      Dictionary<string, object> model = new Dictionary<string, object>();
      Category foundCategory = Category.Find(categoryId);
      Item newItem = new Item(itemDescription);
      foundCategory.AddItem(newItem);
      List<Item> categoryItems = foundCategory.Items;
      model.Add("items", categoryItems);
      model.Add("category", foundCategory);
      return View("Show", model);
    }

    // For the Show() feature
    [HttpGet("/categories/{categoryId}")]
    public ActionResult Show(int categoryId)
    {
        Dictionary<string, object> model = new Dictionary<string, object>();
        Category selectedCategory = Category.Find(categoryId);
        List<Item> categoryItems = selectedCategory.Items;
        model.Add("category", selectedCategory);
        model.Add("items", categoryItems);
        return View(model);
        /*
            We're doing something new here. Because this page will display both a Category and all Item objects saved within that Category, we must pass two types of objects to the view. However, View() can only accept one model argument. To work around this, we do the following:
            Create a new Dictionary called model because a Dictionary can hold multiple key-value pairs.
            Add both the Category and its associated Items to this Dictionary. These will be stored with the keys "category" and "items".
            The Dictionary, which is named model, will be passed into View().
        
            In the Show.cshtml file;
            We loop through all Items in the model. We passed the view a Dictionary containing key-value pairs, so we access Items with @Model["items"] square bracket notation.
            For each Item, we display its description in a <li>.
            We've also made each <li> a link to the path '/categories/@Model["category"].Id/items/@item.Id'. This will be the item's detail view. We already have an Item detail page associated with the Show() route on the ItemsController, but it's not at this super long path! Don't worry, this is intentional. We'll discuss what's up in the next lesson.
        */

    }
  
  }
}