using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TicketApi.GraphQL;
using TicketApi.GraphQL.Schemas;
using TicketBusinessLogic;
using TicketCore;
using TicketCore.Interfaces;
using TicketSearch.Lucene;
using TicketStorage.InMemory;

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

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ReactApp/build";
            });

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.AddGraphQL(opts =>
            {
                opts.EnableMetrics = true;
                opts.ExposeExceptions = !Environment.IsProduction();
            })
            .AddGraphQLAuthorization(options =>
            {
                options.AddPolicy(TicketGraphSchema.GraphQLAuthPolicyName, p =>
                {
                    p.RequireAssertion(x => true);  //p.RequireAuthenticatedUser(); change this when JWT auth is setup 
                });
            })
            .AddDataLoader()
            .AddRelayGraphTypes()
            .AddGraphTypes();

            services.AddSingleton<TicketGraphSchema>();
            services.AddSingleton<ITicketStore, InMemoryTicketStore>();
            services.AddSingleton<ITicketSearch, InMemoryLuceneSearch>();
            services.AddSingleton<IDataLoader, EmbeddedJsonDataLoader>();
            services.AddSingleton<BusinessLogic>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseResponseCompression();

            app.UseGraphQL<TicketGraphSchema, TicketGraphQLHttpMiddleware<TicketGraphSchema>>();
            if (!env.IsProduction())
            {
                app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());
            }

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ReactApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

            var logger = app.ApplicationServices.GetService<ILogger<Startup>>();
            logger.LogInformation($"Application Version: {RuntimeInfo.ApplicationVersion} {RuntimeInfo.ApplicationBuild}");

            var logic = app.ApplicationServices.GetService<BusinessLogic>();
            var initDataTask = logic.InitializeDataAsync();
        }
    }
}
