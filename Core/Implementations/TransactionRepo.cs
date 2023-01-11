using Core.Services;
using Entity.DTOModels;
using Entity.Emuns;
using Entity.Helper;
using Entity.Models;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.Http;
using System.Text.Json;

namespace Core.Implementations
{
    public class TransactionRepo : ITransaction
    {
        
        private readonly ApplicationContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILoggerManager _logger;

        public TransactionRepo(ApplicationContext context, IHttpClientFactory httpClientFactory,ILoggerManager logger)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }
        public async Task<PageList<TransactionUserViewDto>> GetAllTransactions(PaginatedParameters parameter)
        {
            
            try
            {
                var transactionList = await _context.Transactions.Select(x =>
                new TransactionUserViewDto
            {
                TransactionAmount = x.TransactionAmount,
                TransactionDate = x.TransactionDate,
                TransactionType = x.TransactionType,
                Description = x.Description,
            }).ToListAsync();
                return PageList<TransactionUserViewDto>.ToPagedList(transactionList, parameter.PageNumber, parameter.PageSize);
            }
            catch (Exception ex)
            {
                _logger.LogInfo($"{ex.InnerException}");
                _logger.LogError($"{ex.Message}");
                _logger.LogDebug($"{ex.Source}");
                _logger.LogWarn($"{ex.StackTrace}");

            }
            return null;                                                                                                                                                                                                                                                                                                                                
        }

        public async Task<bool> MakeDeposit(TransactionDto deposit, string walletAddress)
        {
            
            try
            {
                var myWallet = await _context.Wallets.FirstOrDefaultAsync(x => x.WalletAddress == walletAddress);
                if (myWallet == null)
                {
                   throw new ArgumentException(nameof(myWallet));
                }
                myWallet.WalletBalance += deposit.TransactionAmount;
                var transaction = new TransactionModel
                {
                    WalletAddress = myWallet,
                    TransactionAmount = deposit.TransactionAmount,
                    Description = deposit.Description,
                    TransactionDate = DateTime.Now.ToString("g"),
                    TransactionType = TransactionTypeEnum.Deposit
                };
                await _context.AddAsync(transaction);
                _context.Update(myWallet);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInfo($"{ex.InnerException}");
                _logger.LogError($"{ex.Message}");
                _logger.LogDebug($"{ex.Source}");
                _logger.LogWarn($"{ex.StackTrace}");

            }
            return false;
        }

        public async Task<bool> MakeTransfer(double transferAmount, string description, string senderWalletAdress, string receivingWalletAddress)
        {
            
            try
            {
                var senderWallet = await _context.Wallets.FirstOrDefaultAsync(x => x.WalletAddress == senderWalletAdress);
                var receiverWallet = await _context.Wallets.FirstOrDefaultAsync(x => x.WalletAddress == receivingWalletAddress);
                if (senderWallet == null || receiverWallet == null)
                {
                    throw new ArgumentException(nameof(senderWallet));
                    throw new ArgumentException(nameof(receiverWallet));
                }
                if (senderWallet.WalletBalance < transferAmount)
                {
                    return false;
                }
                senderWallet.WalletBalance -= transferAmount;
                receiverWallet.WalletBalance += transferAmount;
                var sendertransaction = new TransactionModel
                {
                    WalletAddress = senderWallet,
                    TransactionAmount = transferAmount,
                    Description = description,
                    TransactionDate = DateTime.Now.ToString("g"),
                    TransactionType = TransactionTypeEnum.Tranfer
                };
                var recievertransaction = new TransactionModel
                {
                    WalletAddress = receiverWallet,
                    TransactionAmount = transferAmount,
                    Description = description,
                    TransactionDate = DateTime.Now.ToString("g"),
                    TransactionType = TransactionTypeEnum.Credit
                };

                await _context.AddAsync(recievertransaction);
                _context.Update(receiverWallet);
                await _context.SaveChangesAsync();
                await _context.AddAsync(sendertransaction);
                _context.Update(senderWallet);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInfo($"{ex.InnerException}");
                _logger.LogError($"{ex.Message}");
                _logger.LogDebug($"{ex.Source}");
                _logger.LogWarn($"{ex.StackTrace}");

            }
            return false;
        }

        public async Task<bool> MakeWithdrawal(TransactionDto withdraw, string walletaddress)
        {
            
            try
            {
                var myWallet = await _context.Wallets.FirstOrDefaultAsync(x => x.WalletAddress == walletaddress);

                if (myWallet == null)
                {
                    throw new ArgumentException(nameof(myWallet));
                }
                if (myWallet.WalletBalance < withdraw.TransactionAmount || withdraw.TransactionAmount < 100)
                {
                    return false;
                }
                myWallet.WalletBalance -= withdraw.TransactionAmount;
                var transaction = new TransactionModel
                {
                    WalletAddress = myWallet,
                    TransactionAmount = withdraw.TransactionAmount,
                    Description = withdraw.Description,
                    TransactionDate = DateTime.Now.ToString("g"),
                    TransactionType = TransactionTypeEnum.Withdraw
                };
                await _context.AddAsync(transaction);
                _context.Update(myWallet);
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInfo($"{ex.InnerException}");
                _logger.LogError($"{ex.Message}");
                _logger.LogDebug($"{ex.Source}");
                _logger.LogWarn($"{ex.StackTrace}");

            }
            return false;
        }
        public async Task<PageList<StatementofTransactionDto>> GetAllTransactionByWalletAddress(PaginatedParameters parameter, string walletaddress)
        {
            
            try
            {
                var alltransactionswallet = await _context.Transactions.Where(x => x.WalletAddress.WalletAddress == walletaddress).Select(x => new StatementofTransactionDto
                {
                    WalletAddress = x.WalletAddress.WalletAddress,
                    TransactionDate = x.TransactionDate,
                    TransactionAmount = x.TransactionAmount,
                    TransactionType = x.TransactionType,
                    Description = x.Description
                }).ToListAsync();

                return PageList<StatementofTransactionDto>.ToPagedList(alltransactionswallet,parameter.PageNumber,parameter.PageSize);
            }
            catch (Exception ex)
            {
                _logger.LogInfo($"{ex.InnerException}");
                _logger.LogError($"{ex.Message}");
                _logger.LogDebug($"{ex.Source}");
                _logger.LogWarn($"{ex.StackTrace}");

            }
            return null;
        }
        public async Task<BalanceDto> GetWalletDetails(string Currency,string walletAddress)
        {
            
            try
            {
                var walletdetails = await _context.Wallets.FirstOrDefaultAsync(x => x.WalletAddress == walletAddress);
                if (walletdetails == null)
                {
                    throw new ArgumentException(nameof(walletdetails));
                }
                var walletUserView = new BalanceDto()

                {
                   
                    WalletAddress = walletdetails.WalletAddress,
                    WalletBalance = $"{Currency.ToUpper()} {Math.Round(walletdetails.WalletBalance * await getApi(Currency), 3)}"


                };
                return walletUserView;
            }
            catch (Exception ex)
            {
                _logger.LogInfo($"{ex.InnerException}");
                _logger.LogError($"{ex.Message}");
                _logger.LogDebug($"{ex.Source}");
                _logger.LogWarn($"{ex.StackTrace}");

            }
            return new BalanceDto();
        }

        public async Task<double> getApi(string currency)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                using (var response = await httpClient.GetAsync("https://open.er-api.com/v6/latest/NGN", 
                    HttpCompletionOption.ResponseHeadersRead))
                {
                    var con = currency.ToUpper();
                    response.EnsureSuccessStatusCode();
                    var stream = await response.Content.ReadAsStreamAsync();
                    var rates = await JsonSerializer.DeserializeAsync<ApiDto>(stream);
                    if (rates != null)
                        return rates.rates[con];
                }
            }
            catch (Exception ex)
            {
                _logger.LogInfo($"{ex.InnerException}");
                _logger.LogError($"{ex.Message}");
                _logger.LogDebug($"{ex.Source}");
                _logger.LogWarn($"{ex.StackTrace}");
            };
            return 0;
        }
    }
}
