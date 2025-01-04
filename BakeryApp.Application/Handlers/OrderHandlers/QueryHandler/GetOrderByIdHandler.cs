using BakeryApp.Application.Mappers;
using BakeryApp.Application.Queries.OrderQueries;
using BakeryApp.Application.Response;
using BakeryApp.Core.Repositary;
using MediatR;

namespace BakeryApp.Application.Handlers.OrderHandlers.QueryHandler
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IOrderRepo _repo;

        public GetOrderByIdHandler(IOrderRepo repo)
        {
            _repo = repo;
        }

        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _repo.GetByIdAsync(request.OrderId);
            var response = BakeryMapper.Mapper.Map<OrderDto>(order);
            return response;
        }
    }
}
