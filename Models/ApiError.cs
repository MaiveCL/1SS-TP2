using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TP2.Models
{
    public class ApiError
    {
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
