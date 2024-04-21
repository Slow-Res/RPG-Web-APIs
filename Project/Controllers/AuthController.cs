using Microsoft.AspNetCore.Mvc;
using Project.DTOs.User;

namespace Project.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class AuthController: ControllerBase
    {
        public  IAuthRepository _authRepository { get; }
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDTO user)
        {
          var result = await _authRepository.Register( new User { UserName = user.UserName} , user.Password );
          return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDTO user)
        {
          var result = await _authRepository.Login( user.UserName , user.Password );
          return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}