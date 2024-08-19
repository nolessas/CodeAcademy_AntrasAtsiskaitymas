using newConsoleApp2.Interfaces;

public class OrderDisplay
{
    public static void DisplayOrders(ITableManager tableManager)
    {
        Console.Clear();
        Console.WriteLine("===================================");
        Console.WriteLine("         Current Orders            ");
        Console.WriteLine("===================================");
        foreach (var table in tableManager.GetAllTables())
        {
            if (table.IsOccupied && table.CurrentOrder != null)
            {
                Console.WriteLine($"Table {table.Number} (Capacity: {table.Capacity}):");
                foreach (var item in table.CurrentOrder.Items)
                {
                    Console.WriteLine($"  - {item.Name} x{item.Quantity} - ${item.Price * item.Quantity:F2}");
                }
                Console.WriteLine($"  Total: ${table.CurrentOrder.CalculateTotal():F2}");
                Console.WriteLine();
            }
        }
        Console.WriteLine("===================================");
        Console.WriteLine("Press any key to return to the main menu...");
        Console.ReadKey();
    }
}
