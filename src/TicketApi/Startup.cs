using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TicketApi.GraphQL;
using TicketApi.GraphQL.Schemas;
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

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                //TODO: add JWT auth
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
                options.AddPolicy(TicketSchema.GraphQLAuthPolicyName, p =>
                {
                    p.RequireAssertion(x => true);
                    // TODO : enable authorization after JWT auth is turned on
                    //p.RequireAuthenticatedUser();
                });
            })
            .AddDataLoader()
            .AddRelayGraphTypes()
            .AddGraphTypes();

            services.AddSingleton<TicketSchema>();
            
            services.AddSingleton<ITicketStore, InMemoryTicketStore>();
            services.AddSingleton<ITicketSearch, InMemoryLuceneSearch>();
            services.AddSingleton<IDataLoader, EmbeddedJsonDataLoader>();

            services.AddSingleton<TicketBusinessLogic>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();            
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();
            app.UseResponseCompression();

            app.UseGraphQL<TicketSchema, TicketGraphQLHttpMiddleware<TicketSchema>>();
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

            var loader = app.ApplicationServices.GetService<TicketBusinessLogic>();
            loader.InitializeData().Wait();            
        }
    }
}
