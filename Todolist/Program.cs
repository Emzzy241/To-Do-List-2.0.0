using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToDoList.Models;
using Microsoft.AspNetCore.Identity;

namespace ToDoList
{
  class Program
  {
    static void Main(string[] args)
    {

      WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

      builder.Services.AddControllersWithViews();

      builder.Services.AddDbContext<ToDoListContext>(
                        dbContextOptions => dbContextOptions
                          .UseMySql(
                            builder.Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(builder.Configuration["ConnectionStrings:DefaultConnection"]
                          )
                        )
                      );
      
      /* New code below!!
        We set up Identity as a service with the line builder.Services.AddIdentity<ApplicationUser, IdentityRole>(). Notice that we specify <ApplicationUser, IdentityRole> — these are the two models that we're using to designate the user and the role. Just like IdentityUser, IdentityRole is a built-in class to Identity, and it allows us to use the default configurations for roles. We won't be configuring roles beyond the defaults, so we use the built-in IdentityRole class here.
        We chain two more method calls to the Identity service that we set up: .AddEntityFrameworkStores<ToDoListContext>() and .AddDefaultTokenProviders();. The first method ensures that the Identity user data is saved via EF Core to our database (as represented by the ToDoListContext class). The second method sets up Identity's providers for tokens, which are created during password reset or two factor authentication, for example. Note that we won't go over how to implement either of those two things in the coursework, and you are encouraged to look into them on your own.
        Finally, we configure our web application app to .UseAuthentication() and .UseAuthorization(). Remember two things here:

        Whenever we call a method on our WebApplication app, we are configuring how our application handles HTTP requests (the "pipeline"). We configure the request pipeline by setting up middleware. Middleware is software that we add to our request pipeline that determines how the request should be processed. Each middleware decides whether to do some work, or to pass the request onto the next middleware. To optionally review more about this topic, visit the MS Docs.
        The order in which we set up the middleware matters! If these methods are called in the wrong order, you may run into unhandled exceptions or issues logging in with Identity. Fortunately, the Microsoft Docs has a list of how middleware should be ordered.
      */

      // override Identity's default settings by configuring our Identity service

      builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ToDoListContext>()
                .AddDefaultTokenProviders();

        // Default Password settings.
      /* builder.Services.Configure<IdentityOptions>(options =>
      {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;

      */

      // });
      
        // Changing the default settings only for development mode... Do not do it for production mode
        builder.Services.Configure<IdentityOptions>(options =>
        {
          options.Password.RequireDigit = false;
          options.Password.RequireLowercase = false;
          options.Password.RequireNonAlphanumeric = false;
          options.Password.RequireUppercase = false;
          options.Password.RequiredLength = 0;
          options.Password.RequiredUniqueChars = 0;

          /*
            The configuration above allows us to input a password of a single character to create a new user. Even though the RequiredLength property is 0, we can't actually put in an empty password because we have a validation attribute in place that states that some input for the RegisterViewModel.Password property is required.
            Keep in mind that the above settings should never be used in a production environment — only during development to make our lives a bit easier.
            Finally, note that when we change our password requirements in Program.cs, we need to make a corresponding update to our [RegularExpression] validation attribute for the RegisterViewModel.Password property.
          */
        });

      WebApplication app = builder.Build();

      // app.UseDeveloperExceptionPage();
      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      // New code below!
      app.UseAuthentication(); 
      app.UseAuthorization();

      app.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}"
        );

      app.Run();
    }
  }
}