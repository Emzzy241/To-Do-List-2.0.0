using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ToDoList.Models;
using System.Threading.Tasks;
using ToDoList.ViewModels;

namespace ToDoList.Controllers
{
  public class AccountController : Controller
  {
    private readonly ToDoListContext _db;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController (UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ToDoListContext db)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _db = db;
    }

    public ActionResult Index()
    {
      return View();
    }

    public IActionResult Register()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Register (RegisterViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      else
      {
        ApplicationUser user = new ApplicationUser { UserName = model.Email };
        IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
          return RedirectToAction("Index");
        }
        else
        {
          foreach (IdentityError error in result.Errors)
          {
            ModelState.AddModelError("", error.Description);
          }
          return View(model);
        }
      }
    }

    public ActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View(model);
      }
      else
      {
        Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
        if (result.Succeeded)
        {
          return RedirectToAction("Index");
        }
        else
        {
          ModelState.AddModelError("", "There is something wrong with your email or username. Please try again.");
          return View(model);
        }
      }
    }

    [HttpPost]
    public async Task<ActionResult> LogOff()
    {
    await _signInManager.SignOutAsync();
    return RedirectToAction("Index");
    }


    /*
        In addition to adding a using directive for Microsoft.AspNetCore.Identity, we also add one for System.Threading.Tasks. This will allow us to use asynchronous Tasks so we can use async and await to register new users.

        We also include a using directive for ToDoList.ViewModels. The ViewModels namespace will include view models that we haven't created yet, like the RegisterViewModel. Note that we haven't covered this yet, and we will in the very next lesson.
        We have private, readonly fields for _userManager and _signInManager. The value for each of these will be set to Identity's UserManager<TUser> and SignInManager<TUser> classes respectively. Both of these classes contains methods that we'll use to create users and sign them in. For registration, we'll use methods from the UserManager<TUser> class. For signing in and out, we'll use methods from the SignInManager<TUser> class.
        Note that the values for the _userManager and _signInManager fields are set in the AccountController class constructor using dependency injection to access the services we set up in Program.cs. Let's review dependency injection now.
        Dependency injection is the act of providing a helpful tool (known as a service) to part of an application that needs it before it actually needs it. This ensures that the application doesn't need to worry about locating, loading, finding, or creating that service on its own.
        ASP.NET Core uses dependency injection to make available all services set up in Program.cs to controllers and views. We've already used ASP.NET Core's system of dependency injection to set up our access to our database via the _db field.
        Right now, we're accessing Identity's UserManager and SignInManager services, which have been injected into the AccountController constructor so that our controller will have access to these services as needed.
        This follows what is known as the "explicit dependencies principle," which states that methods and classes should explicitly require any dependencies. This makes the code much easier to read and understand and also ensures that our code will function correctly.
        Dependency injection can be a tricky concept to grasp simply because it happens implicitly. It's functionality that's built into the ASP.NET Core framework, which makes it become a bit like black box magic. Well, know that when you are just starting out as a developer it's not important to have a deep understanding of dependency injection. If you'd like to learn more about how ASP.NET Core uses dependency injection, we recommend beginning with the ASP.NET Core documentation on Dependency Injection.

        // Looking at the Register() now,
        The method's signature: This method is an async Task because creating user accounts will be an asynchronous action. Our Register() action doesn't return an ActionResult. Instead, it returns a Task containing an ActionResult. Remember, the built-in Task<TResult> class represents asynchronous actions
        This method is an async Task because creating user accounts will be an asynchronous action. Our Register() action doesn't return an ActionResult. Instead, it returns a Task containing an ActionResult. Remember, the built-in Task<TResult> class represents asynchronous actions

        RegisterViewModel represent the user's data when registering for a new account
        Within the body of the Register() POST action method, we first check if our model is valid: if it is (meaning that the registration form has been filled out correctly), then we continue with the registration process, and if not, we return to the registration view. Whenever we see ModelState.IsValid, it means that we're using validation attributes in our register's model.
        The first thing we do if our model state is valid is create a new ApplicationUser with the Email from the form submission as its UserName: ApplicationUser user = new ApplicationUser { UserName = model.Email };
        Then, we invoke an async method: IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        Remember that _userManager represents Identity's UserManager<TUser> class, which was injected as a service into the AccountController. The UserManager<TUser> class has a method called CreateAsync(). As explained in the documentation, this method will create a user with the provided password.
        Our async method will return a new IdentityResult object which we call result. The IdentityResult class simply represents the result of an Identity-driven action, like whether it's successful or not.
        We use await because CreateAsync() is an asynchronous action, which means our application needs to wait until CreateAsync() successfully returns an IdentityResult before we actually define result, and before we can move on to process other code.
        Note that CreateAsync() takes two arguments:
        An ApplicationUser with user information;
        A password that will be encrypted when the user is added to the database.
        The next thing we do is check whether or not our user creation succeeded.

        The IdentityResult object contains a Succeeded property that contains a bool. After creating the new user, we check the result.Succeeded property in an if statement to determine what to do next: if CreateAsync() is successful, the controller redirects to Index; if the user creation fails, then the Register view is returned with an error message about why the user could not be created.
        The IdentityResult object contains an Errors property that is of the type IEnnumerable<IdentityError>. That means it's an iterable collection of IdentityError objects. Each error IdentityError object itself has a Description property that contains a string description of the Identity error that occurred. We use this description to create a new model error to add to our model's state with the following line: ModelState.AddModelError("", error.Description);
        It may seem surprising to create model errors in the controller, but this is a great way to use ASP.NET Core's built-in tools around model validation to deliver Identity-related errors.
        The AddModelError() method takes two arguments: the first is a key for the error so that we can access it in our view, and the second argument is the description of the error that will be displayed. We've listed an empty string "" for our error keys. Why so? We don't need unique keys for these errors because we'll be displaying all of them at once in a list using the HTML.ValidationSummary() method. We'll get into exactly what that means soon
        After we create a model error for each Identity error and attach it to our model's state, our job is simply to return our model to the view: return View(model);
        This code will re-display the Register() GET action with our same model that has the error messages associated with it. If we did not pass in our model variable to the view, the Register() GET action would display again, but it would have no conception of any errors — it would be a brand new model, just like hitting the refresh button.
    
        // For the signin feature:
        We'll focus on the Login() POST method which once again uses an asynchronous method. Note that there are several similarities with our Register() POST method:

        Both methods are async and return a Task<ActionResult>.
        Both take a ViewModel as an argument.
        Both methods check if the model is valid, and if not return to the view to inform the user of their errors.
        Both use an Identity method ending with Async. All async Identity methods have Async appended to them.
        Both methods have a result that must await the completion of an Identity method.
        Now let's take a closer look at the following line:

        Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: true, lockoutOnFailure: false);
        Remember that we've injected a SignInManager service, which is being referenced in the signInManager variable. The SignInManager class includes the PasswordSignInAsync() method, which has a self-explanatory name: it's an async method that allows users to sign in with a password.
        PasswordSignInAsync() takes four parameters: userName, password, isPersistent, and lockoutOnFailure. For now we're only handling username and password, so we set explicit boolean values for isPersistent and lockoutOnFailure.
        However, just like in our Register() action, we want to ensure our application doesn't freeze or break if Identity can't successfully authenticate an account. That's why we add an if statement based on whether the result has succeeded or not. The Microsoft.AspNetCore.Identity.SignInResult object has a Succeeded boolean property to help with this. If our sign in action has succeeded, then we'll be redirected to our accounts index page.
        If our sign in action has failed, well, there's no SignInResult.Errors property to use to create error messages for our user. There are individual properties with information about different signin states, which you can review in the [SignInResult] docs.
        So what should we do? This is really a trick question: we don't actually want to give specific error messages to the user about why a sign in attempt has failed. That's because specific error messages could be delivered to malicious users that improve their ability to break into an account. For example, it's better to state "there was an issue with your username or password" than to state the more specific "that password does not match our records". So this is exactly what we do in case of a signin failure

        Next, let's give the user the ability to log out. Instead of creating a separate GET and POST controller action and view, we'll create just a POST controller action and add a form directly to our account's index page.
        Add a LogOff() action to the controller.

        This method is straightforward. SignInManager has the asynchronous method SignOutAsync() that signs the user out. Everything else in this method should look familiar at this point.

        Note that we don't need to add any error handling or model validation here, since a user should only be able to log out if they're already logged in.
    */

  }
}