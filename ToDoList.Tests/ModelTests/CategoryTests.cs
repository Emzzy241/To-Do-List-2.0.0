using Microsoft.VisualStudio.TestTools.UnitTesting;
using ToDoList.Models;
using System.Collections.Generic;
using System;

namespace ToDoList.Tests
{
    [TestClass]
    public class CategoryTests : IDisposable
    {
        public void Dispose()
        {
            // We had an Id 3 returned in the first one instead of Id; 1.. This is because This is because we assign each Category Id by running Id = _instances.Count; in the constructor. The third test is receiving a Category with an Id of 3 because sample Categorys created in previous tests remain in the static _instances list.
            // We can fix this by disposing of all Categorys between tests with a teardown method similar to the one we implemented in our Triangle tests in the last
            Category.ClearAll();
        }

        [TestMethod]
        public void CategoryConstructor_CreatesInstanceOfCategory_Category()
        {
            Category newCategory = new Category("test category");
            Assert.AreEqual(typeof(Category), newCategory.GetType());
        }

        [TestMethod]
        public void GetName_ReturnsName_String()
        {
            //Arrange
            string name = "Test Category";
            Category newCategory = new Category(name);

            //Act
            string result = newCategory.Name;

            //Assert
            Assert.AreEqual(name, result);
        }

        [TestMethod]
        public void GetId_ReturnsCategoryId_Int()
        {
            //Arrange
            string name = "Test Category";
            Category newCategory = new Category(name);

            //Act
            int result = newCategory.Id;

            //Assert
            Assert.AreEqual(1, result);
        } 

        // A test to retrieve all Category objects to display in our app
        [TestMethod]
        public void GetAll_ReturnsAllCategoryObjects_CategoryList()
        {
            //Arrange
            string name01 = "Work";
            string name02 = "School";
            Category newCategory1 = new Category(name01);
            Category newCategory2 = new Category(name02);
            List<Category> newList = new List<Category> { newCategory1, newCategory2 };

            //Act
            List<Category> result = Category.GetAll();

            //Assert
            CollectionAssert.AreEqual(newList, result);
        }

        [TestMethod]
        public void Find_ReturnsCorrectItemCategory_Category()
        {
            //Arrange
            string name01 = "Work";
            string name02 = "School";
            Category newCategory1 = new Category(name01);
            Category newCategory2 = new Category(name02);
            
            //Act
            Category foundCategory = Category.Find(2);

            //Assert
            Assert.AreEqual(newCategory2, foundCategory);
        }

        // Test 12
        // First, let's make sure we can add an Item object into the Items property of a Category object. Here's the test:
        [TestMethod]
        public void AddItem_AssociatesItemWithCategory_ItemList()
        {
            //Arrange
            string description = "Walk the dog.";
            Item newItem = new Item(description);
            List<Item> newList = new List<Item> { newItem };
            string name = "Work";
            Category newCategory = new Category(name);
            newCategory.AddItem(newItem);

            //Act
            List<Item> result = newCategory.Items;

            //Assert
            CollectionAssert.AreEqual(newList, result);
        }

        
  }
}