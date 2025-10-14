using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TP2.Properties; // pour Settings.Default
using TP2.ViewModels.Commands;

namespace TP2.ViewModels
{
    public class ConfigWindowViewModel : BaseViewModel
    {

        public RelayCommand CmdSave { get; private set; }
        public RelayCommand CmdCancel { get; private set; }

        // Delegate pour fermer la fenêtre
        private readonly Action _fermerFenetre;

        private string _apiToken;
        public string ApiToken
        {
            get => _apiToken;
            set
            {
                if (_apiToken != value)
                {
                    _apiToken = value;
                    OnPropertyChanged();
                }
            }
        }

        public ConfigWindowViewModel(Action fermerFenetre)
        {
            _fermerFenetre = fermerFenetre;
            //LanguesDisponibles = new ObservableCollection<string> { "Fr", "En" };

            //LangueSelectionnee = Settings.Default.langue;
            //RedemarrerAuto = Settings.Default.RedemarrerAuto;

            // Charger le token existant depuis Settings
            ApiToken = Settings.Default.ApiToken;

            // Commands avec delegates
            CmdSave = new RelayCommand(OnSave, null);
            CmdCancel = new RelayCommand(OnCancel, null);
        }
        private void OnSave(object? obj)
        {
            // Ici on écrit directement dans Settings.Default
            Settings.Default.ApiToken = ApiToken;
            Settings.Default.Save();

            _fermerFenetre?.Invoke();

            // FUTUR : remplacer Settings.Default par un service
            // Exemple : ISettingsService _settings;
            // _settings.ApiToken = ApiToken;
            // _settings.Save();
            // Avantages : testable, découplé, facile à changer de backend.

            //    public interface ISettingsService
            //  {
            //      string ApiToken { get; set; }
            //      void Save();
            //  }

            //    public class SettingsService : ISettingsService
            //      {
            //          public string ApiToken
            //          {
            //              get => Settings.Default.ApiToken;
            //              set => Settings.Default.ApiToken = value;
            //          }

            //          public void Save() => Settings.Default.Save();
            //      }

        //   private readonly ISettingsService _settings;
        //   public ConfigWindowViewModel(ISettingsService settings, Action fermerFenetre)
        //      {
        //      _settings = settings;
        //      ApiToken = _settings.ApiToken;
        //      }

        }

        private void OnCancel(object? obj)
        {
            _fermerFenetre?.Invoke(); // j'ai copié la syntaxe vu dans les délegates datant d'avant qu'on sache que c'est des delegate
        }
    }
}
