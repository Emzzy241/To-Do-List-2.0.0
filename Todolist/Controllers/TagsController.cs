using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    public class TagsController : Controller
    {
        private readonly ToDoListContext _db;
        public TagsController(ToDoListContext db)
        {
            _db = db;
        }

        public ActionResult Index()
        {
            return View(_db.Tags.ToList()); // a shortcut, instead of saving our lisst of tags to a variable and passing it into the View() method, we pass in the method call _db.Tags.ToList() directly as the argument to View();
        }

        public ActionResult Details(int id)
        {
            Tag thisTag = _db.Tags
                .Include(tag => tag.JoinEntities) //To load Entities property of each tag. However, JoinEntitties property on a Tag is just a collection of join entities(List<ItemTag>) which are tracked by ids: ItemTagId, TagId, and ItemId. They are not the actual item objects related to a Tag.
                .ThenInclude(join => join.Item) // getting the actual Item objects, using .THenInclude() to load the item object associated with each Tag. used to fetch the associated Item object for each ItemTag join entities
                .FirstOrDefault(tag => tag.TagId == id);
            return View(thisTag);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Tag tag)
        {
            _db.Tags.Add(tag);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // ability to create many-to-many associations between tags and items
    public ActionResult AddItem(int id)
    {
      Tag thisTag = _db.Tags.FirstOrDefault(tags => tags.TagId == id);
      ViewBag.ItemId = new SelectList(_db.Items, "ItemId", "Description");
      return View(thisTag);
    }

    // The AddItem() POST action contains some new code to ensure that we don't create a join relationship in two scenarios: when there's no item in the select list with which to create a join relationship, and when a join relationship between a tag and item already exists. If either of these scenarios are true, that means we don't want to create a new join relationship and we simply route back to the details page.
    // For the first scenario, to prevent the creation of a join relationship when there's no item in the select list, all we need to do is double check the value of itemId: if it's equal to 0, there's no item in the select list with which to create a join relationship. We do this in the if statement with the condition itemId != 0.
    // For the second scenario, to prevent creating duplicate join relationships we need to go through a process of checking whether the join relationship already exists
    [HttpPost]
    public ActionResult AddItem(Tag tag, int itemId)
    {
        // code for 2nd screnario, avoiding the creation of duplicate join relationships
      #nullable enable
      ItemTag? joinEntity = _db.ItemTags.FirstOrDefault(join => (join.ItemId == itemId && join.TagId == tag.TagId)); //Since our joinEntity variable will be either an ItemTag object or null, we need to make it a nullable type. We can turn a type into a nullable type by adding a question mark ? at the end of the type, like ItemTag?.
      #nullable disable
      if (joinEntity == null && itemId != 0)
      {
        _db.ItemTags.Add(new ItemTag() { ItemId = itemId, TagId = tag.TagId });
        _db.SaveChanges();
      }
      return RedirectToAction("Details", new { id = tag.TagId });

        // To use nullable reference types, e must also have a nullable annotation context enabled so that our C# compiler can process the nullable reference types. We can enable a nullable annotation context for our entire app via our .csproj file, or for a file or a few lines of code with nullable directives: #nullable enable and #nullable disable. We're opting for the latter in our code because it will require less refactoring across our whole app
        // To complete the checking process for duplicate join relationships, we simply need to check if joinEntity == null in our conditional. If the result of our search for duplicates is null, it means that we can move forward with creating the new join relationship in our database.
    }

        public ActionResult Edit(int id)
        {
            Tag thisTag = _db.Tags.FirstOrDefault(tags => tags.TagId == id);
            return View(thisTag);
        }

        [HttpPost]
        public ActionResult Edit(Tag tag)
        {
            _db.Tags.Update(tag);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            Tag thisTag = _db.Tags.FirstOrDefault(tags => tags.TagId == id);
            return View(thisTag);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Tag thisTag = _db.Tags.FirstOrDefault(tags => tags.TagId == id);
            _db.Tags.Remove(thisTag);
            _db.SaveChanges();
            return RedirectToAction("Index");
        // when we delete a Tag, all join entities that reference that tag's TagId will also automatically be deleted. This is configured through foreign key constraints, which configures how objects in relationships should be deleted or updated when one object in the relationship is deleted or updated.    
        }

        // code to remove a database entry
        [HttpPost]
        public ActionResult DeleteJoin(int joinId)
        {
            ItemTag joinEntry = _db.ItemTags.FirstOrDefault(entry => entry.ItemTagId == joinId);
            _db.ItemTags.Remove(joinEntry);
            _db.SaveChanges();
            return RedirectToAction("Index");
            // we're going into the ItemTags database table. Note that the DeleteJoin() action will find the entry in the join table by using the join entry's ItemTagId. The ItemTagId is being passed in through the variable joinId in our action's parameter and came from the BeginForm() HTML helper method in our details view.
            // Note that we've called our action DeleteJoin() because it handles deleting an ItemTag join relationship. However we could name this route something more specific if we prefer, like DeleteItemTagJoin, or if we happen to have multiple many-to-many relationships
        }

    }
}