using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32; // OpenFileDialog
using TP2.ViewModels.Commands;
using System.Windows;
using TP2.Views; // pour ConfigWindow

namespace TP2.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public RelayCommand CmdOuvrirConfig { get; private set; }

        public RelayCommand CmdOuvrirStatut { get; private set; }

        public MainViewModel()
        {
            CmdOuvrirConfig = new RelayCommand(OuvrirConfig, null);
            CmdOuvrirStatut = new RelayCommand(OuvrirStatut, null);
        }

        private void OuvrirConfig(object? obj)
        {
            // Crée et ouvre la fenêtre ConfigWindow en modal
            var configWindow = new ConfigWindow();

            // Optionnel : si tu veux centrer par rapport à la MainWindow
            if (Application.Current.MainWindow != null)
            {
                configWindow.Owner = Application.Current.MainWindow;
            }

            configWindow.ShowDialog(); // fenêtre modale
        }

        private void OuvrirStatut(object? obj)
        {
            // Crée et ouvre la fenêtre ConfigWindow en modal
            var statutWindow = new AccountStatusWindow();

            // Optionnel : si tu veux centrer par rapport à la MainWindow
            if (Application.Current.MainWindow != null)
            {
                statutWindow.Owner = Application.Current.MainWindow;
            }

            statutWindow.ShowDialog(); // fenêtre modale
        }
    }
}
