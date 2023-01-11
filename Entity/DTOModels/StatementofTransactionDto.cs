using Entity.Emuns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOModels
{
    public class StatementofTransactionDto
    {
        public string WalletAddress { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public string TransactionDate { get; set; }
        public string Description { get; set; }
        public Double TransactionAmount { get; set; }
    }
}
