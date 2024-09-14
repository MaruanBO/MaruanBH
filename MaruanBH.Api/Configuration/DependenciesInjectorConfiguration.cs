using MaruanBH.Domain.Repositories;
using MaruanBH.Persistance.Repositories;
using MaruanBH.Business.CustomerContext.QueryHandler;
using MaruanBH.Business.CustomerContext.CommandHandler;
using MaruanBH.Core.Services;
using MaruanBH.Business.Services;
using MaruanBH.Core.CustomerContext.Validators;
using MaruanBH.Core.CustomerContext.Queries;
using FluentValidation.AspNetCore;
using FluentValidation;
using Serilog;
using MaruanBH.Core.CustomerContext.DTOs;
using MaruanBH.Business.AccountContext.QueryHandler;
using MaruanBH.Core.AccountContext.Validators;
using MaruanBH.Core.AccountContext.DTOs;
using MaruanBH.Business.AccountContext.CommandHandler;
// DI
namespace MaruanBH.Api.Configuration
{
    public static class DependenciesInjectorConfiguration
    {
        // For the sake of simplicity and speed during development, we are manually registering the repositories here.
        // In a more robust solution, we would typically implement a Unit of Work pattern alongside repository pattern.
        // This would improve testability and facilitate Test-Driven Development (TDD) by decoupling the data access layer.
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddLogging(logBuilder => logBuilder.AddSerilog(dispose: true));
            services.AddMemoryCache();
        }

        public static void AddCQRS(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetCustomerDetailsQueryHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateCustomerCommandHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAccountDetailsQueryHandler).Assembly));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateAccountCommandHandler).Assembly));
        }

        public static void AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<GetCustomerDetailsQuery>, GetCustomerDetailsQueryValidator>();
            services.AddScoped<IValidator<CreateCustomerDto>, CreateCustomerDtoValidator>();
            services.AddScoped<IValidator<CreateAccountDto>, CreateAccountDtoValidator>();
        }
    }
}