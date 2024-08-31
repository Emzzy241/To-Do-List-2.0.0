using System;
using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public List<Item> Items { get; set; } // For EF core, Items is a navigation property, specifically a "collection navigation property"

        // A navigation property is a property on one entity(like Category) that includes a reference to a related entity(like Item).
        // EF Core uses navigation property to recognize the relationship between Category and Item.
        // In this case, EF Core sees that the Items property has the type List<Item> which references another entity Item within our project, and because of that it is able to understand that there is a relationship between Category and Item
        // THe Items property is more specifically categorized as a collection navigation property because it contains multiple entries. In this case we have a collection (List<>) of multiple Item objects
        // Notably, navigation properties are never saved in the database, instead they are populated in our projects EF Core from the data in the database






        
    }
}