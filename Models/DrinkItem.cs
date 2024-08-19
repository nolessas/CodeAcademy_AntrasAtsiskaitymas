namespace newConsoleApp2.Models
{
    public class DrinkItem : MenuItem
    {
        public bool IsAlcoholic { get; set; }

        public DrinkItem(int id, string name, decimal price, bool isAlcoholic, int quantity = 0)
            : base(id, name, price, quantity)
        {
            IsAlcoholic = isAlcoholic;
        }

        public override string GetCategory() => "Drink";
    }
}
