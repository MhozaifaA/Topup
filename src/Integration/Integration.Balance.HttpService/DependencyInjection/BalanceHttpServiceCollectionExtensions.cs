using Integration.Balance.HttpService;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class BalanceHttpServiceCollectionExtensions
    {
        /// <summary>
        /// section name <see cref="_section"/> <code>BalanceHttpService</code>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddBalanceHttpService(this IServiceCollection services)
        {
            IConfiguration configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var options  = 
                configuration.GetSection(BalanceHttpServiceOptions._section).Get<BalanceHttpServiceOptions>();

            ArgumentNullException.ThrowIfNullOrWhiteSpace(options?.BaseUrl);

            services.AddHttpClient<BalanceHttpService>(o => o.BaseAddress = new Uri(options.BaseUrl));

            return services;
        }
    }
}
