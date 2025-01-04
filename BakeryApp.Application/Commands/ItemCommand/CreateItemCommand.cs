using BakeryApp.Application.Response;
using MediatR;

namespace BakeryApp.Application.Commands.ItemCommand
{
    public class CreateItemCommand :IRequest<BakeryItemDto>
    {
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }
}
