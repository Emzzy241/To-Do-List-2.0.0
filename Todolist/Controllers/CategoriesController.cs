using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // So we can make use of the Include() method
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Rendering; //add the necessary using directive so that we have access to SelectList.
using System;
using System.Linq; // allows us to use the method ToList()
using System.Collections.Generic;
using ToDoList.Models;



namespace ToDoList.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ToDoListContext _db;
        public CategoriesController(ToDoListContext db)
        {
            _db = db;
        }

         public ActionResult Index()
        {
            List<Category> model = _db.Categories.ToList();
            // Working with a ViewBag to add custom Titles to our app
             ViewBag.PageTitle = "View All Categories";
            return View(model);
        }

        public ActionResult Create()
        {
             ViewBag.PageTitle = "Create a Category";
            return View();
        }

        [HttpPost]
        public ActionResult Create(Category category)
        {
            _db.Categories.Add(category);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            // Category has only one navigation property, Category.Items; this is why there is only one .Include() method call. If we want to want to access each item's tag(s), we need to use a series of .ThenInclude() method calls to get the Item.JoinEntities data for each item, and then the joinEntity.Tag data for each entity
            Category thisCategory = _db.Categories
                                    .Include(category => category.Items) // Here we want to include the Items property, which tells EF Core to fetch every Item object belonging to the Category
                                    .ThenInclude( item => item.JoinEntities)
                                    .ThenInclude(join => join.Tag) // with the 2 .ThenInclude(), we are fetching not only a list of items, but each item's tags
                                    .FirstOrDefault(category => category.CategoryId == id);
             ViewBag.PageTitle = "Category Details";
            return View(thisCategory);
            //Just like before, If we don't explicitly tell EF Core to include the navigation property Category It won't. However We'll still get the Category.CategoryId, Category.Name information, but the navigation property Category will be empty
        }

        public ActionResult Edit(int id)
        {
            Category thisCategory = _db.Categories.FirstOrDefault(category => category.CategoryId == id);
             ViewBag.PageTitle = "Edit Category";
            return View(thisCategory);
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            _db.Categories.Update(category);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            Category thisCategory = _db.Categories.FirstOrDefault(category => category.CategoryId == id);
             ViewBag.PageTitle = "Delete Category?";
            return View(thisCategory);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Category thisCategory = _db.Categories.FirstOrDefault(category => category.CategoryId == id);
            _db.Categories.Remove(thisCategory);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }



      
    }

}


