namespace BakeryApp.Core.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public List<BakeryItem> Items { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
