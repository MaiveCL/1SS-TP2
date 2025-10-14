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
using TP2.ViewModels;

namespace TP2.Views
{
    /// <summary>
    /// Logique d'interaction pour AccountStatusWindow.xaml
    /// </summary>
    public partial class AccountStatusWindow : Window
    {
        public AccountStatusWindow()
        {
            InitializeComponent();

            // Passe un delegate qui ferme la fenêtre
            this.DataContext = new AccountStatusViewModel(FermerFenetre);
            // delegate qui appelle Close() sur la fenêtre.
            // ConfigWindowViewModel reçoit ce delegate et le stocke dans _fermerFenetre du AccountStatusViewModel.cs
            // Avantage : le ViewModel n’a pas besoin de connaître directement la fenêtre, respectant le pattern MVVM.
        }
        private void FermerFenetre()
        {
            this.Close();
        }
    }
}
