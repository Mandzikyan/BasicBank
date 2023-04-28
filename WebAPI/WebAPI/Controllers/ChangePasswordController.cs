using BL.Core;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Models.ChangePassword;
using BL.MailConfirmation;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ChangePasswordController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserBL userBl;
        private readonly ILogger<ChangePasswordController> logger;
        public int ActiveCode;

        public ChangePasswordController(UserBL userBl, IHttpContextAccessor httpContextAccessor,ILogger<ChangePasswordController> logger)
        {
            this.userBl = userBl;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
        }      

        [HttpGet]
        [Route("User/MailConfirm")]
        public async Task<IActionResult> MailConfirm(string mail)
        {
            var response = userBl.UserMailConfirm(mail);
            return Ok(response);
        }

        [HttpGet]
        [Route("User/VerificationCodeConfirm")]
        public async Task<IActionResult> VerificationCodeConfirm(int generatedCode, string insertedCode)
        {
            return Ok(userBl.VerificationCodeConfirm(generatedCode, insertedCode));
        }

        [HttpPost]
        [Route("User/RecoverPassword")]
        public async Task<IActionResult> RecoverPassword(string mail, NewPassword newPassword)
        {
            return Ok(userBl.RecoverPassword(mail, newPassword));
        }

        [HttpPost]
        [Route("User/UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(string currentPassword,[FromBody] NewPassword newPassword)
        {
            var principal = httpContextAccessor.HttpContext.User;
            return Ok(userBl.UpdatePassword(currentPassword, newPassword, principal));
        }
    }
}
