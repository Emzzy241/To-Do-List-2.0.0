using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ToDoList.Models;
using System.Collections.Generic;

namespace ToDoList.Tests
{
    [TestClass]
    public class MyItemTests : IDisposable
    {
        public void Dispose()
        {
            Item.ClearAll();
        }
        // 1st Test: Creates an instance of Item class
        [TestMethod]
        public void ToDoListConstructor_CreatesInstanceOfToDoListClass_ToDoList()
        {
            Item myItem = new Item("Greet Tammy");
            Assert.AreEqual(typeof(Item), myItem.GetType());
        }

        // 2nd Test: Getting a description from a auto-implemented property
        [TestMethod]
        public void GetDescription_GetsDescription_String()
        {
            // Arrange
            Item myItem = new Item("Greet Tammy");
            string expectedDescription = "Greet Tammy";

            // Act
            string returnedDescription = myItem.Description;

            // Assert
            Assert.AreEqual(expectedDescription, returnedDescription);

        }

        // 3rd Test: Setting a description for a auto-implemented property
        [TestMethod]
        public void SetDescription_SetsDescription_Void()
        {
            // Arrange
            Item myItem = new Item("Exercise");
            string changedDescription = "Drive";

            // Act
            myItem.Description = changedDescription;

            Assert.AreEqual(changedDescription, myItem.Description);
        }

        [TestMethod]
        public void GetAll_ReturnsAllItemInstances_List()
        {
        // Arrange
        Item newItem1 = new Item("Exercise");
        Item newItem2 = new Item("Wash");
        Item newItem3 = new Item("Drive");
        List<Item> expected = new List<Item> { newItem1, newItem2, newItem3 };
        // Act
        List<Item> actualResult = Item.GetAll();
        // Assert
        CollectionAssert.AreEqual(expected, actualResult);
        
        }

        [TestMethod]
        public void GetId_ItemsInstantiateWithAnIdAndGetterReturns_Int()
        {
            //Arrange
            string description = "Walk the dog.";
            Item newItem = new Item(description);

            //Act
            int result = newItem.Id;

            //Assert
            Assert.AreEqual(1, result);
        }

        [TestMethod]
        public void Find_ReturnsCorrectItem_Item()
        {
            //Arrange
            string description01 = "Walk the dog";
            string description02 = "Wash the dishes";
            Item newItem1 = new Item(description01);
            Item newItem2 = new Item(description02);

            //Act
            Item result = Item.Find(2);
            
            //Assert
            Assert.AreEqual(newItem2, result);
        }
    }
}