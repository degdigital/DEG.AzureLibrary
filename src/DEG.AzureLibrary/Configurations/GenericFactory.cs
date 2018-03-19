using System;
using Microsoft.Extensions.DependencyInjection;

namespace DEG.AzureLibrary
{
    /// <summary>
    /// Interface IFactory
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFactory<T>
        where T : class
    {
        /// <summary>
        /// Roots the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        void RootServiceCollection(IServiceCollection services);
        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>T.</returns>
        T CreateInstance(params object[] parameters);
    }

    /// <summary>
    /// Class GenericFactory.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="DEG.AzureLibrary.IFactory{T}" />
    public class GenericFactory<T> : IFactory<T>
        where T : class
    {
        readonly IServiceProvider _provider;
        IServiceCollection _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericFactory{T}"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public GenericFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// Roots the service collection.
        /// </summary>
        /// <param name="services">The services.</param>
        public void RootServiceCollection(IServiceCollection services)
        {
            _services = services;
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns>T.</returns>
        public T CreateInstance(params object[] parameters)
        {
            return ActivatorUtilities.CreateInstance<T>(_provider, parameters);
        }
    }
}
