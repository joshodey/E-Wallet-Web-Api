using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.DTOModels;

namespace Core.Services
{
    public interface IWalletRepository
    {
        Task<IEnumerable<WalletDto>> GetWallet(int id);
        Task<bool> CreateWallet(WalletDto wallet);
        Task <IEnumerable<WalletDto>> GetAllWallets();
        void DeleteWallet(int id);  
        
           
        
    }
}
