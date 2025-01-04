using BakeryApp.Application.Mappers;
using BakeryApp.Application.Queries.OrderQueries;
using BakeryApp.Application.Response;
using BakeryApp.Core.Repositary;
using MediatR;

namespace BakeryApp.Application.Handlers.OrderHandlers.QueryHandler
{
    public class GetAllOrdersHandler : IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
    {
        private readonly IOrderRepo _repo;
        public GetAllOrdersHandler(IOrderRepo repo)
        {
            _repo = repo;
        }
        public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _repo.GetAllAsync();
            var resonse = BakeryMapper.Mapper.Map<List<OrderDto>>(orders);
            return resonse;
        }
    }
}
