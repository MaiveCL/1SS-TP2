using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TP2.Properties;

namespace TP2.Models
{
    public class ListeLangues
    {
        private static ListeLangues? _instance;
        public static ListeLangues Instance => _instance ??= new ListeLangues();

        private readonly Dictionary<string, Langue> _listeLangues = new();

        public Langue? GetLanguageByCode(string code)
        {
            if (_listeLangues.TryGetValue(code, out var langue))
                return langue;
            return null;
        }
        private ListeLangues() { }

        public async Task ChargerAsync()
        {
            if (_listeLangues.Count > 0) return; // déjà chargé

            using var client = new ApiClient("https://ws.detectlanguage.com/0.2");
            string token = Settings.Default.ApiToken;
            if (!string.IsNullOrWhiteSpace(token))
                client.SetHttpRequestHeader("Authorization", "Bearer " + token);

            string json = await client.RequeteGetAsync("/languages");

            if (!string.IsNullOrWhiteSpace(json))
            {
                // Désérialisation directe en List<Langue>
                var liste = JsonConvert.DeserializeObject<List<Langue>>(json);
                if (liste != null)
                { 
                    // FONCTIONNEMENT DICTIONAIRE
                    // Pour chaque langue, utiliser son code comme clé et stocker l'objet complet comme valeur
                    foreach (var objetValeur in liste)
                    {
                        var clef = objetValeur.Code;
                        _listeLangues[clef] = objetValeur;
                    }
                    //foreach (var l in liste)
                    //    _listeLangues[l.Code] = l;
                }
            }
        }
    }
}
