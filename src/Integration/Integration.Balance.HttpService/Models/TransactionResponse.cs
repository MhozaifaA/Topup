using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Integration.Balance.HttpService
{
    public class TransactionResponse
    {
        [JsonPropertyName("transactionReference")]
        public string TransactionReference { get; set; }

        [JsonPropertyName("date")]
        public DateTimeOffset Date { get; set; }

        [JsonPropertyName("status")]
        public int Status { get; set; }

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("currentBalance")]
        public decimal CurrentBalance { get; set; }


        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
