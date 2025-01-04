using BakeryApp.Application.Response;
using MediatR;

namespace BakeryApp.Application.Queries.OrderQueries
{
    public class GetOrderByIdQuery : IRequest<OrderDto>
    {
        public Guid OrderId { get; set; }
        public GetOrderByIdQuery(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
