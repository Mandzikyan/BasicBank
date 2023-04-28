using Models.BaseType;
using Models.DTO;

namespace CoreWCFService
{
    public partial class Service
    {
        public async Task<ResponseBase<CustomerModel>> InsertCustomer(CustomerModel model)
        {
            var response = await client.PostAsJsonAsync($"Customer/Post", model);
            var result = await response.Content.ReadAsAsync<ResponseBase<CustomerModel>>();
            return result;
        }
        public async Task<ResponseBase<CustomerModel>> GetCustomerById(int id)
        {
            var response = await client.GetAsync($"Customer/GetById?id={id}");
            var result = await response.Content.ReadAsAsync<ResponseBase<CustomerModel>>();
            return result;
        }
        public async Task<ResponseBase<CustomerModel>> DeleteCustomer(string passport)
        {
            var response = await client.DeleteAsync($"Customer/Remove?passport={passport}");
            var result = await response.Content.ReadAsAsync<ResponseBase<CustomerModel>>();
            return result;
        }
        public async Task<ResponseBase<CustomerModel>> UpdateCustomer(CustomerModel model)
        {
            var response = await client.PutAsJsonAsync($"Customer/Update", model);
            var result = await response.Content.ReadAsAsync<ResponseBase<CustomerModel>>();
            return result;
        }
        public async Task<ResponseBase<List<CustomerModel>>> GetAllCustomers()
        {
            var response = await client.GetAsync("Customer/GetAll");
            var result = await response.Content.ReadAsAsync<ResponseBase<List<CustomerModel>>>();
            return result;
        }
        public async Task<ResponseBase<CustomerModel>> GetCustomerByPassport(string passport)
        {
            var response = await client.GetAsync($"Customer/GetbyPassport?passport={passport}");
            var result = await response.Content.ReadAsAsync<ResponseBase<CustomerModel>>();
            return result;
        }
        public async Task<ResponseBase<IEnumerable<CustomerModel>>> FilterCustomers(FiltrationModel filtrationModel)
        {
            var response = await client.PostAsJsonAsync($"Customer/CustomerFiltration", filtrationModel);
            var result = await response.Content.ReadAsAsync<ResponseBase<IEnumerable<CustomerModel>>>();
            return result;
        }
    }
}
