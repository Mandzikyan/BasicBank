using CoreWCFService;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FcBankClient.ViewModels
{
    internal class ForgetPasswordViewModel : ViewModelBase
    {
        Service serviceWCF = new Service();

        private string email;
        private int generatedCode;
        private string verificationCode;
        public ICommand InsertVerificationCodeCommand { get; set; }

        public ForgetPasswordViewModel(string email, int generatedCode)
        {
            this.email = email;
            this.generatedCode = generatedCode;
            InsertVerificationCodeCommand = new RelayCommand(InsertVerificationCode);
        }

        public string VerificationCode
        {
            get { return verificationCode; }
            set
            {
                verificationCode = value;
                OnPropertyChanged(nameof(VerificationCode));
            }
        }

        private async void InsertVerificationCode()
        {           
            var currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            var confirmationStatus = await serviceWCF.VerificationCodeConfirm(generatedCode, VerificationCode);
            RecoverPasswordView recoverPasswordWindow = new RecoverPasswordView(email);
            LoginView loginWindow = new LoginView();
            currentWindow.Close();

            if (confirmationStatus.Success)
            {
                recoverPasswordWindow.Show();
            }
            else
            {
                loginWindow.Show();
            }
        }
    }
}
