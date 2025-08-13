using CRUDApp.Data;
using CRUDApp.Services;
using CRUDApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace CRUDApp.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        // Register Db Context
        collection.AddDbContext<ApplicationDbContext>();
        
        // Register View Models
        collection.AddTransient<MainWindowViewModel>();
        
        // Register servcies
        collection.AddTransient<ICustomerService, CustomerService>();
    }
}