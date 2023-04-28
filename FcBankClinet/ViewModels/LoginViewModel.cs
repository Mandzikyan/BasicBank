using GalaSoft.MvvmLight.Command;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Models.DTO;
using CoreWCFService;
using System;
using System.Text.RegularExpressions;

namespace FcBankClient.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private Service serviceWCF = new Service();
        private string email;
        private string password;
        private bool emailIsValid = false;
        private bool passwordIsValid = false;
        private string errorMessage;
        public ICommand LoginCommand { get; set; }
        public ICommand RegistrationWindowCommand { get; set; }
        public ICommand ForgetPasswordCommand { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
            RegistrationWindowCommand = new RelayCommand(RegistrationWindow);
            ForgetPasswordCommand = new RelayCommand(ForgetPasswordWindow);
        }

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }


        public override string this[string columnName]
        {
            get
            {
                string error = base[columnName];

                switch (columnName)
                {
                    case "Email":
                        if (string.IsNullOrEmpty(Email))
                        {
                            error = "Email is required";
                        }
                        else if (!Regex.IsMatch(Email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                        {
                            error = "Email is invalid";
                        }
                        else
                        {
                            emailIsValid = true;
                        }
                        break;
                    case "Password":
                        if (string.IsNullOrEmpty(Password))
                        {
                            error = "Password is required";
                        }
                        else if (Password.Contains(" "))

                        {
                            error = "Password cannot contain spaces";
                        }
                        else
                        {
                            passwordIsValid = true;
                        }
                        break;
                }
                return error;
            }
        }
        private async void Login()
        {
            LoginModel loginModel = new LoginModel
            {
                Email = Email,
                Password = Password
            };

            try
            {
                var response = await serviceWCF.LoginUser(loginModel);
                if (response.Success)
                {
                    var CurrentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
                    FiltrationView customerWindpw = new FiltrationView();
                    CurrentWindow.Close();
                    customerWindpw.Show();
                }
                else
                {
                    MessageBox.Show("User Login failed: " + response.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while logging the user:User not found");
            }

        }

        private void RegistrationWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            RegistrationView registrationWindow = new RegistrationView();
            currentWindow.Close();
            registrationWindow.Show();

        }

        private async void ForgetPasswordWindow()
        {
            if (emailIsValid)
            {
                var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
                var confirmationCode = await serviceWCF.MailConfirm(Email);
                if (!confirmationCode.Success)
                {
                    ErrorMessage = confirmationCode.Message;
                }
                else
                {
                    ForgetPasswordView forgetPasswordWindow = new ForgetPasswordView(Email, confirmationCode.Data);
                    currentWindow.Close();
                    MessageBox.Show("Confirm Code: " + confirmationCode.Data.ToString());
                    forgetPasswordWindow.Show();
                }
            }
        }
    }
}
