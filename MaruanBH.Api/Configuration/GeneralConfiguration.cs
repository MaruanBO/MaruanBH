using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MaruanBH.Api.Configuration
{
    public static class GeneralConfiguration
    {
        public static void UseSwagger(this IApplicationBuilder app, string endpointName)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        public static void AddLogging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConfiguration(configuration.GetSection("Logging"));
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });
        }

    }
}
