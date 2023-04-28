using FcBankClient.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FcBankClient
{
    /// <summary>
    /// Interaction logic for RecoverPasswordView.xaml
    /// </summary>
    public partial class RecoverPasswordView : Window
    {
        public RecoverPasswordView(string email)
        {
            InitializeComponent();
            DataContext = new RecoverPasswordViewModel(email);
        }
    }
}
