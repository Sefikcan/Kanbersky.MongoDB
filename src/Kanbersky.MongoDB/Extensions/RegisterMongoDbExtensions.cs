using Kanbersky.MongoDB.Abstract;
using Kanbersky.MongoDB.Concrete;
using Kanbersky.MongoDB.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Kanbersky.MongoDB.Extensions
{
    public static class RegisterMongoDbExtensions
    {
        public static IServiceCollection RegisterKanberskyMongoDB(this IServiceCollection services, IConfiguration configuration)
        {
            MongoDBSettings mongoDBSettings = new MongoDBSettings();
            configuration.GetSection(nameof(MongoDBSettings)).Bind(mongoDBSettings);
            services.AddSingleton(mongoDBSettings);

            services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
            return services;
        }
    }
}
