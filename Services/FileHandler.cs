using newConsoleApp2.Interfaces;
using newConsoleApp2.Models;

namespace newConsoleApp2.Services
{
    public class FileHandler : IFileHandler
    {
        private const string FoodMenuFilePath = "food_menu.txt";
        private const string DrinksMenuFilePath = "drinks_menu.txt";
        private const string OrdersFilePath = "orders.txt";

        public List<MenuItem> ReadMenuItems()
        {
            List<MenuItem> menuItems = new List<MenuItem>();
            try
            {
                menuItems.AddRange(ReadFoodItems());
                menuItems.AddRange(ReadDrinkItems());
            }
            catch (Exception ex)
            {
                ErrorHandler.LogError("Error reading menu items", ex);
            }
            return menuItems;
        }

        private List<FoodItem> ReadFoodItems()
        {
            List<FoodItem> items = new List<FoodItem>();
            try
            {
                if (File.Exists(FoodMenuFilePath))
                {
                    string[] lines = File.ReadAllLines(FoodMenuFilePath);
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 4)
                        {
                            int id = int.Parse(parts[0].Trim());
                            string name = parts[1].Trim();
                            decimal price = decimal.Parse(parts[2].Trim());
                            string cuisine = parts[3].Trim();
                            items.Add(new FoodItem(id, name, price, cuisine));
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Food menu file not found: {FoodMenuFilePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading food menu items: {ex.Message}");
            }
            return items;
        }

        private List<DrinkItem> ReadDrinkItems()
        {
            List<DrinkItem> items = new List<DrinkItem>();
            try
            {
                if (File.Exists(DrinksMenuFilePath))
                {
                    string[] lines = File.ReadAllLines(DrinksMenuFilePath);
                    foreach (string line in lines)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 4)
                        {
                            int id = int.Parse(parts[0].Trim());
                            string name = parts[1].Trim();
                            decimal price = decimal.Parse(parts[2].Trim());
                            bool isAlcoholic = bool.Parse(parts[3].Trim());
                            items.Add(new DrinkItem(id, name, price, isAlcoholic));
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Drinks menu file not found: {DrinksMenuFilePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading drink menu items: {ex.Message}");
            }
            return items;
        }

        public void WriteMenuItems(List<MenuItem> menuItems)
        {
            WriteFoodItems(menuItems.OfType<FoodItem>().ToList());
            WriteDrinkItems(menuItems.OfType<DrinkItem>().ToList());
        }

        private void WriteFoodItems(List<FoodItem> items)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(FoodMenuFilePath))
                {
                    foreach (var item in items)
                    {
                        writer.WriteLine($"{item.Id},{item.Name},{item.Price},{item.Cuisine}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing food menu items: {ex.Message}");
            }
        }

        private void WriteDrinkItems(List<DrinkItem> items)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(DrinksMenuFilePath))
                {
                    foreach (var item in items)
                    {
                        writer.WriteLine($"{item.Id},{item.Name},{item.Price},{item.IsAlcoholic}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing drink menu items: {ex.Message}");
            }
        }

        public void WriteOrderData(Order order)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(OrdersFilePath, true))
                {
                    writer.WriteLine($"Order Time: {order.OrderTime}");
                    writer.WriteLine($"Table: {order.Table.Number}");
                    writer.WriteLine("Items:");
                    foreach (var item in order.Items)
                    {
                        writer.WriteLine($"- {item.Name}, ${item.Price:F2}, Quantity: {item.Quantity}");
                    }
                    writer.WriteLine($"Total: ${order.CalculateTotal():F2}");
                    writer.WriteLine(new string('-', 40));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing order data: {ex.Message}");
            }
        }

        public List<Order> ReadOrderData(ITableManager tableManager)
        {
            List<Order> orders = new List<Order>();
            try
            {
                if (File.Exists(OrdersFilePath))
                {
                    string[] lines = File.ReadAllLines(OrdersFilePath);
                    Order currentOrder = null;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string line = lines[i];
                        if (line.StartsWith("Order Time:"))
                        {
                            if (currentOrder != null)
                            {
                                orders.Add(currentOrder);
                            }
                            string dateTimeString = line.Substring("Order Time:".Length).Trim();
                            DateTime orderTime = DateTime.Parse(dateTimeString);
                            int tableNumber = int.Parse(lines[++i].Substring("Table:".Length).Trim());
                            Table table = tableManager.GetTable(tableNumber);
                            currentOrder = new Order(table) { OrderTime = orderTime };
                        }
                        else if (line.StartsWith("-"))
                        {
                            string[] parts = line.Substring(1).Split(',');
                            string name = parts[0].Trim();
                            decimal price = decimal.Parse(parts[1].Trim().Substring(1));
                            int quantity = int.Parse(parts[2].Substring("Quantity:".Length).Trim());
                            currentOrder?.AddItem(new FoodItem(0, name, price, "", quantity));
                        }
                    }
                    if (currentOrder != null)
                    {
                        orders.Add(currentOrder);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading order data: {ex.Message}");
            }
            return orders;
        }
    }
}
