using BakeryApp.Application.Commands.OrderCommand;
using BakeryApp.Core.Repositary;
using MediatR;

namespace BakeryApp.Application.Handlers.OrderHandlers.CommandHandler
{
    public class DeleteOrderHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IOrderRepo _repo;

        public DeleteOrderHandler(IOrderRepo repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _repo.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {request.OrderId} not found.");
            }

            await _repo.DeleteAsync(request.OrderId);
            return true;
        }
    }
}
