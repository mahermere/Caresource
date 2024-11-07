// ------------------------------------------------------------------------------------------------
// <copyright>
//   Copyright (c) CareSource, 2018. All rights reserved.
// 
//   WorkFlowAndCollab.API.Eligibility
//   Startup.cs
// </copyright>
// ------------------------------------------------------------------------------------------------

using CareSource.WC.Core.Http;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CareSource.WC.Services.Eligibility
{
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Swashbuckle.AspNetCore.Swagger;

	public class Startup
	{
		public Startup(IConfiguration configuration) => Configuration = configuration;

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddCareSourceDefaultWebApiConfiguration(Configuration,
                c =>
                {
                    c.SwaggerDoc(
                        "v1",
                        new Info
                        {
                            Title = "Eligibility",
                            Version = "v1"
                        });
                });

            services.AddApiVersioning(o => o.DefaultApiVersion = ApiVersion.Default);
        }

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            app.UseHttpsRedirection();
            app.AddCareSourceDefaultWebApiConfiguration(env);
            app.UseMvc();
        }
	}
}