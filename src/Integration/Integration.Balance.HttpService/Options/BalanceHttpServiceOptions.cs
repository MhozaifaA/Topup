using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Balance.HttpService
{
    /// <summary>
    /// section name <see cref="_section"/> <code>BalanceHttpService</code>
    /// </summary>
    public class BalanceHttpServiceOptions
    {
        internal const string _section = "BalanceHttpService";
        public string BaseUrl { get; set; }
    }

    internal class Routes
    {
        public const string CurrentBalanceUrl = "/api/Balance/CurrentBalance";
        public const string DebitUrl = "/api/Balance/Debit";
        public const string ConfirmDebitTransactionUrl = "/api/Balance/ConfirmDebitTransaction";
    }
}
