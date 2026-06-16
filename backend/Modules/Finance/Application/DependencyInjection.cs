using Microsoft.Extensions.DependencyInjection;

using ManagementSystem.Application.Contracts;
using ManagementSystem.Modules.Finance.Application.Services;
using ManagementSystem.Modules.Finance.Infrastructure.Repositories;

namespace ManagementSystem.Modules.Finance.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddFinanceModule(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ITransactionService, TransactionService>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IBudgetService, BudgetService>();
        services.AddScoped<IBudgetRepository, BudgetRepository>();
        services.AddScoped<IInvestmentService, InvestmentService>();
        services.AddScoped<IInvestmentRepository, InvestmentRepository>();
        services.AddScoped<IRecurringTransactionService, RecurringTransactionService>();
        services.AddScoped<IRecurringTransactionRepository, RecurringTransactionRepository>();
        return services;
    }
}
