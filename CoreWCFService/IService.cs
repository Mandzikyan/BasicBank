using Models.BaseType;
using Models.ChangePassword;
using Models.DTO;
using WebAPI;

namespace CoreWCFService
{

    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        Task<ResponseBase<CustomerModel>> GetCustomerByPassport(string passport);

        [OperationContract]
        Task<ResponseBase<List<CustomerModel>>> GetAllCustomers();

        [OperationContract]
        Task<ResponseBase<CustomerModel>> GetCustomerById(int id);

        [OperationContract]
        Task<ResponseBase<CustomerModel>> InsertCustomer(CustomerModel model);

        [OperationContract]
        Task<ResponseBase<CustomerModel>> UpdateCustomer(CustomerModel model);

        [OperationContract]
        Task<ResponseBase<CustomerModel>> DeleteCustomer(string passport);

        [OperationContract]
        Task<ResponseBase<IEnumerable<CustomerModel>>> FilterCustomers(FiltrationModel filtrationModel);

        [OperationContract]
        Task<ResponseBase<AuthenticatedResponse>> RegisterUser(UserModel model);

        [OperationContract]
        Task<ResponseBase<UserModel>> UpdateUser(UserModel model);

        [OperationContract]
        Task<ResponseBase<UserModel>> DeleteUser(UserModel model);

        [OperationContract]
        Task<ResponseBase<AuthenticatedResponse>> LoginUser(LoginModel model);

        [OperationContract]
        Task<ResponseBase<int>> MailConfirm(string mail);

        [OperationContract]
        Task<ResponseBase<int>> VerificationCodeConfirm(int generatedCode, string insertedCode);

        [OperationContract]
        Task<ResponseBase<LoginModel>> RecoverPassword(string mail, NewPassword newPassword);

        [OperationContract]
        Task<ResponseBase<string>> UpdatePassword(string currentPassword, NewPassword newPassword);
        [OperationContract]
        Task<ResponseBase<PhoneModel>> AddPhone(PhoneModel model);
        [OperationContract]
        Task<ResponseBase<PhoneModel>> UpdatePhone(PhoneModel model);
        [OperationContract]
        Task<ResponseBase<PhoneModel>> DeletePhone(PhoneModel model);
        [OperationContract]
        Task<ResponseBase<PhoneModel>> GetPhoneByUserId(int id);
    }
}