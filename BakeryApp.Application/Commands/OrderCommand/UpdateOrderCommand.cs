using BakeryApp.Application.Response;
using MediatR;

namespace BakeryApp.Application.Commands.OrderCommand
{
    public class UpdateOrderCommand : IRequest<OrderDto>
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public List<BakeryItemDto> Items { get; set; }
        
    }
}
