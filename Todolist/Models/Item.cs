using System;
using System.Collections.Generic;

namespace ToDoList.Models
{
    public class Item
    {
        public string Description { get; set; }

        private static List<Item> _myList = new List<Item>() {};
        
        
        public Item(string myDescription)
        {
            Description = myDescription;
            _myList.Add(this);
        }

        public static List<Item> GetAll() 
        { 
            return _myList; 
        }
        
        public static void ClearAll() 
        { 
            _myList.Clear(); 
        }
        
        
    }
}