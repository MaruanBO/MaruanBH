using MaruanBH.Api.Configuration;

namespace MaruanBH.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Startup>());
            services.AddControllers();
            services.AddRepositories();
            services.AddServices();
            services.AddCQRS();
            services.AddValidators();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseSwagger("MaruanBH");
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
