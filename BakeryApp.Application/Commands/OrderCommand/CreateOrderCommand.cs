using BakeryApp.Application.Response;
using BakeryApp.Core.Entities;
using MediatR;

namespace BakeryApp.Application.Commands.OrderCommand
{
    public class CreateOrderCommand : IRequest<OrderDto>
    {
        public string CustomerName { get; set; }
        public List<BakeryItem> Items { get; set; }
       
       
    }
}
