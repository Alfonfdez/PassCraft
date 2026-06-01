using Microsoft.Extensions.DependencyInjection;
using PassCraft.Core.Contracts;
using PassCraft.Core.Services;

namespace PassCraft.Core.Extensions
{
    /// <summary>
    /// Centralized extension methods to register domain core business logic workflows.
    /// </summary>
    public static class ApplicationLayerExtensions
    {
        /// <summary>
        /// Registers all high-level orchestrators, generators, and tracking services.
        /// </summary>
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            // Register Business Logic Services
            services.AddSingleton<IPasswordHistoryService, PasswordHistoryService>();

            return services;
        }
    }
}
