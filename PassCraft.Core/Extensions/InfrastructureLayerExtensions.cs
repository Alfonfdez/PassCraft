using Microsoft.Extensions.DependencyInjection;
using PassCraft.Core.Contracts;
using PassCraft.Core.Repositories;

namespace PassCraft.Core.Extensions
{
    /// <summary>
    /// Centralized extension methods to register low-level storage data infrastructure engines.
    /// </summary>
    public static class InfrastructureLayerExtensions
    {
        /// <summary>
        /// Registers all repositories and persistent engines required for data durability.
        /// </summary>
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
        {
            // Register Data Repositories
            services.AddSingleton<IPasswordRepository, InMemoryPasswordRepository>();

            return services;
        }
    }
}
