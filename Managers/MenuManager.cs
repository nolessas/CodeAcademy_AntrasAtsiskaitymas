using newConsoleApp2.Interfaces;
using newConsoleApp2.Models;

namespace newConsoleApp2.Managers
{
    public class MenuManager : IMenuManager
    {
        private List<MenuItem> _menuItems;
        private readonly IFileHandler _fileHandler;

        // Constructor for MenuManager, takes an IFileHandler as parameter
        public MenuManager(IFileHandler fileHandler)
        {
            _fileHandler = fileHandler;
            _menuItems = new List<MenuItem>();
            LoadMenuItems();
        }


        public void LoadMenuItems()
        {
            _menuItems = _fileHandler.ReadMenuItems();
        }


        public void SaveMenuItems()
        {
            _fileHandler.WriteMenuItems(_menuItems);
        }

        public void DisplayMenu()
        {
         
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║           Restaurant Menu          ║");
            Console.WriteLine("╚════════════════════════════════════╝");

            // Filters food/drink items from _menuItems
            var foodItems = _menuItems.Where(i => i is FoodItem).ToList();
            var drinkItems = _menuItems.Where(i => i is DrinkItem).ToList();

            DisplayMenuSection(" Food ", foodItems);
            DisplayMenuSection("Drinks", drinkItems);

            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
        }


        private void DisplayMenuSection(string sectionName, List<MenuItem> items)
        {
            Console.WriteLine();
            Console.WriteLine($"═══════════════ {sectionName} ═══════════════");
            foreach (var item in items)
            {
                Console.WriteLine($"{item.Name,-30} ${item.Price,6:F2}");
            }
            Console.WriteLine();
        }
        public MenuItem? SelectItem(string itemName)
        {
            // Returns the first item that matches the name (case-insensitive), or null if not found
            return _menuItems.FirstOrDefault(item => item.Name.Equals(itemName, StringComparison.OrdinalIgnoreCase));
        }
        public void AddMenuItem(MenuItem item)
        {
            // Assigns a new ID to the item
            item.Id = _menuItems.Count > 0 ? _menuItems.Max(i => i.Id) + 1 : 1;
            _menuItems.Add(item);
            SaveMenuItems();
        }

        public void RemoveMenuItem(MenuItem item)
        {
            _menuItems.Remove(item);
            SaveMenuItems();
        }


        public List<MenuItem> GetAllMenuItems()
        {
            // Returns the _menuItems list
            return _menuItems;
        }
    }
}