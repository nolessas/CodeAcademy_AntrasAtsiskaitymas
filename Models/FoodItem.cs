namespace newConsoleApp2.Models
{
    public class FoodItem : MenuItem
    {
        public string Cuisine { get; set; }

        public FoodItem(int id, string name, decimal price, string cuisine, int quantity = 0)
            : base(id, name, price, quantity)
        {
            Cuisine = cuisine;
        }

        public override string GetCategory() => "Food";
    }
}
