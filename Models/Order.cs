namespace newConsoleApp2.Models
{
    public class Order
    {
        public Table Table { get; }
        public List<MenuItem> Items { get; }
        public DateTime OrderTime { get; set; }

        public Order(Table table)
        {
            Table = table;
            Items = new List<MenuItem>();
            OrderTime = DateTime.Now;
        }

        public void AddItem(MenuItem item)
        {
            Items.Add(item);
        }

        public void RemoveItem(MenuItem item)
        {
            Items.Remove(item);
        }

        public decimal CalculateTotal()
        {
            return Items.Sum(item => item.Price * item.Quantity);
        }

        public override string ToString()
        {
            return $"Order for Table {Table.Number} - {Items.Count} items, Total: ${CalculateTotal():F2}";
        }
    }
}
