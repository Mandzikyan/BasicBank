using BL.Core;
using FCBankBasicHelper.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models.BaseType;
using Models.DTO;
using Serilog;
using WebAPI.Abstract;
using WebAPI.Domain;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAuthController : ControllerBase
    {
        private readonly TokenService tokenService;
        private readonly TokenBL tokenBL;
        private readonly UserBL userBl;
        private readonly ILogger<UserAuthController> logger;

        public UserAuthController(TokenService tokenService, ILogger<UserAuthController> logger, UserBL userBl, TokenBL tokenBL)
        {
            this.tokenService = tokenService;
            this.tokenBL = tokenBL;
            this.userBl = userBl;
            this.logger = logger;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            var model = userBl.GetUser(login.Email, login.Password);
            if (model is null)
            {
                throw new Exception("Mail or password is incorrect");
            }
            var authClaims = await tokenService.GetClaims(model.Data);
            var JWTtoken = await tokenService.GetToken(authClaims);
            var refreshToken = await tokenService.GetRefreshToken();
            tokenBL.Save(refreshToken, model.Data.Username);
            logger.LogInformation($"[Username:{model.Data.Username}] was logined seccessfully.");
           
            var response = new ResponseBase<AuthenticatedResponse>(true, "User loginned Successfully", new AuthenticatedResponse
            {
                Token = JWTtoken,
                RefreshToken = refreshToken
            });
            return Ok(response);
        }

        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Registration([FromBody] UserModel model)
        {
                var user = new IdentityUser
                {
                    UserName = model.Username,
                    Email = model.Email
                };
                var response = userBl.InsertUser(model);
                if (response.Success == false)
                {
                    throw new Exception(response.Message);
                }
                var authClaims = await tokenService.GetClaims(model);
                var token = await tokenService.GetToken(authClaims);
                var refreshToken = await tokenService.GetRefreshToken();
                tokenBL.Save(refreshToken, user.UserName);
                logger.LogInformation($"[Username:{model.Username}] was rigistered seccessfully.");
                var result = new ResponseBase<AuthenticatedResponse>(true, "User registered Successfully", new AuthenticatedResponse
                {
                    Token = token,
                    RefreshToken = refreshToken
                });
                return Ok(result);
            
            throw new Exception("Registration failed");
        }
    }
}
