using ToDoList.Models;

namespace ToDoList.Models
{
    // Also notice that the class and property are both static, meaning that the class can't have an instance, and the property also cannot be called on an instance. When we want to get or set ConnectionString, we'll do so by accessing DBConfiguration.ConnectionString.
    public static class DBConfiguration
    {
      public static string ConnectionString { get; set; }
    }
}