using System;
using System.Collections.Generic;

namespace ToDoList.Models
{
    public class Item
    {
        public string Description { get; set; }

        // We don't add a set method, because this property will be set in the constructor automatically. In fact, we specifically don't ever want to manually edit it. That would increase the risk of IDs not being unique
        public int Id { get; }

        private static List<Item> _myList = new List<Item>() {};
        
        
        public Item(string myDescription)
        {
            Description = myDescription;
            //  Remember that when we are working inside an object, we can use the keyword this to reference that object. In the code above, we use this to reference the Item being actively constructed by the constructor.
            _myList.Add(this);
            // This assigns an Item's Id to the current number of Items in the static _instances list. After the first Item is added, _instances.Count will be 1. After the second Item, it'll be 2, and so on. Using _instances.Count to assign Id ensures each is always unique. Note that we do this after adding Items to the _instances list in order to get an updated Count for Id.
            Id = _myList.Count; //creating a readonly property; a property that can be read but not overwritten
            // We don't add a set method, because this property will be set in the constructor automatically. In fact, we specifically don't ever want to manually edit it. That would increase the risk of IDs not being unique. This is called creating a readonly property. In other words, it's a property that can be read but not overwritten.
        }

        public static List<Item> GetAll() 
        { 
            return _myList; 
        }
        
        public static void ClearAll() 
        { 
            _myList.Clear(); 
        }

        //  It's static because it must sift through all Items to find the one we're seeking.
        // Also, notice we subtract 1 from the provided searchId because indexes in the _instances array begin at 0, whereas our Id properties will begin at 1. since we made use of the .Count property
        public static Item Find(int searchId)
        {
            return _myList[searchId - 1];
        }    
    }
}