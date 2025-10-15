using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP2.Models
{
    // Models/AccountStatus.cs
    public class AccountStatus
    {
        public string Date { get; set; }                  // "2025-10-14"
        public int Requests { get; set; }                // requêtes envoyées aujourd'hui
        public int Bytes { get; set; }                   // octets envoyés aujourd'hui
        public string Plan { get; set; }                 // ex: "FREE"
        public string Plan_expires { get; set; }          // date d'expiration ou null
        public int Daily_requests_limit { get; set; }      // limite quotidienne de requêtes
        public int Daily_bytes_limit { get; set; }         // limite quotidienne en octets
        public string Status { get; set; }               // "ACTIVE" ou "SUSPENDED"
        public ApiError? Error { get; set; } = null;
    }

}
