using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GoolgeTrendsApi.Models;
using GoolgeTrendsApi.WebGateway.Models;
using GoolgeTrendsApi.WebGateway.Options;
using GoolgeTrendsApi.WebGateway.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GoolgeTrendsApi.WebGateway
{
    public class Startup
    {
        //public Startup(IHostingEnvironment env)
        //{
        //    // Set up configuration sources.
        //    var builder = new ConfigurationBuilder()
        //        .SetBasePath(env.ContentRootPath)
        //        .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        //    Configuration = builder.Build();
        //}

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddMemoryCache();
            services.AddLogging();

            services.Configure<SynonymsOption>(Configuration.GetSection(nameof(SynonymsOption)));
            services.AddSingleton<ISynonymsProvider, StaticConfigBasedSynonymsProvider>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseStaticFiles();

            ConfigAutoMapper();
        }


        private void ConfigAutoMapper()
        {
            Mapper.Initialize(c =>
            {
                c.CreateMap<GetTrendsDataModel, ApiTransactionArgs>(MemberList.None)
                            .ForMember(_ => _.Keys, src => src.MapFrom(_ => _.Keys.Split(",", StringSplitOptions.RemoveEmptyEntries)));
            });

            Mapper.AssertConfigurationIsValid();
        }
    }
}
