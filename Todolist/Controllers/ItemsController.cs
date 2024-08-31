using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // So we can make use of the Include() method
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Rendering; // Add the necessary using directive so that we have access to SelectList.
using Microsoft.AspNetCore.Authorization; // To implement Authorization in the ItemsController
using Microsoft.AspNetCore.Identity; // For using UserManager and other Identity tools
using System.Threading.Tasks; // Necessary to call async methods.
using System.Security.Claims; // Necessary for claim-based authorization.
using System;
using System.Linq; // Allows us to use LINQ methods like ToList()
using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.Controllers
{
    [Authorize]
    public class ItemsController : Controller
    {
        private readonly ToDoListContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public ItemsController(UserManager<ApplicationUser> userManager, ToDoListContext database)
        {
            _db = database;
            _userManager = userManager;
        }

        public async Task<ActionResult> Index()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
            List<Item> userItems = _db.Items
                                .Where(entry => entry.User.Id == currentUser.Id)
                                .Include(item => item.Category)
                                .ToList();
            return View(userItems);
        }

        public ActionResult Create()
        {
            ViewBag.PageTitle = "Create an Item";
            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(Item item, int CategoryId)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
                return View(item);
            }

            string userId = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID is null or user is not authenticated.");
            }

            ApplicationUser currentUser = await _userManager.FindByIdAsync(userId);
            if (currentUser == null)
            {
                return NotFound("User not found.");
            }

            if (item == null)
            {
                return BadRequest("Item cannot be null.");
            }

            item.User = currentUser;

            try
            {
                _db.Items.Add(item);
                await _db.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error. Please try again later.");
            }

            return RedirectToAction("Index");
        }

        public ActionResult Details(int id)
        {
            ViewBag.PageTitle = "Item Details";
            Item thisItem = _db.Items
                                .Include(item => item.Category)
                                .Include(item => item.JoinEntities)
                                .ThenInclude(join => join.Tag)
                                .FirstOrDefault(item => item.ItemId == id);
            return View(thisItem);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.PageTitle = "Edit an Item";
            Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
            ViewBag.CategoryId = new SelectList(_db.Categories, "CategoryId", "Name");
            return View(thisItem);
        }

        [HttpPost]
        public ActionResult Edit(Item item)
        {
            _db.Items.Update(item);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            ViewBag.PageTitle = "Delete Item";
            Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
            return View(thisItem);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Item thisItem = _db.Items.FirstOrDefault(item => item.ItemId == id);
            _db.Items.Remove(thisItem);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult AddTag(int id)
        {
            Item thisItem = _db.Items.FirstOrDefault(items => items.ItemId == id);
            ViewBag.TagId = new SelectList(_db.Tags, "TagId", "Title");
            return View(thisItem);
        }

        [HttpPost]
        public ActionResult AddTag(Item item, int tagId)
        {
            #nullable enable
            ItemTag? joinEntity = _db.ItemTags.FirstOrDefault(join => (join.TagId == tagId && join.ItemId == item.ItemId));
            #nullable disable
            if (joinEntity == null && tagId != 0)
            {
                _db.ItemTags.Add(new ItemTag() { TagId = tagId, ItemId = item.ItemId });
                _db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = item.ItemId });
        }

        [HttpPost]
        public ActionResult DeleteJoin(int joinId)
        {
            ItemTag joinEntry = _db.ItemTags.FirstOrDefault(entry => entry.ItemTagId == joinId);
            _db.ItemTags.Remove(joinEntry);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    } // Closing brace for the ItemsController class
} // Closing brace for the ToDoList.Controllers namespace
