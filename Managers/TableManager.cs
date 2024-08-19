using newConsoleApp2.Interfaces;
using newConsoleApp2.Models;

namespace newConsoleApp2.Managers
{
    public class TableManager : ITableManager
    {
      
        private List<Table> _tables;
        private readonly IFileHandler _fileHandler;

        // Constructor for TableManager, takes an IFileHandler as parameter
        public TableManager(IFileHandler fileHandler)
        {
            _fileHandler = fileHandler;
            _tables = new List<Table>();
            InitializeTables();
        }
        private void InitializeTables()
        {
            _tables.Clear();
            AddTable(new Table(1, 2));
            AddTable(new Table(2, 4)); 
            AddTable(new Table(3, 6)); 
            AddTable(new Table(4, 8)); 
            AddTable(new Table(5, 2)); 
            AddTable(new Table(6, 4)); 
            AddTable(new Table(7, 5));
            AddTable(new Table(8, 8)); 
            AddTable(new Table(9, 2)); 
            AddTable(new Table(10, 4)); 
        }
        public void AddTable(Table table)
        {
            _tables.Add(table);
        }

        // Method to get a table by its number
        public Table? GetTable(int tableNumber)
        {
            // Return the first table that matches the given number, or null if not found
            return _tables.FirstOrDefault(t => t.Number == tableNumber);
        }
        public void DisplayTableStatus()
        {
            Console.Clear();
            Console.WriteLine(" ╔════════════════════════════════════════╗");
            Console.WriteLine(" ║              Table status              ║");
            Console.WriteLine(" ╚════════════════════════════════════════╝");
            foreach (var table in _tables)
            {
                Console.WriteLine(table);
            }
            Console.WriteLine("════════════════════════════════════════");
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }

        // Method to get all tables
        public IEnumerable<Table> GetAllTables()
        {
            return _tables;
        }

        public void LoadPastOrders()
        {
            // Read past order data from file
            List<Order> pastOrders = _fileHandler.ReadOrderData(this);
            foreach (var order in pastOrders)
            {
                Table table = GetTable(order.Table.Number);
                // If the table exists, add the order to its past orders
                if (table != null)
                {
                    table.PastOrders.Add(order);
                }
            }
        }

        // Method to find a suitable table for a given number of people
        public Table? FindSuitableTable(int numberOfPeople)
        {
            // Return the first unoccupied table with sufficient capacity, or null if none found
            return _tables.FirstOrDefault(t => !t.IsOccupied && t.Capacity >= numberOfPeople);
        }
    }
}