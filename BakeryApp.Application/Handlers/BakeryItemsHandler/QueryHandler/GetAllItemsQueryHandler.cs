using BakeryApp.Application.Mappers;
using BakeryApp.Application.Queries.BakeryItemQueries;
using BakeryApp.Application.Response;
using BakeryApp.Core.Repositary;
using MediatR;

namespace BakeryApp.Application.Handlers.BakeryItemsHandler.QueryHandler
{
    public class GetAllItemsQueryHandler : IRequestHandler<GetAllItemsQuery, List<BakeryItemDto>>
    {
        private readonly IBakeryItemRepo _itemRepo;
        public GetAllItemsQueryHandler(IBakeryItemRepo itemRepo)
        {
            _itemRepo = itemRepo;

        }
        public async Task<List<BakeryItemDto>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
        {
            var items = await _itemRepo.GetAllAsync();
            var response = BakeryMapper.Mapper.Map<List<BakeryItemDto>>(items);
            return response;
        }
    }
}
