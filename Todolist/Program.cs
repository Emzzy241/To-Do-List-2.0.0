using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ToDoList
{
  class Program
  {
    public static void Main(string[] args)
    {
      WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

      builder.Services.AddControllersWithViews();

      // new line!
      // DBConfiguration.ConnectionString = builder.Configuration["ConnectionStrings:DefaultConnection"];

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