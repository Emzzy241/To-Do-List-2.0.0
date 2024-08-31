using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ToDoList.Models
{
    public class Item
    {
        public int ItemId { get; set; }
        [Required(ErrorMessage = "The item's description can't be empty!")]
        public string Description { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "You must add your item to a category. Have you created a category yet?")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public List<ItemTag> JoinEntities { get;}
        public ApplicationUser User { get; set; }  // New line!

    }
    /*
        In the last course section, we created instructions and used simple condition to avoid creating a database error if a user tries to create an Item without there being a Category to associate it with. In the Items/Create.cshtml view we added this message
        Then, in the Create() POST action in the ItemsController, we verified whether there was a Category object selected by checking if the categoryId parameter has a value of 0. If so, we simply directed to the Items/Create.cshtml page. Otherwise, we went ahead and saved the new Item to our database
        In this lesson, we're going to try out a new tool to handle this same issue: model validation with validation attributes. With model validation we configure .NET to validate user input based on rules that we set in our models; we set these rules using validation attributes. As we'll learn a bit later on, "validation attributes" are also commonly called "data annotations". Let's get into this refactor and then revisit these concepts.

        A validation attribute lets us specify the rules to use when validating a model's property. The validation attribute that we added is [Range(1, int.MaxValue)]. In order to attach a validation attribute to a property, we need to list it directly above the property, which in our case is CategoryId.
        In order to use validation attributes at all, we need to include the using directive using System.ComponentModel.DataAnnotations; at the top of the file, which allows us to use the tools within the System.ComponentModel.DataAnnotations namespace. Note that the terms data annotations and validation attributes are used interchangeably. 
        With [Range(1, int.MaxValue)], we are specifying the rule that the value of CategoryId must be a number between 1 to the int.MaxValue, which is the maximum possible integer value in C#. What this validation rule does for us is ensure that the CategoryId value is never 0. If it is 0, then we can deliver an error message to the user.
        The ErrorMessage property can be applied to any validation attribute.
        The error message "You must add your item to a category. Have you created a category yet?" will be delivered to the user if they try to create an item without specifying a category (which happens when the CategoryId has a value of 0) However, we have a few more steps to complete before this will actually work! We need to update our Item/Create.cshtml view to display the error message, and we need to update our item's Create() POST controller action to check if our model's state is valid.
    
        Each property of a model can have one or more validation attributes. Let's get more practice by adding the [Required] validation attribute to our Item.Description property. When we require a property, it means that a form input for that property can't be empty. Doing this for our items will ensure that no description input is left empty when a user creates a new item.
        Note that we could simply list [Required] without specifying an ErrorMessage.


        // The process of looking inside the migration file just verifies that the [Required] validation attribute for Item.Description not only provided model validation in our apps, but also re-configured our entity property to be non-nullable. The big takeaway is that we need to be aware of how data annotations used for model validation can also change how our entity properties are configured. When this happens, we'll want to track the new configuration with a new migration and an update to our database.
    
        
            Associating Users with Items​
            With authorization now added to our ItemsController, you need to be signed in to visit any route for items. However once we are signed in, we can see any user's items. To make our authorization more meaningful, we'll update our To Do List app so that each item that is created is associated with the logged in user, and the only items visible to a user is their own.
            
            Updating the Item Model​
            To achieve our goal, the first thing we'll do is create an association between Identity's ApplicationUser and our Item class. This will enable us to associate items with a specific user, and fetch and display only the items that belong to that user.
    */
}