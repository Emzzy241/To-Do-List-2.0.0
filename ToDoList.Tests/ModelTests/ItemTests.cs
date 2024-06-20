using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using ToDoList.Models;
using System.Collections.Generic;

namespace ToDoList.Tests
{
    [TestClass]
    public class ItemTests : IDisposable
    {
        public IConfiguration Configuration { get; set; }

        public ItemTests()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            DBConfiguration.ConnectionString = Configuration["ConnectionStrings:TestConnection"];
        }

        // [TestInitialize]
        // public void Setup()
        // {
        //     using (MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString))
        //     {
        //         conn.Open();
        //         MySqlCommand cmd = new MySqlCommand(@"
        //             CREATE TABLE IF NOT EXISTS items (
        //                 description TEXT,
        //                 id INT AUTO_INCREMENT PRIMARY KEY
        //             );", conn);
        //         cmd.ExecuteNonQuery();
        //     }
        // }

        // [TestCleanup]
        // public void Cleanup()
        // {
        //     Item.ClearAll();
        // }

        public void Dispose()
        {
            Item.ClearAll();
        }

        [TestMethod]
        public void GetAll_ReturnsEmptyListFromDatabase_ItemList()
        {
            // Arrange
            List<Item> newList = new List<Item> { };

            // Act
            List<Item> result = Item.GetAll();

            // Assert
            CollectionAssert.AreEqual(newList, result);
        }

        // Overriding Equals and GetHashCode
        // We want to consider a specific problem that comes up when comparing two objects.
        [TestMethod]
        public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Item()
        {
            // Arrange, Act
            Item firstItem = new Item("Mow the lawn");
            Item secondItem = new Item("Mow the lawn");

            // From the perspective of our user, firstItem and secondItm are the same thing, but our test fails with the error: Assert.AreEqual failed. Expected:<ToDoList.Models.Item>. Actual:<ToDoList.Models.Item>... Even though they have the same Description property & id
            // This is because objects created from C# classes are saved as references to actual data stored in memory. This means that when we create firstItem, this variable stores a reference to data saved in memory, but not the actual data, same as secondItem... So we are comparing their two references which are not the same
            // When we create classes or interfaces, we create a reference type. When a variable stores a reference type it means it stores the reference to the object in memory and not the actual object data
                // From the perspective our tests, we want two Item objects to be seen as the same when we instantiate two item objects(from our db) that have equal properties.

            // Assert
            Assert.AreEqual(firstItem, secondItem);
        }
        // After implementing the Equals() method, our test now pass for only description


        // [TestMethod]
        // public void ReferenceTypes_ReturnsTrueBecauseBothItemsAreSameReference_bool()
        // {
        //     // Arrange, Act
        //     Item firstItem = new Item("Mow the lawn");
        //     Item copyOfFirstItem = firstItem;
        //     copyOfFirstItem.Description = "Learn about C#";

        //     // Assert
        //     Assert.AreEqual(firstItem.Description, copyOfFirstItem.Description);
        // }.... Used to explain reference types to fully understand how I made the previous test pass

        // [TestMethod]
        // public void Save_SaveToDatabase_ItemList()
        // {
        //     // Arrange
        //     Item testItem = new Item("Mow the lawn");

        //     // Act
        //     testItem.Save();
        //     List<Item> result = Item.GetAll();
        //     List<Item> testList = new List<Item>(){testItem};

        //     // Assert
        //     CollectionAssert.AreEqual(testList, result);
        // }  


         [TestMethod]
        public void Save_SavesToDatabase_ItemList()
        {
            // Arrange
            string description01 = "Walk the Dog";
            string description02 = "Wash the dishes";

            Item newItem1 = new Item(description01);

            newItem1.Save();
            Item newItem2 = new Item(description02);
            newItem2.Save();

            // The Expected list
            List<Item> newList = new List<Item>(){newItem1, newItem2};

            // Act
            // The Actual List
            List<Item> result = Item.GetAll();

            // Assert

            CollectionAssert.AreEqual(newList, result);
        }

        // Writing test for Find()
         [TestMethod]
        public void Find_ReturnsCorrectItem_Item()
        {
            //Arrange
            Item newItem = new Item("Mow the lawn");
            newItem.Save();
            Item newItem2 = new Item("Wash the dishes");
            newItem2.Save();

            //Act
            Item foundItem = Item.Find(newItem.Id);
            
            //Assert
            Assert.AreEqual(newItem, foundItem);
        }
    
    }
}























// using Microsoft.VisualStudio.TestTools.UnitTesting;
// using Microsoft.Extensions.Configuration;
// using MySqlConnector;
// using System;
// using ToDoList.Models;
// using System.Collections.Generic;

// namespace ToDoList.Tests
// {
//     [TestClass]
//     public class ItemTests : IDisposable
//     {
//         // we've added a new property to help in setting our app's configurations
//         public IConfiguration Configuration { get; set; }

//         public void Dispose()
//         {
//             Item.ClearAll();
//         }

//         // we've added a constructor
//         public ItemTests()
//         {
//             // The ConfigurationBuilder(); Method is used to load appsettings.json with our app's configurations...It is contained in the namespace using Microsoft.Extensions.Configuration;
//             // First, We create a Configuration object that includes all of the data in our appsettings.json    
//             IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
//             Configuration = builder.Build();
//             // We set DBConfiguration.ConnectionString to the "TestConnection" database connection string, which we access through the Configuration object. This overrides the DBConfiguration.ConnectionString we set in DatabaseConfig.cs, ensuring that our tests are connected to our test database, not our development database.
//             DBConfiguration.ConnectionString = Configuration["ConnectionStrings:TestConnection"];
//         }

//         // Rewriting our Tests, since we are now working with a database
//         // 1st Test: The Simplest possible behaviour we can think of is a test to confirm whether our database is empty
//         [TestMethod]
//         public void GetAll_ReturnsEmptyListFromDatabase_ItemList()
//         {
//         //Arrange
//             List<Item> newList = new List<Item> { };

//             //Act
//             List<Item> result = Item.GetAll();

//             //Assert
//             CollectionAssert.AreEqual(newList, result);
//         }

        
        // 1st Test: Creates an instance of Item class
        /*[TestMethod]
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

       

        */


//     }
// }