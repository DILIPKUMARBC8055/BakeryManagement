using BakeryApp.Application.Commands.OrderCommand;
using BakeryApp.Application.Mappers;
using BakeryApp.Application.Response;
using BakeryApp.Core.Entities;
using BakeryApp.Core.Repositary;
using MediatR;

namespace BakeryApp.Application.Handlers.OrderHandlers.CommandHandler
{
    public class UpdateOrderHandler : IRequestHandler<UpdateOrderCommand, OrderDto>
    {
        private readonly IOrderRepo _repo;

        public UpdateOrderHandler(IOrderRepo repo)
        {
            _repo = repo;
        }

        async Task<OrderDto> IRequestHandler<UpdateOrderCommand, OrderDto>.Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _repo.GetByIdAsync(request.Id);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {request.Id} not found.");
            }

            order.CustomerName = request.CustomerName;
            order.Items = BakeryMapper.Mapper.Map<List<BakeryItem>>(request.Items);
            order.Items = order.Items.Where(item => item.Quantity > 0).ToList();
            order.Total = order.Items.Sum(item => item.Quantity * item.Price);

            await _repo.UpdateAsync(order);
            return BakeryMapper.Mapper.Map<OrderDto>(order);
        }
    }
}
