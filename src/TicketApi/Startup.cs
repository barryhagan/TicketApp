using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TicketApi.GraphQL.Schemas;
using TicketCore;
using TicketCore.Interfaces;

namespace TicketApi
{
    public class Startup
    {
        private IHostingEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Environment = env;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                //options.SetInHubJwtBearerOptions(jwtIssuerOptions);
            });

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.AddGraphQL(opts =>
            {
                opts.EnableMetrics = false;
                opts.ExposeExceptions = Environment.IsDevelopment();
            })
            .AddGraphQLAuthorization(options =>
            {
                options.AddPolicy(TicketSchema.GraphQLAuthPolicyName, p =>
                {
                    p.RequireAssertion(x => true);
                    //p.RequireAuthenticatedUser();
                });
            })
            .AddDataLoader()
            .AddRelayGraphTypes()
            .AddGraphTypes();

            services.AddSingleton<TicketSchema>();
            
            services.AddSingleton<ITicketStore, TicketStorage.InMemoryTicketStore>();
            services.AddSingleton<ITicketSearch, TicketSearch.InMemoryLuceneSearch>();

            services.AddSingleton<TicketBusinessLogic>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();            
            }

            app.UseAuthentication();

            app.UseResponseCompression();

            app.UseGraphQL<TicketSchema>();
            if (env.IsDevelopment())
            {
                app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());
            }

            var logger = app.ApplicationServices.GetService<ILogger<Startup>>();
            logger.LogInformation($"Application Version: {RuntimeInfo.ApplicationVersion} {RuntimeInfo.ApplicationBuild}");

            var loader = app.ApplicationServices.GetService<TicketBusinessLogic>();
            loader.InitializeData().Wait();

            
        }
    }
}
