using CoreWCFService;
using GalaSoft.MvvmLight.Command;
using Models.ChangePassword;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FcBankClient.ViewModels
{
    internal class RecoverPasswordViewModel : ViewModelBase
    {
        Service serviceWCF = new Service();

        private string email;
        private string password;
        private string repeatPassword;
        private bool passwordIsValid = false;
        private bool repeatPasswordIsValid = false;

        private string errorMessage;
        public ICommand ConfirmPasswordCommand { get; set; }

        public RecoverPasswordViewModel(string email)
        {
            this.email = email;
            ConfirmPasswordCommand = new RelayCommand(ConfirmPassword);
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
        public string RepeatPassword
        {
            get { return repeatPassword; }
            set
            {
                repeatPassword = value;
                OnPropertyChanged(nameof(RepeatPassword));
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
                    case "Password":
                        if (string.IsNullOrEmpty(Password))
                        {
                            error = "Password is required";
                            passwordIsValid = false;
                        }
                        else if (password.Contains(" "))
                        {
                            error = "Password cannot contain spaces";
                            passwordIsValid = false;
                        }
                        else
                        {
                            passwordIsValid = true;
                        }
                        break;
                    case "RepeatPassword":
                        if (password != repeatPassword)
                        {
                            error = "Passwords does not match";
                            repeatPasswordIsValid = false;
                        }
                        else
                        {
                            repeatPasswordIsValid = true;
                        }
                        break;
                }
                return error;
            }
        }

        private async void ConfirmPassword()
        {
            if (passwordIsValid && repeatPasswordIsValid && repeatPassword == password)
            {
                NewPassword newPassword = new NewPassword
                {
                    Password = Password,
                    RepeatPassword = RepeatPassword
                };
                var recoverPassword = await serviceWCF.RecoverPassword(email, newPassword);
                if (!recoverPassword.Success)
                {
                    ErrorMessage = recoverPassword.Message;
                }
                else
                {
                    var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(x => x.IsActive);
                    LoginView loginWindow = new LoginView();
                    currentWindow.Close();
                    loginWindow.Show();
                }
            }
            else
            {
                MessageBox.Show("The passwords fields must be same\nLength must be more then 8 symbols" +
                "\nPassword must contain letters,numbers and special character");
            }
        }
    }
}
