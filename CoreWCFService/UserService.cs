using Models.BaseType;
using Models.DTO;
using WebAPI;

namespace CoreWCFService
{
    public partial class Service
    {
        public async Task<ResponseBase<AuthenticatedResponse>> LoginUser(LoginModel model)
        {
            var response = await client.PostAsJsonAsync($"UserAuth/Login", model);
            var result = await response.Content.ReadAsAsync<ResponseBase<AuthenticatedResponse>>();
            return result;
        }
        public async Task<ResponseBase<UserModel>> DeleteUser(UserModel model)
        {
            var response = await client.PutAsJsonAsync($"User/Remove", model);
            var result = await response.Content.ReadAsAsync<ResponseBase<UserModel>>();
            return result;
        }
        public async Task<ResponseBase<AuthenticatedResponse>> RegisterUser(UserModel model)
        {
            var response = await client.PostAsJsonAsync($"UserAuth/Registration", model);
            var result = await response.Content.ReadAsAsync<ResponseBase<AuthenticatedResponse>>();
            return result;
        }
        public async Task<ResponseBase<UserModel>> UpdateUser(UserModel model)
        {
            var response = await client.PutAsJsonAsync($"User/Update", model);
            var result = await response.Content.ReadAsAsync<ResponseBase<UserModel>>();
            return result;
        }
    }
}
