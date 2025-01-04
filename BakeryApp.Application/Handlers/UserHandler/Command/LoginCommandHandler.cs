using BakeryApp.Application.Services;
using MediatR;
using BakeryApp.Core.Repositary;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly IUserRepo _userRepository;
    private readonly PasswordService _passwordService;
    private readonly TokenService _tokenService;

    public LoginCommandHandler(IUserRepo userRepository, PasswordService passwordService, TokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {

        var user = await _userRepository.GetUserByUsernameAsync(request.Username);

        if (user == null)
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }


        var isPasswordValid = _passwordService.VerifyPassword(user.Password, request.Password);

        if (!isPasswordValid)
        {
            throw new UnauthorizedAccessException("Invalid credentials.");
        }


        var token = _tokenService.GenerateToken(user);

        return new LoginResponse { Token = token };
    }
}
