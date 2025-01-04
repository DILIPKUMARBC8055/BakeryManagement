using BakeryApp.Application.Response;
using MediatR;

namespace BakeryApp.Application.Queries.BakeryItemQueries
{
    public class GetItemByIdQuery : IRequest<BakeryItemDto>
    {
        public Guid ItemId { get; set; }
        public GetItemByIdQuery(Guid id)
        {
            ItemId = id;
        }

    }
}
