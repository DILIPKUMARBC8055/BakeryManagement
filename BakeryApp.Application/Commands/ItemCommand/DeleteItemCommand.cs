using MediatR;

namespace BakeryApp.Application.Commands.ItemCommand
{
    public class DeleteItemCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public DeleteItemCommand(Guid id)
        {
            Id = id;
        }
    }
}
