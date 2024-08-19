namespace newConsoleApp2.Models
{
    public abstract class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        protected MenuItem(int id, string name, decimal price, int quantity = 0)
        {
            Id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
        }

        public abstract string GetCategory();

        public override string ToString()
        {
            return $"{Name} - ${Price:F2} ({GetCategory()})";
        }
    }
}