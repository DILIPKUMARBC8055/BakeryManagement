using BakeryApp.Application.Mappers;
using BakeryApp.Application.Queries.UserQueries;
using BakeryApp.Application.Response;
using BakeryApp.Core.Repositary;
using MediatR;

namespace BakeryApp.Application.Handlers.UserHandler.Query
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
    {
        private readonly IUserRepo _repo;
        public GetAllUsersQueryHandler(IUserRepo repo)
        {
            _repo = repo;
        }
        public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _repo.GetAllAsync();
            var result = BakeryMapper.Mapper.Map<List<UserDto>>(users);
            return result;


        }
    }
}
