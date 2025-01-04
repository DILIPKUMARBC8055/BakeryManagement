using BakeryApp.Application.Commands.OrderCommand;
using BakeryApp.Application.Mappers;
using BakeryApp.Application.Response;
using BakeryApp.Core.Entities;
using BakeryApp.Core.Repositary;
using MediatR;

namespace BakeryApp.Application.Handlers.OrderHandlers.CommandHandler
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, OrderDto>
    {
        private readonly IOrderRepo _repo;

        public CreateOrderHandler(IOrderRepo repo)
        {
            _repo = repo;
        }

        public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = BakeryMapper.Mapper.Map<Order>(request);
            orderEntity.Id = Guid.NewGuid();
            orderEntity.OrderDate = DateTime.Now;
            orderEntity.Items = orderEntity.Items.Where(item => item.Quantity > 0).ToList();
            orderEntity.Total = orderEntity.Items.Sum(it => it.Quantity * it.Price);
            await _repo.AddAsync(orderEntity);
            
            return BakeryMapper.Mapper.Map<OrderDto>(orderEntity);
        }
    }
}
