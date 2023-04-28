using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Validationn.Interfaces
{
    public interface IValidation
    {
        public bool EmailValidation(string mail);
        public bool AgeValidation(DateTime age);
        public bool PhoneValidation(string number);
        public bool PasswordValidation(string password);
        public bool NameValidation(string firstName, string lastName);
    }
}
