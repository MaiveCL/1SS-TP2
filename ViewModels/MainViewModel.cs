using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32; // OpenFileDialog
using TP2.Models;
using TP2.ViewModels.Commands;
using TP2.Views; // pour ConfigWindow

namespace TP2.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public RelayCommand CmdOuvrirConfig { get; private set; }

        public RelayCommand CmdOuvrirStatut { get; private set; }
        public AsyncCommand CmdDetecterLangue { get; private set; }

        private string _texteAAnalyser = string.Empty;
        public string TexteAAnalyser
        {
            get => _texteAAnalyser;
            set
            {
                _texteAAnalyser = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PeutDetecter));
            }
        }
        public bool PeutDetecter => !string.IsNullOrWhiteSpace(TexteAAnalyser) && !string.IsNullOrEmpty(Properties.Settings.Default.ApiToken);
        public ObservableCollection<DetectionResult> ResultatsDetection { get; set; } = new ObservableCollection<DetectionResult>();

        private DetectionResult? _detectionSelectionnee;
        public DetectionResult? DetectionSelectionnee
        {
            get => _detectionSelectionnee;
            set { _detectionSelectionnee = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            CmdOuvrirConfig = new RelayCommand(OuvrirConfig, null);
            CmdOuvrirStatut = new RelayCommand(OuvrirStatut, null);
            CmdDetecterLangue = new AsyncCommand(async _ => await DetecterLangueAsync(), _ => PeutDetecter);
        }

        private void OuvrirConfig(object? obj)
        {
            // Crée et ouvre la fenêtre ConfigWindow en modal
            var configWindow = new ConfigWindow();

            // Optionnel, centrer par rapport à la MainWindow
            if (Application.Current.MainWindow != null)
            {
                configWindow.Owner = Application.Current.MainWindow;
            }

            configWindow.ShowDialog(); // fenêtre modale
            OnPropertyChanged(nameof(PeutDetecter));
        }

        private void OuvrirStatut(object? obj)
        {
            // Crée et ouvre la fenêtre ConfigWindow en modal
            var statutWindow = new AccountStatusWindow();

            // Optionnel : centrer par rapport à la MainWindow
            if (Application.Current.MainWindow != null)
            {
                statutWindow.Owner = Application.Current.MainWindow;
            }

            statutWindow.ShowDialog(); // fenêtre modale
        }
    }
}
