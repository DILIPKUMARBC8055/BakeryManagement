namespace BakeryApp.Application.Response
{
    public class BakeryItemDto
    {
        public Guid Id { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string Status { get; set; }
    }
}
