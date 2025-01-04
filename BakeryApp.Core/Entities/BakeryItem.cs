namespace BakeryApp.Core.Entities
{
    public class BakeryItem
    {
        public Guid Id { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string Status { get; set; }
    }
}
