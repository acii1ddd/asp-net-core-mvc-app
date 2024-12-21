using BLL.Profiles;
using BLL.ServiceInterfaces;
using BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BLL.Configuration
{
    public static class ConfigurationExtensions
    {
        public static void ConfigureBLL(this IServiceCollection services, string connection)
        {
            services.ConfigureDAL(connection); // репозитории для работы с данными

            // добавление профилей для automapper
            services.AddAutoMapper(
                typeof(PassengerProfile),
                typeof(TicketProfile),
                typeof(TrainProfile)
            );

            // добавление объектов сервисов в коллекцию
            services.AddTransient<IPassengerService, PassengerService>();
            services.AddTransient<ITicketService, TicketService>();
            services.AddTransient<ITrainService, TrainService>(); // TrainService реализация ITrainService (интерфейс)
        }

        public static void ConfigureEfServices(this IServiceCollection services) // пассажиры
        {
            services.ConfigurateDalEFServices(); // config dal for ef
        }

        public static void ConfigureFileServices(this IServiceCollection services, string _csvFilePath)
        {
            string csvFilePath = _csvFilePath;
            services.ConfigurateDalFileServices(_csvFilePath); // конфигурирует DAL под csv файлы
        }
    }
}