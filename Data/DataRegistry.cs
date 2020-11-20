using Microsoft.Extensions.DependencyInjection;
using MyGame.Data.Concrete;
using MyGame.Data.Interfaces;

namespace MyGame.Data
{
    public static class DataRegistry
    {
        public static IServiceCollection RegisterData(this IServiceCollection services)
        {
            services.AddTransient(typeof(ILeaderboardsData), typeof(LeaderboardsData));

            return services;
        }
    }
}
