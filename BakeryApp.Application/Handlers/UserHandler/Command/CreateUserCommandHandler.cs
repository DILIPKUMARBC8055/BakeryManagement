
using MediatR;
using BakeryApp.Core.Entities;
using BakeryApp.Core.Repositary;
using BakeryApp.Application.Mappers;
using BakeryApp.Application.Response;
using BakeryApp.Application.Commands.UserCommand;

namespace BakeryApp.Application.Handlers.UserHandler.Command
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUserRepo _userRepository;


        public CreateUserCommandHandler(IUserRepo userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {

            
            var user = BakeryMapper.Mapper.Map<User>(request);

           
            await _userRepository.AddAsync(user);

            return BakeryMapper.Mapper.Map<UserDto>(user);
        }
    }
}
