namespace newConsoleApp2.Models
{
    public class Table
    {
        public int Number { get; }
        public int Capacity { get; }
        public bool IsOccupied { get; private set; }
        public Order? CurrentOrder { get; private set; }
        public List<Order> PastOrders { get; } = new List<Order>();

        public Table(int number, int capacity)
        {
            Number = number;
            Capacity = capacity;
        }

        public void Occupy()
        {
            if (IsOccupied)
            {
                throw new InvalidOperationException($"Table {Number} is already occupied.");
            }
            IsOccupied = true;
            CurrentOrder = new Order(this);
        }

        public void Free()
        {
            if (!IsOccupied)
            {
                throw new InvalidOperationException($"Table {Number} is not occupied.");
            }
            if (CurrentOrder != null)
            {
                PastOrders.Add(CurrentOrder);
            }
            IsOccupied = false;
            CurrentOrder = null;
        }

        public override string ToString()
        {
            return $"Table {Number} (Capacity: {Capacity}) - {(IsOccupied ? "Occupied" : "Free")}";
        }
    }
}
