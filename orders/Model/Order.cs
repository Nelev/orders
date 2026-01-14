namespace orders.Model
{
    public class Order
    {
        public int Id { get; set; }
        public string? CustomerId { get; set; }

        public List<string> Items { get; set; } = new();

    }
}
