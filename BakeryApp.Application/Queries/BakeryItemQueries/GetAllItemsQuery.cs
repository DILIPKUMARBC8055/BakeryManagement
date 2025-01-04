using BakeryApp.Application.Response;
using MediatR;

namespace BakeryApp.Application.Queries.BakeryItemQueries
{
    public class GetAllItemsQuery : IRequest<List<BakeryItemDto>>
    {
    }
}
