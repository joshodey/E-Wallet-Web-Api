using Core.Services;
using Entity.DTOModels;
using Entity.Models;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Implementations
{
    
    public class WalletRepository : IWalletRepository
    {
        private ApplicationContext _applicationContext;

        public WalletRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;

        }
        public async Task<bool> CreateWallet(WalletDto wallet)
        {
            var newWallet = new WalletModel
            {
                WalletAddress = Guid.NewGuid().ToString().Substring(0,10)
                //WalletBalance = 0,
            };
            await _applicationContext.Wallets.AddAsync(newWallet);
            if (await _applicationContext.SaveChangesAsync() > 0) return true;
            return false;   
            
        }

        public async void DeleteWallet(int id)
        {
            var result = _applicationContext.Wallets.FirstOrDefaultAsync(x=>x.WalletAddressId == id);
            if (await result != null)
               _applicationContext.Remove(result);
            await _applicationContext.SaveChangesAsync();


        }

        public async Task<IEnumerable<WalletDto>> GetAllWallets()
        {
           var AllWallets= await _applicationContext.Wallets.Select(x=>new WalletDto
            {
                WalletBalance = x.WalletBalance,
                WalletAddress = x.WalletAddress,

                
            }).ToListAsync();
            return AllWallets;
            
        }

        public async Task<IEnumerable<WalletDto>> GetWallet(int id)
        {
         var response = await _applicationContext.Wallets.Where(x => x.WalletAddressId == id).
                Select(x => new WalletDto
                {
                    WalletAddress = x.WalletAddress,
                    WalletBalance=x.WalletBalance,

                }).ToListAsync();

            return response;
              
        }

       
    }
}
