namespace newConsoleApp2.Core
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Singleton pattern: Get the single instance of Restaurant
                var restaurant = Restaurant.Instance;
                // Start the main program loop
                restaurant.Run();
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Error during initialization", ex);
            }
        }
    }
}