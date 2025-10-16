using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TP2.ViewModels;

namespace TP2.Models
{
    public class DetectionResult : BaseViewModel
    {
        private string _language = string.Empty;
        public string Language
        {
            get => _language;
            set
            {
                _language = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(LangueComplete));
            }
        }

        private double _confidence;
        public double Confidence
        {
            get => _confidence;
            set
            {
                _confidence = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Confiance));
            }
        }

        private bool _isReliable;
        public bool IsReliable
        {
            get => _isReliable;
            set
            {
                _isReliable = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(EstFiable));
            }
        }

        public string LangueComplete
        {
            get
            {
                var langue = ListeLangues.Instance.GetLanguageByCode(Language);
                return langue?.Name ?? Language;
            }
        }

        public string Confiance => Confidence.ToString("F2");
        public string EstFiable => IsReliable ? "Oui" : "Non";
    }
}
