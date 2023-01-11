using Entity.DTOModels;
using Entity.Helper;
using Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
   
    
        public interface ITransaction
        {
            Task<PageList<TransactionUserViewDto>> GetAllTransactions(PaginatedParameters parameter);
            Task<bool> MakeDeposit(TransactionDto deposit, string walletAddress);
            Task<bool> MakeTransfer(double transferAmount, string description, string senderWalletAdress, string receivingWalletAddress);
            Task<bool> MakeWithdrawal(TransactionDto withdraw, string walletaddress);

            Task<PageList<StatementofTransactionDto>> GetAllTransactionByWalletAddress(PaginatedParameters parameter, string walletaddress);
            Task<BalanceDto> GetWalletDetails(string Currency, string walletAddress);
            Task<Double> getApi(string currency);


        }
    
}
