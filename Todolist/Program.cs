using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ToDoList.Models;

namespace ToDoList
{
  class Program
  {
    public static void Main(string[] args)
    {
      WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

      builder.Services.AddControllersWithViews();

      // new line! -- We want to set the value of the ConnectionString property to the database connection string within appsettings.json; Remember that when we defined ConnectionString in the DatabaseConfig file, we made both the class and the property static i.e an instance of the class & an instance of the class' property cannot be created
      DBConfiguration.ConnectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

      WebApplication app = builder.Build();

      app.UseHttpsRedirection();

       app.UseStaticFiles();

      app.UseRouting();

      app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
      );

      app.Run();

    }
  }
}