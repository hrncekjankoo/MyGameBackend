using Microsoft.Extensions.DependencyInjection;
using MyGame.Domain.Concrete;
using MyGame.Domain.Interfaces;

namespace MyGame.Domain
{
    public static class DomainRegistry
    {
        public static IServiceCollection RegisterDomains(this IServiceCollection services)
        {
            services.AddTransient(typeof(ILeaderboardsDomain), typeof(LeaderboardsDomain));
            services.AddTransient(typeof(IMatchDomain), typeof(MatchDomain));

            return services;
        }
    }
}
