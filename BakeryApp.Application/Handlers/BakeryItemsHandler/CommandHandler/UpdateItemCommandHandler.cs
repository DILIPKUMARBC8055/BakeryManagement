using BakeryApp.Application.Commands.ItemCommand;
using BakeryApp.Application.Mappers;
using BakeryApp.Application.Response;
using BakeryApp.Core.Entities;
using BakeryApp.Core.Repositary;
using MediatR;

namespace BakeryApp.Application.Handlers.BakeryItemsHandler.CommandHandler
{
    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, BakeryItemDto>
    {
        private readonly IBakeryItemRepo _repo;
        public UpdateItemCommandHandler(IBakeryItemRepo repo)
        {
            _repo = repo;
        }
        public async Task<BakeryItemDto> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var item = await _repo.GetByIdAsync(request.Id);
            if (item == null)
            {
                throw new KeyNotFoundException($"Item with ID {request.Id} not found.");
            }
            
            item.ItemName = request.ItemName;
            item.Price = request.Price;
            item.Quantity = request.Quantity;
            await _repo.UpdateAsync(item);
            return BakeryMapper.Mapper.Map<BakeryItemDto>(item);
        }
    }
}
