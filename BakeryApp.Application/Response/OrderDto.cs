namespace BakeryApp.Application.Response
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public List<BakeryItemDto> Items { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
