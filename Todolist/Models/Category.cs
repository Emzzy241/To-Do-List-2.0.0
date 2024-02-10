using System.Collections.Generic;

namespace ToDoList.Models
{
  public class Category
  {
    private static List<Category> _instances = new List<Category> {};
    public string Name { get; set; } // Name will contain a name for the Category of items.
    public int Id { get; }
    public List<Item> Items { get; set; } // This is an auto for list, whose purpose is to store objects of Items inside a list
    // When we created our Category class, we included an Items property. It's a List that is empty at first; This is where Items related to the parent Category will be stored.


    // Items will contain a List of all Item objects that belong to that Category. For instance, if we had a Category with a Name of "chores," this list would contain a series of Item objects with Descriptions(Remember our Items class has a field for Description) like "mop the floor", "scrub the shower", or "do the dishes."
    // Auto-implemented property; we use get; set;.... Afield is quite different; a field is a variable but it has an access modifier and it is desclared within a class. Thanks to our auto-implemented preperty we are able to carry out encapsulation where user cannot directly alter the code everytime, we used "get; set;" to get info when user needs it and set to change the info when user wants to. We could also do this with public geter and setter methods
    // int myInt = 50; A variable
    // public int ourInt = 35; A field


    public Category(string categoryName)
    {
        Name = categoryName;
        _instances.Add(this);
        Id = _instances.Count;
        Items = new List<Item>{};
    }

    // The method below, we pass in a sample item(of the Item object type) in it, To help us store Item objects inside the Catrgory object
    public void AddItem(Item item)
    {
        // AddItem() will accept an Item object and then use the built-in List Add() method to save that item into the Items property of a specific Category.
        Items.Add(item);
    }

    public static List<Category> GetAll()
    {
        return _instances;
    }

     public static void ClearAll()
    {
        _instances.Clear();
    }

    // Since its searching through all Items and we call it on the Category class itself; we make it static. Notice we subtract 1 from the provided searchId because indexes in the _instances array begin at 0, whereas our Id properties will begin at 1. since we made use of the .Count property
    public static Category Find(int searchItemId)
    {
        return _instances[searchItemId-1];
    }

    // Aim: we want to be able to save Item objects within Category objects.
    // Now we can focus on tying together our Category and Item classes. That way, a Category object with a Name like "School" can hold many different To Do List Items with Descriptions like "Finish section 2 code review," "Email teacher about planned absence," and so on.


  }
}