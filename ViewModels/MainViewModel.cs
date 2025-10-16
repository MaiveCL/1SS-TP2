using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32; // OpenFileDialog
using Newtonsoft.Json;
using TP2.Models;
using TP2.Properties;
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
        public bool PeutDetecter => !string.IsNullOrWhiteSpace(TexteAAnalyser);
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

        private async Task DetecterLangueAsync()
        {
            ResultatsDetection.Clear();
            DetectionSelectionnee = null;

            string token = Settings.Default.ApiToken;
            if (string.IsNullOrWhiteSpace(token))
            {
                MessageBox.Show("Aucun jeton configuré.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                // Charger la liste des langues si nécessaire
                await ListeLangues.Instance.ChargerAsync();

                using var client = new ApiClient("https://ws.detectlanguage.com/0.2");
                client.SetHttpRequestHeader("Authorization", "Bearer " + token);

                // Préparer JSON simple pour l'envoi (form-urlencoded)
                var payload = $"q={Uri.EscapeDataString(TexteAAnalyser)}";
                string json = await client.RequetePostFormUrlEncodedAsync("/detect", payload);

                // &&&&&&&&&&&&&&&&&&&&&&&&& Test brut pour vérifier le contenu 
                MessageBox.Show($"Texte envoyé : '{TexteAAnalyser}'", "DEBUG");

                // &&&&&&&&&&&&&&&&&&&&&&&&& Test brut pour vérifier le contenu 
                MessageBox.Show(json, "JSON brut reçu");

                if (string.IsNullOrWhiteSpace(json))
                {
                    MessageBox.Show("Aucune réponse de l'API.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (json.Contains("\"error\""))
                {
                    var wrapper = JsonConvert.DeserializeObject<ApiErrorWrapper>(json);
                    MessageBox.Show($"Erreur {wrapper?.Error?.Code} : {wrapper?.Error?.Message}", "Erreur API", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Parser la réponse
                var wrapperDetection = JsonConvert.DeserializeObject<DetectionFirstWrapper>(json);
                if (wrapperDetection?.Data?.Detections != null)
                {
                    ResultatsDetection = new ObservableCollection<DetectionResult>(wrapperDetection.Data.Detections);
                    OnPropertyChanged(nameof(ResultatsDetection));
                    DetectionSelectionnee = ResultatsDetection.FirstOrDefault();

                    // &&&&&&&&&&&&&&&&&&&&&&&&&&&&&&&& --- Test de contenu ---
                    if (ResultatsDetection.Count == 0)
                    {
                        MessageBox.Show("La collection ResultatsDetection est vide !");
                    }
                    else
                    {
                        var texte = string.Join(Environment.NewLine,
                            ResultatsDetection.Select(d => $"Langue: {d.Language}, LangueComplete: {d.LangueComplete}, Confiance: {d.Confiance}, Fiable: {d.EstFiable}"));
                        MessageBox.Show(texte, "Contenu de ResultatsDetection");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
