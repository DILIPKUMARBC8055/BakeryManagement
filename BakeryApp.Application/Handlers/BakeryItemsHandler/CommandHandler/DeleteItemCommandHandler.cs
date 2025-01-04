using BakeryApp.Application.Commands.ItemCommand;
using BakeryApp.Core.Repositary;
using MediatR;

namespace BakeryApp.Application.Handlers.BakeryItemsHandler.CommandHandler
{
    public class DeleteItemCommandHandler : IRequestHandler<DeleteItemCommand, bool>
    {
        private readonly IBakeryItemRepo _repo;
        public DeleteItemCommandHandler(IBakeryItemRepo repo)
        {
            _repo = repo;

        }
        public async Task<bool> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
        {
            var order = await _repo.GetByIdAsync(request.Id);
            if (order == null)
            {
                throw new KeyNotFoundException($"Order with ID {request.Id} not found.");
            }

            await _repo.DeleteAsync(request.Id);
            return true;
        }
    }
}
