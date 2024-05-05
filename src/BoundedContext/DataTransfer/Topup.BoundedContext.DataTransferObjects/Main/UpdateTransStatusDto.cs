using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topup.Infrastructure.Models;

namespace Topup.BoundedContext.DataTransferObjects
{
    public class UpdateTransStatusDto
    {
        public required string TransactionReference { get; set; }

        public string? SourceReference { get; set; }

        public TransactionStatus Status { get; set; }
    }
}
