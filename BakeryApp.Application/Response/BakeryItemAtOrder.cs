namespace BakeryApp.Application.Response
{
    public class BakeryItemAtOrder
    {
        public Guid Id { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
    }
}
