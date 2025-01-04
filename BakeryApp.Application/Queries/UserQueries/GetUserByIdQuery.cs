// Queries/GetUserByIdQuery.cs
using MediatR;
using BakeryApp.Core.Entities;
using BakeryApp.Application.Response;

namespace BakeryApp.Application.Queries.UserQueries
{
    public class GetUserByIdQuery : IRequest<UserDto>
    {
        public Guid Id { get; set; }

        public GetUserByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}
