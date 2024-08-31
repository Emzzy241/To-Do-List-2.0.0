using System;
using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.Models
{
    public class ItemTag
    {
        public int ItemTagId { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set;}
        
        public int TagId { get; set; }
        public Tag Tag { get; set; }
        
        
        
        // Then we update our ToDoListContext.cs to include DbSet to represent the Tag in our database. Remeber, we are now implementing a many-to-many database relationship
        // We have different Id properties: one for ItemTag, one for Item, and one for Tag. In addition to that, we also have both Item and Tag included as navigation properties. 
        // Remmeber, a navigation property is simply a property on one entity that includes a reference to a related entity, and this is what ef corre uses to define relationships between classes. In this case, there will be one Item and one Tag in each many-to-many relationship we create.
    }
}