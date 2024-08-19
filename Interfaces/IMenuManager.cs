using newConsoleApp2.Models;

namespace newConsoleApp2.Interfaces
{
    public interface IMenuManager
    {
        void LoadMenuItems();
        void SaveMenuItems();
        void DisplayMenu();
        MenuItem? SelectItem(string itemName);
        void AddMenuItem(MenuItem item);
        void RemoveMenuItem(MenuItem item);
        List<MenuItem> GetAllMenuItems();
    }
}
