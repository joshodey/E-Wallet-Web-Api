using Core.Implementations;
using Core.Services;
using Infrastructure.Context;
using Infrastructure.Logging;
using Microsoft.EntityFrameworkCore;

namespace WalletApp.Extensions
{
    public static class DependencyInjection
    {
        public static void WalletServicesExtension( this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ILoggerManager, Logger>();
            services.AddScoped<ITransaction, TransactionRepo>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<IValidations, Validations>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddHttpClient();
            services.AddDbContext<ApplicationContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
            services.AddApiVersioning(setup =>
            {
                setup.AssumeDefaultVersionWhenUnspecified = true;
                setup.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                setup.ReportApiVersions = true;
            });
        }
    }
}
