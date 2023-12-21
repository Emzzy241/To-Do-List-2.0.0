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
            _myList.Add(this);
            // This assigns an Item's Id to the current number of Items in the static _instances list. After the first Item is added, _instances.Count will be 1. After the second Item, it'll be 2, and so on. Using _instances.Count to assign Id ensures each is always unique. Note that we do this after adding Items to the _instances list in order to get an updated Count for Id.
            Id = _myList.Count;
        }

        public static List<Item> GetAll() 
        { 
            return _myList; 
        }
        
        public static void ClearAll() 
        { 
            _myList.Clear(); 
        }

        public static Item Find(int searchId)
        {
            return _myList[searchId - 1];
        }    
    }
}