using MediatR;

namespace BakeryApp.Application.Commands.OrderCommand
{
    public class DeleteOrderCommand : IRequest<bool>
    {
        public Guid OrderId { get; set; }
        public DeleteOrderCommand(Guid orderId)
        {
            OrderId = orderId;
        }
    }
}
