using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP2.Models
{
    public class DetectionResult
    {
        public string Language_code { get; set; } = string.Empty;
        public double Confidence_score { get; set; }
        public bool Is_reliable { get; set; }
    }
}
