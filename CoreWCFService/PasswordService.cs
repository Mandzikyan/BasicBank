using Models.BaseType;
using Models.ChangePassword;
using Models.DTO;
using WebAPI;

namespace CoreWCFService
{
    public partial class Service
    {
        public async Task<ResponseBase<int>> MailConfirm(string mail)
        {
            var response = await client.GetAsync($"ChangePassword/User/MailConfirm?mail={mail}");
            var result = await response.Content.ReadAsAsync<ResponseBase<int>>();
            return result;
        }

        public async Task<ResponseBase<int>> VerificationCodeConfirm(int generatedCode, string insertedCode)
        {
            var response = await client.GetAsync($"ChangePassword/User/VerificationCodeConfirm?generatedCode={generatedCode}&insertedCode={insertedCode}");
            var result = await response.Content.ReadAsAsync<ResponseBase<int>>();
            return result;
        }

        public async Task<ResponseBase<LoginModel>> RecoverPassword(string mail, NewPassword newPassword)
        {
            var response = await client.PostAsJsonAsync($"ChangePassword/User/RecoverPassword?mail={mail}",newPassword);
            var result = await response.Content.ReadAsAsync<ResponseBase<LoginModel>>();
            return result;
        }

        public async Task<ResponseBase<string>> UpdatePassword(string currentPassword, NewPassword newPassword)
        {
            var response = await client.PostAsJsonAsync($"ChangePassword/User/UpdatePassword?currentPassword={currentPassword}", newPassword);
            var result = await response.Content.ReadAsAsync<ResponseBase<string>>();
            return result;
        }
    }
}
