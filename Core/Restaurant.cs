using newConsoleApp2.Interfaces;
using newConsoleApp2.Managers;
using newConsoleApp2.Models;
using newConsoleApp2.Services;
using newConsoleApp2.Utilities;

namespace newConsoleApp2.Core
{
    public class Restaurant
    {
        // Singleton instance
        private static Restaurant? _instance;
        // Thread-safe lock object
        private static readonly object _lock = new object();

        // Public property to access the singleton instance
        public static Restaurant Instance
        {
            get
            { // Double-checked locking pattern
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new Restaurant();
                        }
                    }
                }
                return _instance;
            }
        }

        private readonly IFileHandler _fileHandler;
        private readonly IMenuManager _menuManager;
        private readonly ITableManager _tableManager;

        private Restaurant()
        {
            _fileHandler = new FileHandler();
            _menuManager = new MenuManager(_fileHandler);
            _tableManager = new TableManager(_fileHandler);
        }

        public void Run()
        {
            while (true)
            {
                DisplayMainMenu();
                string choice = Console.ReadLine();

                try
                {
                    ProcessUserChoice(choice);
                }
                catch (Exception ex)
                {
                    ErrorHandler.LogError("An error occurred", ex);
                }
            }
        }

        private void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║    Restaurant Management System    ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.WriteLine("1. Display Menu");
            Console.WriteLine("2. Display Table Status");
            Console.WriteLine("3. Display Current Orders");
            Console.WriteLine("4. Occupy Table");
            Console.WriteLine("5. Place Order");
            Console.WriteLine("6. Free Table");
            Console.WriteLine("7. Print Receipt");
            Console.WriteLine("8. Email Receipt");
            Console.WriteLine("9. View Order History");
            Console.WriteLine("10. Add Menu Item");
            Console.WriteLine("11. Remove Menu Item");
            Console.WriteLine("12. Exit");
            Console.WriteLine("════════════════════════════════════");
            Console.Write("Enter your choice: ");
        }

        private void ProcessUserChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    _menuManager.DisplayMenu();
                    break;
                case "2":
                    _tableManager.DisplayTableStatus();
                    break;
                case "3":
                    OrderDisplay.DisplayOrders(_tableManager);
                    break;
                case "4":
                    OccupyTable();
                    break;
                case "5":
                    PlaceOrder();
                    break;
                case "6":
                    FreeTable();
                    break;
                case "7":
                    PrintReceipt();
                    break;
                case "8":
                    EmailReceipt();
                    break;
                case "9":
                    ViewOrderHistory();
                    break;
                case "10":
                    AddMenuItem();
                    break;
                case "11":
                    RemoveMenuItem();
                    break;
                case "12":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        private void OccupyTable()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║           Occupy a Table           ║");
            Console.WriteLine("╚════════════════════════════════════╝");

            Console.Write("How many people in your party? ");
            if (InputValidator.ValidateInteger(Console.ReadLine(), out int partySize, 1))
            {
                Table? suitableTable = _tableManager.FindSuitableTable(partySize);
                if (suitableTable != null)
                {
                    Console.WriteLine($"We have a suitable table for you. Table {suitableTable.Number} (Capacity: {suitableTable.Capacity})");
                    Console.Write("Would you like to occupy this table? (y/n): ");
                    if (Console.ReadLine().ToLower().StartsWith("y"))
                    {
                        suitableTable.Occupy();
                        Console.WriteLine($"Table {suitableTable.Number} is now occupied.");
                    }
                }
                else
                {
                    Console.WriteLine("I'm sorry, we don't have any suitable tables available at the moment.");
                    Console.WriteLine("You may need to wait a few minutes for a table to become available.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid number of people.");
            }

            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }

        private void PlaceOrder()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║            Place an Order          ║");
            Console.WriteLine("╚════════════════════════════════════╝");

            Console.Write("Enter table number to place order: ");
            if (InputValidator.ValidateInteger(Console.ReadLine(), out int tableNumber, 1))
            {
                Table table = _tableManager.GetTable(tableNumber);
                if (table != null && table.IsOccupied && table.CurrentOrder != null)
                {
                    var menuItems = _menuManager.GetAllMenuItems();
                    var selectedItems = ConsoleInputHandler.SelectMenuItems(menuItems);

                    foreach (var (item, quantity) in selectedItems)
                    {
                        item.Quantity = quantity;
                        table.CurrentOrder.AddItem(item);
                        Console.WriteLine($"Added {quantity} {item.Name} to the order.");
                    }

                    Console.WriteLine($"Order placed for Table {tableNumber}. Total: ${table.CurrentOrder.CalculateTotal():F2}");

                    // Always save the order data
                    _fileHandler.WriteOrderData(table.CurrentOrder);

                    
                    Console.Write("Print restaurant receipt? (y/n): ");
                    if (Console.ReadLine().ToLower().StartsWith("y"))
                    {
                        ReceiptGenerator.PrintReceipt(table.CurrentOrder, true);
                    }

                    Console.Write("Email restaurant receipt? (y/n): ");
                    if (Console.ReadLine().ToLower().StartsWith("y"))
                    {
                        Console.Write("Enter email address: ");
                        string email = Console.ReadLine();
                        ReceiptGenerator.EmailReceipt(table.CurrentOrder, true, email);
                    }

                    
                    Console.Write("Print customer receipt? (y/n): ");
                    if (Console.ReadLine().ToLower().StartsWith("y"))
                    {
                        ReceiptGenerator.PrintReceipt(table.CurrentOrder, false);
                    }

                    Console.Write("Email customer receipt? (y/n): ");
                    if (Console.ReadLine().ToLower().StartsWith("y"))
                    {
                        Console.Write("Enter email address: ");
                        string email = Console.ReadLine();
                        ReceiptGenerator.EmailReceipt(table.CurrentOrder, false, email);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid table number or table is not occupied.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid table number.");
            }
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }

        private void FreeTable()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║            Free a Table            ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.Write("Enter table number to free: ");
            if (InputValidator.ValidateInteger(Console.ReadLine(), out int tableNumber, 1))
            {
                Table table = _tableManager.GetTable(tableNumber);
                if (table != null)
                {
                    try
                    {
                        table.Free();
                        Console.WriteLine($"Table {tableNumber} is now free.");
                    }
                    catch (InvalidOperationException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine("Invalid table number.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid table number.");
            }
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }

        private void PrintReceipt()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║            Print Receipt           ║ ");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.Write("Enter table number to print receipt: ");
            if (InputValidator.ValidateInteger(Console.ReadLine(), out int tableNumber, 1))
            {
                Table table = _tableManager.GetTable(tableNumber);
                if (table != null && table.IsOccupied && table.CurrentOrder != null)
                {
                    ReceiptGenerator.PrintReceipt(table.CurrentOrder, true);
                }
                else
                {
                    Console.WriteLine("Invalid table number or table is not occupied.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid table number.");
            }
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }

        private void EmailReceipt()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║            Email Receipt           ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.Write("Enter table number to email receipt: ");
            if (InputValidator.ValidateInteger(Console.ReadLine(), out int tableNumber, 1))
            {
                Table table = _tableManager.GetTable(tableNumber);
                if (table != null && table.IsOccupied && table.CurrentOrder != null)
                {
                    Console.Write("Enter email address: ");
                    string email = Console.ReadLine();
                    if (InputValidator.ValidateEmail(email))
                    {
                        ReceiptGenerator.EmailReceipt(table.CurrentOrder, true, email);
                    }
                    else
                    {
                        Console.WriteLine("Invalid email address.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid table number or table is not occupied.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid table number.");
            }
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }

        private void ViewOrderHistory()
        {
            Console.Clear();
            Console.WriteLine("╔═════════════════════════════════════╗");
            Console.WriteLine("║             Order History           ║");
            Console.WriteLine("╚═════════════════════════════════════╝");
            Console.Write("Enter table number to view order history: ");
            if (InputValidator.ValidateInteger(Console.ReadLine(), out int tableNumber, 1))
            {
                Table table = _tableManager.GetTable(tableNumber);
                if (table != null)
                {
                    Console.WriteLine($"Number of past orders: {table.PastOrders.Count}"); // Debugging line
                    var pastOrders = table.PastOrders;
                    if (pastOrders.Count > 0)
                    {
                        Console.WriteLine($"Order history for Table {tableNumber}:");
                        foreach (var order in pastOrders)
                        {
                            Console.WriteLine($"Order Time: {order.OrderTime}");
                            Console.WriteLine($"Total: ${order.CalculateTotal():F2}");
                            Console.WriteLine("Items:");
                            foreach (var item in order.Items)
                            {
                                Console.WriteLine($"  - {item.Name} x{item.Quantity} - ${item.Price * item.Quantity:F2}");
                            }
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                    else
                    {
                        Console.WriteLine($"No order history for Table {tableNumber}.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid table number.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid table number.");
            }
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }

        private void AddMenuItem()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║            Add Menu Item           ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.Write("Enter item type (1 for Food, 2 for Drink): ");
            string itemType = Console.ReadLine();

            Console.Write("Enter item name: ");
            string name = Console.ReadLine();
            Console.Write("Enter item price: ");
            if (InputValidator.ValidateDecimal(Console.ReadLine(), out decimal price))
            {
                MenuItem newItem;
                if (itemType == "1")
                {
                    Console.Write("Enter cuisine: ");
                    string cuisine = Console.ReadLine();
                    newItem = new FoodItem(0, name, price, cuisine);
                }
                else if (itemType == "2")
                {
                    Console.Write("Is alcoholic? (true/false): ");
                    bool isAlcoholic = bool.Parse(Console.ReadLine());
                    newItem = new DrinkItem(0, name, price, isAlcoholic);
                }
                else
                {
                    Console.WriteLine("Invalid item type. Menu item not added.");
                    return;
                }

                _menuManager.AddMenuItem(newItem);
                Console.WriteLine("Menu item added successfully.");
            }
            else
            {
                Console.WriteLine("Invalid price. Menu item not added.");
            }
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }

        private void RemoveMenuItem()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║          Remove Menu Item          ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            Console.Write("Enter item name to remove: ");
            string name = Console.ReadLine();
            MenuItem item = _menuManager.SelectItem(name);
            if (item != null)
            {
                _menuManager.RemoveMenuItem(item);
                Console.WriteLine("Menu item removed successfully.");
            }
            else
            {
                Console.WriteLine("Item not found in the menu.");
            }
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }

        // Helper methods
        private void DisplayMenu()
        {
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║                Menu                ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            var menuItems = _menuManager.GetAllMenuItems();
            foreach (var item in menuItems)
            {
                Console.WriteLine($"{item.Name} - ${item.Price:F2}");
            }
            Console.WriteLine("════════════════════════════════════");
            Console.WriteLine();
        }

        private void DisplayFreeTables()
        {
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║             Free Tables            ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            var allTables = _tableManager.GetAllTables();
            foreach (var table in allTables)
            {
                if (!table.IsOccupied)
                {
                    Console.WriteLine($"Table {table.Number} (Capacity: {table.Capacity})");
                }
            }
            Console.WriteLine("════════════════════════════════════");
            Console.WriteLine();
        }
    }
}