using Entity.Emuns;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Models
{
    public class TransactionModel
    {
        [Key]
        public int Id { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public string TransactionDate { get; set; } = DateTime.Now.ToString();
        public string Description { get; set; }
        public Double TransactionAmount { get; set; }

        [ForeignKey(nameof(WalletAddress))]
        public int WalletAddressId { get; set; }
        public WalletModel WalletAddress { get; set; }

    }
}
