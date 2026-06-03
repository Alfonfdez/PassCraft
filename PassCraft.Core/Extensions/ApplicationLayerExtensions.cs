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
        /// Registers all high-level orchestrators, generators, validation engines, and tracking services.
        /// </summary>
        /// <param name="services">The core framework service collection instance being extended.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance to allow method chaining.</returns>
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            // Register Business Logic Services
            services.AddSingleton<IPasswordGenerationService, PasswordGenerationService>();
            services.AddSingleton<IPasswordValidationService, PasswordValidationService>();
            services.AddSingleton<IPasswordHistoryService, PasswordHistoryService>();

            return services;
        }
    }
}
