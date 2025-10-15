using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TP2.Models
{
    public class ApiErrorWrapper
    {
        public ApiError Error { get; set; } = new ApiError();
    }
}
