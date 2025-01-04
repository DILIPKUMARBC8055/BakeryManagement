// Handlers/GetUserByIdQueryHandler.cs
using MediatR;
using BakeryApp.Application.Response;
using BakeryApp.Core.Repositary;
using BakeryApp.Application.Mappers;

namespace BakeryApp.Application.Queries.UserQueries
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
    {
        private readonly IUserRepo _userRepository;

        public GetUserByIdQueryHandler(IUserRepo userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            
            var user = await _userRepository.GetByIdAsync(request.Id);
            return BakeryMapper.Mapper.Map<UserDto>(user);
        }
    }
}
