using Microsoft.Extensions.DependencyInjection;
using System;

namespace DEG.AzureLibrary
{
    /// <summary>
    /// Class ServiceLocator.
    /// </summary>
    public static class ServiceLocator
    {
        /// <summary>
        /// The service provider
        /// </summary>
        public static IServiceProvider ServiceProvider;

        /// <summary>
        /// Resolves the specified parameters.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameters">The parameters.</param>
        /// <returns>T.</returns>
        public static T Resolve<T>(params object[] parameters)
        {
            return ActivatorUtilities.CreateInstance<T>(ServiceProvider, parameters);
        }
    }
}
