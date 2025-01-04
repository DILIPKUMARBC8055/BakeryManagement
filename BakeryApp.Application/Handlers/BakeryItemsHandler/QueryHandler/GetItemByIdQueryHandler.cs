using BakeryApp.Application.Mappers;
using BakeryApp.Application.Queries.BakeryItemQueries;
using BakeryApp.Application.Response;
using BakeryApp.Core.Repositary;
using MediatR;

namespace BakeryApp.Application.Handlers.BakeryItemsHandler.QueryHandler
{
    public class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, BakeryItemDto>
    {
        private readonly IBakeryItemRepo _repo;
        public GetItemByIdQueryHandler(IBakeryItemRepo repo)
        {
            _repo = repo;
        }
        public async Task<BakeryItemDto> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
        {
            var item = await _repo.GetByIdAsync(request.ItemId);
            var response = BakeryMapper.Mapper.Map<BakeryItemDto>(item);
            return response;
        }
    }
}
