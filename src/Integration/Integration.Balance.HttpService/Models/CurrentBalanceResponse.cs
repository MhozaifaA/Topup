using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Integration.Balance.HttpService
{
    public class CurrentBalanceResponse
    {
        [JsonPropertyName(name: "currentBalance")]
        public decimal CurrentBalance { get; set; }
    }
}
