using Models.BaseType;
using Models.DTO;

namespace CoreWCFService
{
    public partial class Service
    {
        public async Task<ResponseBase<PhoneModel>> AddPhone(PhoneModel model)
        {
            var response = await client.PutAsJsonAsync($"Phone/Post", model);
            var result = await response.Content.ReadAsAsync<ResponseBase<PhoneModel>>();
            return result;
        }

        public async Task<ResponseBase<PhoneModel>> UpdatePhone(PhoneModel model)
        {
            var response = await client.PutAsJsonAsync($"Phone/Update", model);
            var result = await response.Content.ReadAsAsync<ResponseBase<PhoneModel>>();
            return result;
        }

        public async Task<ResponseBase<PhoneModel>> DeletePhone(PhoneModel model)
        {
            var response = await client.PutAsJsonAsync($"Phone/Remove", model);
            var result = await response.Content.ReadAsAsync<ResponseBase<PhoneModel>>();
            return result;
        }

        public async Task<ResponseBase<PhoneModel>> GetPhoneByUserId(int id)
        {
            var response = await client.GetAsync($"Phone/GetByUserId?userid={id}");
            var result = await response.Content.ReadAsAsync<ResponseBase<PhoneModel>>();
            return result;
        }
    }
}
