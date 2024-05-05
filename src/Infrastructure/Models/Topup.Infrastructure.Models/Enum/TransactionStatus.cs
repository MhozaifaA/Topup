using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topup.Infrastructure.Models
{
    /// <summary>
    /// simple status
    /// </summary>
    public enum TransactionStatus
    {
        /// <summary>
        /// Pending
        /// </summary>
        Pending,
        /// <summary>
        /// Authorized
        /// </summary>
        Authorized,
        /// <summary>
        /// Processing
        /// </summary>
        Processing,
        /// <summary>
        /// completed-Successful
        /// </summary>
        Completed,
        /// <summary>
        /// Failed
        /// </summary>
        Failed,
    }
}
