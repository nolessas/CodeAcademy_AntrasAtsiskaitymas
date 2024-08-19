using newConsoleApp2.Models;
using System;
using System.Collections.Generic;

namespace newConsoleApp2.Utilities
{
    public static class ConsoleInputHandler
    {
        public static List<(MenuItem Item, int Quantity)> SelectMenuItems(List<MenuItem> menuItems)
        {
            var selectedItems = new List<(MenuItem Item, int Quantity)>();
            int currentIndex = 0;
            bool selecting = true;

            while (selecting)
            {
                Console.Clear();
                Console.WriteLine(" ╔═════════════════════════════════════╗");
                Console.WriteLine(" ║           Restaurant Menu           ║");
                Console.WriteLine(" ╚═════════════════════════════════════╝");
                Console.WriteLine("       Use arrow keys to navigate,");
                Console.WriteLine("            SPACE to select,");
                Console.WriteLine("            ENTER to finish");
                Console.WriteLine();

                var foodItems = menuItems.Where(i => i is FoodItem).ToList();
                var drinkItems = menuItems.Where(i => i is DrinkItem).ToList();

                DisplayMenuSection(" Food ", foodItems, currentIndex, selectedItems);
                Console.WriteLine();
                DisplayMenuSection("Drinks", drinkItems, currentIndex, selectedItems, foodItems.Count);

                var key = Console.ReadKey(true).Key;
                switch (key)
                { 
                    case ConsoleKey.UpArrow:
                        currentIndex = (currentIndex - 1 + menuItems.Count) % menuItems.Count;
                        break;
                    case ConsoleKey.DownArrow:
                        currentIndex = (currentIndex + 1) % menuItems.Count;
                        break;
                    case ConsoleKey.Spacebar:
                        var selectedItem = menuItems[currentIndex];
                        var existingItem = selectedItems.Find(si => si.Item == selectedItem);
                        if (existingItem.Item != null)
                        {
                            selectedItems.Remove(existingItem);
                            selectedItems.Add((existingItem.Item, existingItem.Quantity + 1));
                        }
                        else
                        {
                            selectedItems.Add((selectedItem, 1));
                        }
                        break;
                    case ConsoleKey.Enter:
                        selecting = false;
                        break;
                }
            }

            return selectedItems;
        }

        private static void DisplayMenuSection(string sectionName, List<MenuItem> items, int currentIndex, List<(MenuItem Item, int Quantity)> selectedItems, int offset = 0)
        {
            Console.WriteLine($"  ═══════════════ {sectionName} ═══════════════");
            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                var quantity = selectedItems.Find(si => si.Item == item).Quantity;
                string prefix = i + offset == currentIndex ? ">" : " ";
                Console.WriteLine($"{prefix} {item.Name,-30} ${item.Price,6:F2} {(quantity > 0 ? $"(x{quantity})" : "")}");
            }
        }
    }
}