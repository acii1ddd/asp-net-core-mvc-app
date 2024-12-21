using DAL.EF;
using DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DAL.EFRepositories;

public static class ConfigurationExtensions
{

    // IoC контейнер.
    public static void ConfigureDAL(this IServiceCollection services, string connection)
    {
        // регистрация контекста бд ef
        services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

        //services.AddScoped<IPassengerRepositor    y, DAL.EFRepositories.PassengerRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>(); // в качетсве реализации ITicketRepository будет использоваться DAL.EFRepositories.TicketRepository>
        services.AddScoped<ITrainRepository, TrainRepository>(); // создается объект DAL.EFRepositories.TrainRepository и маппер к нему
    }

    public static void ConfigurateDalFileServices(this IServiceCollection services, string csvFilePath)
    {
        // при getService с IPassengerRepository будет создаваться new DAL.CSVRepositories.PassengerRepository(csvFilePath)
        services.AddScoped<IPassengerRepository>(provider => new DAL.CSVRepositories.PassengerRepository(csvFilePath)); // в качетсве реализации IPassengerRepository будет использоваться DAL.CSVRepositories.PassengerRepository
    }

    public static void ConfigurateDalEFServices(this IServiceCollection services)
    {
        services.AddScoped<IPassengerRepository, DAL.EFRepositories.PassengerRepository>(); // здесь не нужно new так как знает как создать DAL.EFRepositories.PassengerRepository из-за контекста
    }
}
