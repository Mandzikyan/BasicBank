
using BL.Encrypt;
using BL.Hashing;
using BL.MailConfirmation;
using BL.Repositories.Interfaceis;
using BL.Repository;
using BL.Validationn.Interfaces;
using FCBankBasicHelper.Models;
using Models.BaseType;
using Models.ChangePassword;
using Models.DTO;
using Models.Mappers;
using System.Security.Claims;

namespace BL.Core
{
    public class UserBL
    {
        private readonly IUserRepository userRepository;
        private readonly IValidation validation;
        private readonly IEncryption encryption;
        private readonly VerificationCode verificationCode;

        public UserBL(IValidation validation, IEncryption encryption , IUserRepository userRepository, VerificationCode verificationCode)
        {
            this.validation =validation;
            this.encryption= encryption;
            this.userRepository=userRepository;
            this.verificationCode=verificationCode;
        }
        private string[] GetRolesForUser(string username)
        {
            string[] userRoles;
            username = encryption.Encrypt(username);
            using (FcbankBasicContext context = new FcbankBasicContext())
            {
                userRoles = (from user in context.Users
                             join roleMapping in context.UserRolesMappings
                             on user.Id equals roleMapping.UserId
                             join role in context.Roles
                             on roleMapping.RoleId equals role.Id
                             where user.Username == username
                             select role.RoleName).ToArray();
            }
            return userRoles;
        }
        public bool HasPermission(string username, params string[] roles)
        {
            var userRoles = GetRolesForUser(username);
            var innerjoin = userRoles.Join(roles, str1 => str1,
                      str2 => str2,
                      (str1, str2) => str1);
            if (innerjoin.Count() == 0) return false;
            return true;
        }
        public ResponseBase<UserModel> InsertUser(UserModel model)
        {
                try
                {
                    User user = Mapper<UserModel, User>.Map(model);
                    if (user == null && !UserValidation(user)) throw new Exception("Invalid User");
                    encryption.EncryptData(user);
                    user.Password = PasswordHash.Hashed_Password(user.Password);
                    userRepository.Add(user);
                    return new ResponseBase<UserModel>(true, "User added Successfully", model);
                }
                catch (Exception ex)
                {
                    return new ResponseBase<UserModel>(false, ex.Message);
                }
        }
        public ResponseBase<UserModel> GetUser(string mail, string password)
        {
            try
            {
                User login = userRepository.Login(encryption.Encrypt(mail), password);
                if (login is null) throw new Exception("User not found");
                encryption.DecryptData(login);
                var loginmodel = Mapper<User, UserModel>.Map(login);
                return new ResponseBase<UserModel>(true, "User getted Successfully", loginmodel);
            }
            catch (Exception ex)
            {
                return new ResponseBase<UserModel>(false, ex.Message);
            }
        }
        public ResponseBase<UserModel> RemoveUser(string username)
        {
                try
                {
                    userRepository.RemoveUser(encryption.Encrypt(username));
                    return new ResponseBase<UserModel>(true, "User removed Successfully");
                }
                catch (Exception ex)
                {
                    return new ResponseBase<UserModel>(false, ex.Message);
                }
        }
        public ResponseBase<UserModel> UpdateUser(UserModel model)
        {
                try
                {
                    User user = Mapper<UserModel, User>.Map(model);
                    if (user == null && !UserValidation(user)) throw new Exception("Invalid user");
                    encryption.EncryptData(user);
                    user.Password = PasswordHash.Hashed_Password(user.Password);
                    userRepository.Update(user);
                    return new ResponseBase<UserModel>(true, "User ipdated Successfully", model);
                }
                catch (Exception ex)
                {
                    return new ResponseBase<UserModel>(false, ex.Message);
                }
        }
        public bool UserExists(string username)
        {
            try
            {
                return userRepository.UserExists(encryption.Encrypt(username));
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ResponseBase<UserModel> GetUserByEmail(string mail)
        {
            try
            {
                mail = encryption.Encrypt(mail);
                var result = userRepository.GetUserByEmail(mail);
                encryption.DecryptData(result);
                return new ResponseBase<UserModel>(true, "User ipdated Successfully", Mapper<User, UserModel>.Map(result));
            }
            catch (Exception ex)
            {
                return new ResponseBase<UserModel>(false, ex.Message);
            }
        }    
        public ResponseBase<UserModel> GetUserById(int id)
        {
            try
            {
                var result = userRepository.GetUserById(id);
                if (result == null) throw new Exception("User not found");
                encryption.DecryptData(result);
                return new ResponseBase<UserModel>(true, "User getted Successfully", Mapper<User, UserModel>.Map(result));
            }
            catch (Exception ex)
            {
                return new ResponseBase<UserModel>(false, ex.Message);
            }
        }
        public bool UserValidation(User userInfo)
        {
            return  validation.NameValidation(userInfo.FirstName, userInfo.LastName) &&
                    validation.PasswordValidation(userInfo.Password) &&
                    validation.EmailValidation(userInfo.Email) &&
                    validation.AgeValidation(userInfo.Birthday);
        }
        public ResponseBase<int> UserMailConfirm(string mail)
        {
            try
            {
                var findUserMail = GetUserByEmail(mail);
                if (!findUserMail.Success)
                {
                    return new ResponseBase<int>(false, "Failed to find user by email");
                }
                return new ResponseBase<int>(true, "Mail confirmed", verificationCode.MailConfirm(mail));
            }
            catch
            {
                return new ResponseBase<int>(false, "Email confirmation failed");
            }
        }

        public ResponseBase<int> VerificationCodeConfirm(int generatedCode, string insertedCode)
        {
            try
            {
                bool verificationCodeConfirmed = verificationCode.VerificationCodeConfirm(generatedCode, insertedCode);
                if (!verificationCodeConfirmed)
                {
                    return new ResponseBase<int>(false, "Incorrect verification code");
                }
                return new ResponseBase<int>(true, "Code confirmed");
            }
            catch
            {
                return new ResponseBase<int>(false, "Code confirmation failed");
            }
        }

        public ResponseBase<LoginModel> RecoverPassword(string mail, NewPassword newPassword)
        {
            try
            {
                var user = GetUserByEmail(mail);
                if (!user.Success)
                {
                    return new ResponseBase<LoginModel>(false, "Failed to find user by email");
                }
                bool passwordChanged = userRepository.ChangePassword(user.Data.Id, newPassword);
                if (!passwordChanged)
                {
                    return new ResponseBase<LoginModel>(false, "Failed to change password");
                }
                LoginModel loginModel = new LoginModel
                {
                    Email = mail,
                    Password = newPassword.Password
                };
                return new ResponseBase<LoginModel>(true, "Password recovered", loginModel);
            }
            catch
            {
                return new ResponseBase<LoginModel>(false, "Password recover failed");
            }
        }
        public ResponseBase<string> UpdatePassword(string currentPassword, NewPassword newPassword, ClaimsPrincipal principal)
        {
            try
            {
                int userId = Convert.ToInt32(principal.FindFirst("id").Value);
                var user = GetUserById(userId);
                if (user.Data.Password != PasswordHash.Hashed_Password(currentPassword))
                {
                    return new ResponseBase<string>(false, "Incorrect password");
                }
                bool passwordChanged = userRepository.ChangePassword(user.Data.Id, newPassword);
                if (!passwordChanged)
                {
                    return new ResponseBase<string>(false, "Failed to change password");
                }
                return new ResponseBase<string>(true, "Password updated");
            }
            catch
            {
                return new ResponseBase<string>(false, "Failed to update password");
            }
        }
    }
}
