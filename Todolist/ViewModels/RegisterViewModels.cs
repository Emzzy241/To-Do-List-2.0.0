using System.ComponentModel.DataAnnotations;

namespace ToDoList.ViewModels
{
  public class RegisterViewModel
  {
    [Required]
    [EmailAddress]
    [Display(Name = "Email Address")]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    // [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{6,}$", ErrorMessage = "Your password must contain at least six characters, a capital letter, a lowercase letter, a number, and a special character.")]
    // After I push to production, I will use this: [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{6,}$", ErrorMessage = "Your password must contain at least six characters, a capital letter, a lowercase letter, a number, and a special character.")]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }

    

    /* The reason for the ViewModels is because of the Confirm Password feature(i.e allowing users enter in their password) for the second time
        As we can see, this ViewModel looks similar to a typical Model: it's really just a grouping of properties and data annotations. Some of these data annotations may be unfamiliar, so we'll go over them now.

        The [DataType(DataType.Password)] annotation lets us specify how data should look or be formatted more precisely than a conventional C# type like string indicates on its own.
        The [Display] annotation lets us specify a different way for our property to be displayed. Looking at [Display(Name = "Confirm password")] as an example, we're specifying that if we use the property's name in our UI that it should be displayed as "Confirm Password" instead of "ConfirmPassword".
        The [EmailAddress] annotation handles validating any input associated with this property to ensure it meets the expected email address format.
        With the [Compare] annotation, we can tell our program to compare two properties and return an error if they don't match.
        Now to summarize our previous conversation, registration is a good use case for a ViewModel because we don't want to save ConfirmPassword to the ApplicationUser that we create when we register a new user. Additionally, we can add validation attributes to our ViewModel that will validate the data in the registration view, but won't interfere or change the data saved in our business model ApplicationUser or how it is saved to our database. This is a good separation between UI and business logic.
    
        From th regex session,
        Here we're using the [RegularExpression] validation attribute, which includes the regex and an error message to display if the requirements are not met.

    */
  }
}