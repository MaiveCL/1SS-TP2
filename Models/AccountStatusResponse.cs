using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TP2.Models
{
    public class AccountStatusResponse
    {
        [JsonProperty("data")]
        public AccountStatus Data { get; set; }
    }
}
