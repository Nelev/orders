using System.ComponentModel.DataAnnotations.Schema;

namespace orders.Model
{
    [Table("orders")]
    public class Order
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("customerid")]
        public char? CustomerId { get; set; }
        [Column("items")]
        public List<string> Items { get; set; } = new();

    }
}
