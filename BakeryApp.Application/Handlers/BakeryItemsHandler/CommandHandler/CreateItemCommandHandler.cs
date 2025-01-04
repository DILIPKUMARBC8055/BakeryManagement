using BakeryApp.Application.Commands.ItemCommand;
using BakeryApp.Application.Mappers;
using BakeryApp.Application.Response;
using BakeryApp.Core.Entities;
using BakeryApp.Core.Repositary;
using MediatR;

namespace BakeryApp.Application.Handlers.BakeryItemsHandler.CommandHandler
{
    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, BakeryItemDto>
    {
        private readonly IBakeryItemRepo _repo;
        public CreateItemCommandHandler(IBakeryItemRepo repo)
        {
            _repo = repo;

        }
        public async Task<BakeryItemDto> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var item = BakeryMapper.Mapper.Map<BakeryItem>(request);
            if (item == null)
            {
                return null;
            }
            item.Id = Guid.NewGuid();
            await _repo.AddAsync(item);
            return BakeryMapper.Mapper.Map<BakeryItemDto>(item);

        }
    }
}
