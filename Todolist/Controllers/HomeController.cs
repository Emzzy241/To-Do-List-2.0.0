using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;

namespace ToDoList.Controllers
{
  public class HomeController : Controller
  {
    [HttpGet("/")]
    public ActionResult Index()
    {
      return View();
    }

    [Route("/favourite_photos")]
    public ActionResult FavouritePhotos()
    {
      return View();
    }

    

    
     

  }
}