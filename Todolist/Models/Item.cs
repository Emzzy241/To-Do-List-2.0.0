
using MySqlConnector;
using System;
using System.Collections.Generic;
using ToDoList.Models;


namespace ToDoList.Models
{
    
public class Item
{
    public int Id { get; set; }
    public string Description { get; set; }

    // Constructor
    public Item(string description)
    {
        Description = description;
    }

    // Currently, our constructor only accepts description as an argument. Whenever we create a new object in our application, it should only have a description. However, when we retrieve a record from the database, we want its id, too. We can add an overloaded constructor so our application can instantiate an Item either way:
    public Item(string description, int id)
    {
        Description = description;
        Id = id;

        // This overloaded constructor is what makes the code: Item newItem = new Item(itemDescription, itemId);... execute and not give any error
    }

     // Ensuring two objects returned from db are the same once their properties are equal.
//         // Best practice dictates this method to be below constructors and properties but above the remaining methods
        public override bool Equals(System.Object otherItem)
        {
            // The method equals is built into C#, its included in a set of default behaviours all objects have, to override it and add new capabilities to it, we use the override keyword
            // Because Equals() accepts any type of object, we used System.Object not Item
            // Conditional checks if the argument passed into the parameter is in fact an Item object, if it isn't our method returns false. At the very least, we know that we want objects to be of the same type 
            // Next, in else, we use type casting to ensure that otherItem is in fact an item. With type casting we turn System.Object otherItem into Item otherItem. Ensure you include that line even though it is similar to the previous one.. If you don't you get an error
            // We can now compare the Description of this(the item our method will be called on) to the Description of the newItem. If they have the same value for description, our application should consider both Items as the same 
            // Updating the method based on id's 
            if (!(otherItem is Item))
            {
                return false;
            }
            else
            {
                Item newItem = (Item) otherItem;
                bool idEquality = (this.Id == newItem.Id); // Updating for id's
                bool descriptionEquality = (this.Description == newItem.Description);
                return (idEquality && descriptionEquality);
            }
        }
        // We also need to override for GetHashcode() method if we want the Equals to method to override for dictionaries
        // Dictionaries and hash tables are hash-based collections, which give each hash entry a unique identifier. The point of entr having a unique identifier is to make searching and finding an entry fast and efficient. Also when comparing 2 objects and they both have the same hash code, they are considered the same object
        // Use the GetHashCode() to get the hash code of an entry. The method uses a hashing function to get hash code(an integer). A hashing function takes an input, changes it and returns a unique output.
        // When we override the GetHashCode() method, we are telling C# compiler how the hash code should be determined for the object that is called on. There are many ways to do this, let us use the simplest way
        public override int GetHashCode()
        {
            return Id.GetHashCode();
            // We telling the compiler to generate a hash code for an Item based on the Id property.Since the id property is unique and changeth not, this is a good value to generate has code.
            // We can use any property inplace of the id but we need to be careful. Choosing a value like Description that may not be unique accross objects, this could generate "collisions" where multiple entries have the same hash code.
            // We won't be making use of the GetHashCode() method because we will not be using custom objects(like Item) as the key of a dictionary entry. Which is when the overriden GetHashCode() method is required
        }

    public void Save()
    {
        // Each time we make a query, we need to open a new database connection
           MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
             cmd.CommandText = "INSERT INTO items (description) VALUES (@ItemDescription);";  
            MySqlParameter param = new MySqlParameter();
            param.ParameterName = "@ItemDescription";
            param.Value = this.Description;
            cmd.Parameters.Add(param);
            cmd.ExecuteNonQuery();
            Id = (int) cmd.LastInsertedId;
            // set Item's id property equal to the value of the id of the new row in our database. Fortunately for us, MySqlCommand object has a LastInsertedId property which we can assign to the Item's id property. This ensures that the ID property for an item is the same  both in our applicatio and inour database.
            // Stumbled uupon another error; can't convert long to int, reason for the error: MySqlCommand property called LastInsertedId returns a value of the type long; A long element is a piece of 64-bit data while an int is 32 bit... To solve we explicitly casted the long back to an int
            // Remember explicit casting causes loss of data but in our case, we are certain that there wont be any loss of data of any form. because there are no floating point numbers in it.
            // We could have simply turned our id in int to long by reducing the size and the length won't exceed 32-bits... Lets just use explicit casting
    }

        public static List<Item> GetAll()
        {
            List<Item> allItems = new List<Item> { };

            MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString); //Opening a Database Connection
            conn.Open(); // Open() the connection. Our application will throw an exception if we try to make a SQL query without first opening a database connection.

            // Construct a SQL Query; Once our connection is open, we can construct our SQL query:
            // When we make a SQL query in our application, it's not just a string of text. The query needs to be stored in a special object called a MySqlCommand.

            // In order to do this, we call the createCommand() method on our conn object. We include the expression as MySqlCommand at the end of this line. Using as creates an expression that casts cmd into a MySqlCommand object.

            // This casting is important because there are many different types of SQL databases and many different types of objects that can interact with them. Because our connection is a MySqlConnection type object, we cast it to send a corresponding MySqlCommand to the database.
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand; 
            // Next, we'll add the actual text of our SQL command. Remember that cmd is a MySqlCommand object. A MySQLCommand object has a number of different properties we can set. We won't cover most of them, but the CommandText property is essential because it's where we'll store our actual SQL query.
            cmd.CommandText = "SELECT * FROM items;"; // just like you did on bash

            // Returning Results from the Database: Next, we need to create a Data Reader Object. It will be responsible for reading the data returned by our database in response to the "SELECT * FROM items;" command:
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            // In the above, we also We'll cast its type for use with MySQL just like we did with conn. We call this Data Reader rdr and use the as keyword to cast it into a MySqlDataReader object.
            
            // The rdr object represents the actual reading of the database. However, we will need to call other methods on the rdr object in order to display the results of the query in our application:
            // A MySqlDataReader object has a built-in Read() method that reads results from the database one at a time and then advances to the next record. This method returns a boolean. If the method advances to the next object in the database, it returns true. If it reaches the end of the records that our query has returned, it returns false and our while loop ends.
            while (rdr.Read())
            {
                // In the while loop, we'll take each individual record from our database and translate that record into an Item object our application understands:
                /*
                    Our MySQLDataReader rdr object has many methods available to it. Many of these methods are specifically for extracting data from a record. GetInt32() returns a 32 bit integer. GetString() is self-explanatory.

                    We also pass in a number value as an argument to both methods. This is because rows from the database are returned by the rdr.Read() method as indexed arrays. Let's use the following table as an example to demonstrate:

                    id | description ---+--------- 1  | Mow the lawn 2  | Walk the dog 3  | Make dinner
                    

                    When the reader object returns the first entry in this example database, it'll look like this:

                    { 1, "Mow the lawn" };
                    

                    The second object will look like this:

                    { 2, "Walk the dog" };
                    

                    The id column is at index 0 while the description column is at index 1. If we had a third column, it'd be at index 2.

                    In our while loop above, we define our itemId as rdr.GetInt32(0); because this will return the integer at the 0th index of the array returned by the reader. Similarly, we define itemDescription as rdr.GetString(1) because our item description will be at the 1st index of the array returned by the reader.

                    Once we've collected the data, we can use it to instantiate new Item objects and add them to our allItems list. Now each row in our database is an Item stored in a List that our application understands.
                */
                int itemId = rdr.GetInt32(0);
                string itemDescription = rdr.GetString(1);
                Item newItem = new Item(itemDescription, itemId);
                allItems.Add(newItem);
            }
            // Closing the Connection​: Communicating with a database is a resource-intensive process. For this reason, it's important to close our database connection when we're done. This allows the database to reallocate resources to respond to requests from other users. We can use a built-in Close() method to do this.
            conn.Close();
            //The Close() method is self-explanatory. We also include a conditional because on rare occasions, our database connection will fail to close properly. It's considered best practice to confirm it's fully closed. That's why we put the Dispose() method inside a conditional. This method will only run if conn is not null.
            if (conn != null)
            {
                conn.Dispose();
            }
            return allItems;
        }

        public static void ClearAll() 
        { 
            // We call new MySqlConnection(DBConfiguration.ConnectionString); to create our conn object and then call Open() on it to open the connection. Remember that DBConfiguration.ConnectionString is originally defined in DatabaseConfig.cs.

             MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString);

            // Next, we'll create a new MySqlCommand object. It will include a SQL command to delete all rows from our items database table
            // MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString);

            conn.Open();



            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

            cmd.CommandText = "DELETE FROM items;";
            cmd.ExecuteNonQuery();
            // SQL statements that modify data instead of querying and returning data are executed with the ExecuteNonQuery() method, as seen here. Ultimately, there are two ways we interact with databases: we can either modify or retrieve data.
            // Modifying data can include adding, deleting, or updating entries. On the other hand, retrieving data includes locating and returning entries. When we save data, we're not searching for specific data. Instead, we're modifying the database by adding a new entry.
            // When we execute commands that modify the database, we use the ExecuteNonQuery() method. Commands that retrieve data use different methods like ExecuteReader(), which we used in our GetAll() method.

            // Finally, we close the connection
            conn.Close();

            if (conn != null)
            {
                // Our conditional confirms it's been successfully closed and destroys it if it's not.
                conn.Dispose();
            }

        }

         public static Item Find(int searchId)
        {
            // Opening up a connection to database
            MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString);
            conn.Open();

            // We create a MySqlCommand object and add a query to its CommandText property
            // We always need to do this to make a SQl Query
            MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = "SELECT * FROM items WHERE id = @ThisId";
            // We have to use parameter placeholders @ThisId and a 'MYSqlParameter' object  to prevent SQL injection attacks.
            // This is only necessary when we are passing parameters into a query
            // We also did this for our Save() method
           MySqlParameter param = new MySqlParameter();
           param.ParameterName = "@ThisId";
           param.Value = "searchId";
           cmd.Parameters.Add(param);

            //    We use the ExecuteReader() method because our query will be returning results and we need this method to read these results.
            // This is in contrast to the ExecuteNonQuery() method,( remember that one doesn't read but only helps to save), which we use for SQL commands that don't return results like our Save() method.
            MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
            int itemId = 0;
            string itemDescription = "";
            while (rdr.Read())
            {
                itemId = rdr.GetInt32(0);
                itemDescription = rdr.GetString(1);

            }
            Item foundItem = new Item(itemDescription, itemId);

            // We close the connection and confirm that the connection is closed
            conn.Close();

            if (conn != null)
            {
                conn.Dispose();
            }
            return foundItem;
        }   


}

       

}



// using MySqlConnector;
// using System;
// using System.Collections.Generic;

// namespace ToDoList.Models
// {
//     public class Item
//     {
//         public string Description { get; set; }

//         // We don't add a set method, because this property will be set in the constructor automatically. In fact, we specifically don't ever want to manually edit it. That would increase the risk of IDs not being unique
//         public int Id { get; }

//         // Removing the _instances(the list we used in storing items), we will now be using a database
//         // private static List<Item> _myList = new List<Item>() {};
        
        
//         public Item(string myDescription, int id)
//         {
//             Description = myDescription;
//             // Overloading our constructor: Currently, our constructor only accepts description as an argument. Whenever we create a new object in our application, it should only have a description. However, when we retrieve a record from the database, we want its id, too. We can add an overloaded constructor so our application can instantiate an Item either way:
//               Id = id;
//         }

//         // Ensuring two objects returned from db are the same once their properties are equal.
//         // Best practice dictates this method to be below constructors and properties but above the remaining methods
//         public override bool Equals(System.Object otherItem)
//         {
//             // The method equals is built into C#, its included in a set of default behaviours all objects have, to override it and add new capabilities to it, we use the override keyword
//             // Because Equals() accepts any type of object, we used System.Object not Item
//             // Conditional checks if the argument passed into the parameter is in fact an Item object, if it isn't our method returns false. At the very least, we know that we want objects to be of the same type 
//             // Next, in else, we use type casting to ensure that otherItem is in fact an item. With type casting we turn System.Object otherItem into Item otherItem. Ensure you include that line even though it is similar to the previous one.. If you don't you get an error
//             // We can now compare the Description of this(the item our method will be called on) to the Description of the newItem. If they have the same value for description, our application should consider both Items as the same 
//             // Updating the method based on id's 
//             if (!(otherItem is Item))
//             {
//                 return false;
//             }
//             else
//             {
//                 Item newItem = (Item) otherItem;
//                 bool descriptionEquality = (this.Description == newItem.Description);
//                 return descriptionEquality;
//             }
//         }
//         // We also need to override for GetHashcode() method if we want the Equals to method to override for dictionaries
//         // Dictionaries and hash tables are hash-based collections, which give each hash entry a unique identifier. The point of entr having a unique identifier is to make searching and finding an entry fast and efficient. Also when comparing 2 objects and they both have the same hash code, they are considered the same object
//         // Use the GetHashCode() to get the hash code of an entry. The method uses a hashing function to get hash code(an integer). A hashing function takes an input, changes it and returns a unique output.
//         // When we override the GetHashCode() method, we are telling C# compiler how the hash code should be determined for the object that is called on. There are many ways to do this, let us use the simplest way
//         public override int GetHashCode()
//         {
//             return Id.GetHashCode();
//             // We telling the compiler to generate a hash code for an Item based on the Id property.Since the id property is unique and changeth not, this is a good value to generate has code.
//             // We can use any property inplace of the id but we need to be careful. Choosing a value like Description that may not be unique accross objects, this could generate "collisions" where multiple entries have the same hash code.
//             // We won't be making use of the GetHashCode() method because we will not be using custom objects(like Item) as the key of a dictionary entry. Which is when the overriden GetHashCode() method is required
//         }

//         public void Save()
//         {
//             // A method for saving Items to the database.
//             MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString);
//             conn.Open();

//             MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

//             // begin new code
//             cmd.CommandText = "INSERT INTO items (description) VALUES (@ItemDescription);";  
//             MySqlParameter param = new MySqlParameter();
//             param.ParameterName = "@ItemDescription";
//             param.Value = this.Description;
//             cmd.Parameters.Add(param);
//             cmd.ExecuteNonQuery();
//             // Id = cmd.LastInsertedId;
//             // end new code
//             // Explaining the section begin new code and end new code
//             //  we pass in INSERT SQL command into cmd.CommandText, we also pass in a parameter placeholder '@ItemDescription' into the SQL statement. We want to use parameter placeholders whenever we are passing along data that a user enters. Information stored to a paramter is trated as a field data and not part of the SQL stmt, which helps to protect our application from a malicious attack called SQL Injection.
//             // The placeholder @ItemDescription will be replaced with the actual data from the user when the MySql Command executes. Parameters placeholders need the '@' symbol prefixing their name
//             // Subsequent lines, We create a MySqlParameter object for each parameter required in our MySqlCommand. The parameter name must match the parameter in the command string.The Value is what will replace the parameter in the command string when it is executed.
//             // We define parameterName property of param as @ItemDescription, matching the parameter used in our SQL command "INSERT INTO items (description) VALUES (@ItemDescription);" exactly
//             // We define th evalue property of param as this.Description This refers to the auto-implemented Description property of the Item we're saving.
//             // We Pass the param into MySqlCommand's parameters property using Add(). If we had more parameters to use we would need to add each one
//             // This may seem confusu=ing, but what we are essentially doing is using an object to say the @ItemDescription in our cmd.CommandText equals this.Description. 
//             // There are simpler ways to do this, e.g:the 4 lines from param to Add(param) can be replaced with this single line: cmd.Parameters.AddWithValue("@ItemDescription", this.Description);
//             // Finally, we call the ExecuteNonQuery() on our cmd object to execute the SQL command.

//             conn.Close();
//             if (conn != null)
//             {
//                 conn.Dispose();
//             }
//         }

      
//         // Our New GetAll() method
//          public static List<Item> GetAll()
//         {
//             List<Item> allItems = new List<Item> { };

//             MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString); //Opening a Database Connection
//             conn.Open(); // Open() the connection. Our application will throw an exception if we try to make a SQL query without first opening a database connection.

//             // Construct a SQL Query; Once our connection is open, we can construct our SQL query:
//             // When we make a SQL query in our application, it's not just a string of text. The query needs to be stored in a special object called a MySqlCommand.

//             // In order to do this, we call the createCommand() method on our conn object. We include the expression as MySqlCommand at the end of this line. Using as creates an expression that casts cmd into a MySqlCommand object.

//             // This casting is important because there are many different types of SQL databases and many different types of objects that can interact with them. Because our connection is a MySqlConnection type object, we cast it to send a corresponding MySqlCommand to the database.
//             MySqlCommand cmd = conn.CreateCommand() as MySqlCommand; 
//             // Next, we'll add the actual text of our SQL command. Remember that cmd is a MySqlCommand object. A MySQLCommand object has a number of different properties we can set. We won't cover most of them, but the CommandText property is essential because it's where we'll store our actual SQL query.
//             cmd.CommandText = "SELECT * FROM items;"; // just like you did on bash

//             // Returning Results from the Database: Next, we need to create a Data Reader Object. It will be responsible for reading the data returned by our database in response to the "SELECT * FROM items;" command:
//             MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
//             // In the above, we also We'll cast its type for use with MySQL just like we did with conn. We call this Data Reader rdr and use the as keyword to cast it into a MySqlDataReader object.
            
//             // The rdr object represents the actual reading of the database. However, we will need to call other methods on the rdr object in order to display the results of the query in our application:
//             // A MySqlDataReader object has a built-in Read() method that reads results from the database one at a time and then advances to the next record. This method returns a boolean. If the method advances to the next object in the database, it returns true. If it reaches the end of the records that our query has returned, it returns false and our while loop ends.
//             while (rdr.Read())
//             {
//                 // In the while loop, we'll take each individual record from our database and translate that record into an Item object our application understands:
//                 /*
//                     Our MySQLDataReader rdr object has many methods available to it. Many of these methods are specifically for extracting data from a record. GetInt32() returns a 32 bit integer. GetString() is self-explanatory.

//                     We also pass in a number value as an argument to both methods. This is because rows from the database are returned by the rdr.Read() method as indexed arrays. Let's use the following table as an example to demonstrate:

//                     id | description ---+--------- 1  | Mow the lawn 2  | Walk the dog 3  | Make dinner
                    

//                     When the reader object returns the first entry in this example database, it'll look like this:

//                     { 1, "Mow the lawn" };
                    

//                     The second object will look like this:

//                     { 2, "Walk the dog" };
                    

//                     The id column is at index 0 while the description column is at index 1. If we had a third column, it'd be at index 2.

//                     In our while loop above, we define our itemId as rdr.GetInt32(0); because this will return the integer at the 0th index of the array returned by the reader. Similarly, we define itemDescription as rdr.GetString(1) because our item description will be at the 1st index of the array returned by the reader.

//                     Once we've collected the data, we can use it to instantiate new Item objects and add them to our allItems list. Now each row in our database is an Item stored in a List that our application understands.
//                 */
//                 int itemId = rdr.GetInt32(0);
//                 string itemDescription = rdr.GetString(1);
//                 Item newItem = new Item(itemDescription, itemId);
//                 allItems.Add(newItem);
//             }
//             // Closing the Connection​: Communicating with a database is a resource-intensive process. For this reason, it's important to close our database connection when we're done. This allows the database to reallocate resources to respond to requests from other users. We can use a built-in Close() method to do this.
//             conn.Close();
//             //The Close() method is self-explanatory. We also include a conditional because on rare occasions, our database connection will fail to close properly. It's considered best practice to confirm it's fully closed. That's why we put the Dispose() method inside a conditional. This method will only run if conn is not null.
//             if (conn != null)
//             {
//                 conn.Dispose();
//             }
//             return allItems;
//         }
        
//         public static void ClearAll() 
//         { 
//             // We call new MySqlConnection(DBConfiguration.ConnectionString); to create our conn object and then call Open() on it to open the connection. Remember that DBConfiguration.ConnectionString is originally defined in DatabaseConfig.cs.

//              MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString);

//             // Next, we'll create a new MySqlCommand object. It will include a SQL command to delete all rows from our items database table
//             // MySqlConnection conn = new MySqlConnection(DBConfiguration.ConnectionString);

//             conn.Open();



//             MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;

//             cmd.CommandText = "DELETE FROM items;";
//             cmd.ExecuteNonQuery();
//             // SQL statements that modify data instead of querying and returning data are executed with the ExecuteNonQuery() method, as seen here. Ultimately, there are two ways we interact with databases: we can either modify or retrieve data.
//             // Modifying data can include adding, deleting, or updating entries. On the other hand, retrieving data includes locating and returning entries. When we save data, we're not searching for specific data. Instead, we're modifying the database by adding a new entry.
//             // When we execute commands that modify the database, we use the ExecuteNonQuery() method. Commands that retrieve data use different methods like ExecuteReader(), which we used in our GetAll() method.

//             // Finally, we close the connection
//             conn.Close();

//             if (conn != null)
//             {
//                 // Our conditional confirms it's been successfully closed and destroys it if it's not.
//                 conn.Dispose();
//             }

//         }

//         //  It's static because it must sift through all Items to find the one we're seeking.
//         // Also, notice we subtract 1 from the provided searchId because indexes in the _instances array begin at 0, whereas our Id properties will begin at 1. since we made use of the .Count property
          
        
//     }
// }
    
// // }