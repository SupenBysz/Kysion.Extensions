using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Kysion.Extensions.Core.Contracts
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 通过程序集加载临时对象
        /// </summary>
        /// <param name="services"></param>
        /// <param name="namespaceName"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddTransientFromNamespace(
            this IServiceCollection services,
            string namespaceName,
            params Assembly[] assemblies
        )
        {
            foreach (Assembly assembly in assemblies)
            {
                IEnumerable<Type> types = assembly
                    .GetTypes()
                    .Where(x => x.IsClass && x.Namespace != null && x.Namespace!.StartsWith(namespaceName, StringComparison.InvariantCultureIgnoreCase));

                foreach (Type? type in types)
                {
                    if (services.All(x => x.ServiceType != type))
                    {
                        _ = services.AddTransient(type);
                    }
                }
            }

            return services;
        }

        /// <summary>
        /// 通过程序集加载单例对象
        /// </summary>
        /// <param name="services"></param>
        /// <param name="namespaceName"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddSingletonFromNamespace(
            this IServiceCollection services,
            string namespaceName,
            params Assembly[] assemblies
        )
        {
            foreach (Assembly assembly in assemblies)
            {
                IEnumerable<Type> types = assembly
                    .GetTypes()
                    .Where(x => x.IsClass && x.Namespace != null && x.Namespace!.StartsWith(namespaceName, StringComparison.InvariantCultureIgnoreCase));

                foreach (Type? type in types)
                {
                    if (services.All(x => x.ServiceType != type))
                        _ = services.AddSingleton(type);
                }
            }

            return services;
        }

        /// <summary>
        /// 通过程序集加载指定作用域的对象
        /// </summary>
        /// <param name="services"></param>
        /// <param name="namespaceName"></param>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static IServiceCollection AddScopedFromNamespace(
            this IServiceCollection services,
            string namespaceName,
            params Assembly[] assemblies
            )
        {
            foreach (Assembly assembly in assemblies)
            {
                IEnumerable<Type> types = assembly
                    .GetTypes()
                    .Where(x => x.IsClass && x.Namespace != null && x.Namespace!.StartsWith(namespaceName, StringComparison.InvariantCultureIgnoreCase));

                foreach (Type? type in types)
                {
                    if (services.All(x => x.ServiceType != type))
                        _ = services.AddScoped(type);
                }
            }

            return services;
        }
    }

}
