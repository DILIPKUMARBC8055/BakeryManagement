using BakeryApp.Application.Response;
using MediatR;

namespace BakeryApp.Application.Queries.OrderQueries
{
    public class GetAllOrdersQuery : IRequest<List<OrderDto>>
    {
    }
}
