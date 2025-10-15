using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TP2.Models;
using TP2.Properties;
using TP2.ViewModels.Commands;

namespace TP2.ViewModels
{
    public class AccountStatusViewModel : BaseViewModel
    {
        private readonly Action _fermerFenetre;

        public RelayCommand CmdFermer { get; private set; }
        public AsyncCommand CmdChargerStatut { get; private set; }

        private AccountStatus _statut;
        public AccountStatus Statut
        {
            get => _statut;
            set
            {
                _statut = value;
                OnPropertyChanged();
            }
        }

        private string _messageErreur;
        public string MessageErreur
        {
            get => _messageErreur;
            set
            {
                _messageErreur = value;
                OnPropertyChanged();
            }
        }

        public AccountStatusViewModel(Action fermerFenetre, bool autoCharger = true)
        {
            _fermerFenetre = fermerFenetre;
            CmdFermer = new RelayCommand(OnFermer, null);
            CmdChargerStatut = new AsyncCommand(async _ => await ChargerStatutAsync(), null);
            Statut = new AccountStatus(); // Initialiser pour éviter les null refs
            MessageErreur = string.Empty;

            if (autoCharger)
            {
                // Charger automatiquement le statut au démarrage
                _ = ChargerStatutAsync();
            }
        }

        private void OnFermer(object? obj) => _fermerFenetre?.Invoke();

        private async Task ChargerStatutAsync()
        {
            MessageErreur = "";

            string token = Settings.Default.ApiToken;

            if (string.IsNullOrWhiteSpace(token))
            {
                MessageErreur = "Aucun jeton n’est configuré.";
                return;
            }

            try
            {
                using ApiClient client = new ApiClient("https://ws.detectlanguage.com/0.2");
                client.SetHttpRequestHeader("Authorization", "Bearer " + token);

                string json = await client.RequeteGetAsync("/user/status");

                if (string.IsNullOrWhiteSpace(json))
                {
                    MessageErreur = "Aucune réponse de l’API.";

                    MessageBox.Show(
                        MessageErreur,
                        "Message du programme",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );

                    return;
                }

                if (json.Contains("\"error\""))
                {
                    var wrapper = JsonConvert.DeserializeObject<ApiErrorWrapper>(json);
                    Statut = new AccountStatus();
                    MessageErreur = $"Erreur {wrapper?.Error?.Code} : {wrapper?.Error?.Message}";

                    // ajouter une alerte aussi
                    MessageBox.Show(
                        MessageErreur,
                        "Message de votre API",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );

                    return;
                }

                var reponse = JsonConvert.DeserializeObject<AccountStatus>(json);
                Statut = reponse ?? new AccountStatus();
            }
            catch (Exception ex)
            {
                MessageErreur = "Erreur : " + ex.Message;
            }
        }
    }
}
