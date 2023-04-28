﻿using FCBankBasicHelper.Models;
using Models.ChangePassword;

namespace BL.Repositories.Interfaceis
{
    public interface IUserRepository : IRepository<User>
    {
        User Login(string email, string password);
        void RemoveUser(string email);
        void UpdateUser(User model);
        bool UserExists(string username);
        User GetUserByEmail(string mail);
        User GetUserById(int id);
        bool ChangePassword(int id, NewPassword newPassword);
    }
}
