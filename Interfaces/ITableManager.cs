using newConsoleApp2.Models;

namespace newConsoleApp2.Interfaces
{
    public interface ITableManager
    {
        void AddTable(Table table);
        Table? GetTable(int tableNumber);
        void DisplayTableStatus();
        IEnumerable<Table> GetAllTables();
        void LoadPastOrders();
        Table? FindSuitableTable(int numberOfPeople);
    }
}
