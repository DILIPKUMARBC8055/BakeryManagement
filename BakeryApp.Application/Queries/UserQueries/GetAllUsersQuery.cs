using BakeryApp.Application.Response;
using MediatR;

namespace BakeryApp.Application.Queries.UserQueries
{
    public class GetAllUsersQuery : IRequest<List<UserDto>>
    {
    }
}
