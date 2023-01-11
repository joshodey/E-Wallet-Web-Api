using Core.Services;
using Entity.DTOModels;
using Entity.Helper;
using Entity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using static System.Reflection.Metadata.BlobBuilder;

namespace WalletApp.Controllers
{
    [Route("api/v{versions:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    
    public class TransactionsController : ControllerBase
    {
        private readonly ITransaction _transaction;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        public TransactionsController(ITransaction transaction)
        {
            _transaction = transaction;
        }

        /// <summary>
        /// Get all Transactions in the database
        /// </summary>
        /// <param name="parameter">Return number items specified by a user</param>
        /// <returns> IActionResult</returns>
        [HttpGet("Get-Transactions")]
        
        public async Task<IActionResult> GetAllTransaction([FromQuery]PaginatedParameters parameter)
        {
            
            var Alltransactions = await _transaction.GetAllTransactions(parameter);
            if(Alltransactions== null)
                return NotFound();
            return Ok(APIListResponseDto.Success(Alltransactions, parameter.PageNumber, Alltransactions.MetaData.PageSize, Alltransactions.MetaData.TotalPages, Alltransactions.MetaData.TotalCount,
            Alltransactions.MetaData.HasPrevious, Alltransactions.MetaData.HasNext));
        }
        /// <summary>
        /// Enable User To Deposit fund into his / her wallet
        /// </summary>
        /// <param name="deposit"></param>
        /// <param name="walletaddress"></param>
        /// <returns>IActionResult</returns>
        [HttpPost("Deposit")]
        public async Task<IActionResult> Deposit([FromForm]TransactionDto deposit, string walletaddress)
        {
            var Alltransactions = await _transaction.MakeDeposit(deposit, walletaddress);
            return Ok(Alltransactions);
        }
        /// <summary>
        /// Remove fund from a wallet
        /// </summary>
        /// <param name="withdraw"></param>
        /// <param name="walletaddress"></param>
        /// <returns>IActionResult</returns>
        [HttpPost("Withdrawal")]
        public async Task<IActionResult> Withdrawal([FromForm] TransactionDto withdraw, string walletaddress)
        {
            var withdrawal = await _transaction.MakeWithdrawal(withdraw, walletaddress);

            return Ok("Successful");
        }
        /// <summary>
        /// Transfers fund from a wallet to another wallet using wallet addresses
        /// </summary>
        /// <param name="transferAmount"></param>
        /// <param name="description"></param>
        /// <param name="senderWalletAdress"></param>
        /// <param name="receivingWalletAddress"></param>
        /// <returns>IActionResult</returns>
        [HttpPost("Transfer")]
        public async Task<IActionResult> Transfer([FromQuery] double transferAmount, string description, string senderWalletAdress, string receivingWalletAddress)
        {
            var withdrawal = await _transaction.MakeTransfer(transferAmount, description, senderWalletAdress, receivingWalletAddress);

            return Ok("Successful");
        }
        /// <summary>
        /// Get wallet transaction history by wallet address
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="walletaddress"></param>
        /// <returns>IActionResult</returns>
        [HttpGet("Get-All-Transactions-By-WalletAddress")]
        public async Task<IActionResult> GetTransactionsByWalletAddress( [FromQuery]PaginatedParameters parameters,[FromQuery]string walletaddress)
        {
            var alltransactions = await _transaction.GetAllTransactionByWalletAddress( parameters,walletaddress);

            return alltransactions == null ? BadRequest($"No Transactions for this wallet {walletaddress}") : 
                 Ok(APIListResponseDto.Success(alltransactions, parameters.PageNumber, 
                alltransactions.MetaData.PageSize, alltransactions.MetaData.TotalPages, alltransactions.MetaData.TotalCount,
            alltransactions.MetaData.HasPrevious, alltransactions.MetaData.HasNext));
        }
        /// <summary>
        /// Get the wallet information and Balance in preferred currency
        /// </summary>
        /// <param name="currency"></param>
        /// <param name="walletaddress"></param>
        /// <returns>IActionResult<returns>
        [HttpGet("Retrieve-Wallet-Details")]
        public async Task<IActionResult> RetrieveWalletDetails([FromQuery]string currency, string walletaddress)
        {
            var walletdetails = await _transaction.GetWalletDetails(currency, walletaddress);

            return walletdetails == null? BadRequest("Wallet Details not Found") : Ok(walletdetails);
        }
    }
}
