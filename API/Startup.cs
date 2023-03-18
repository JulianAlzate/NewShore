using API.Model;
using AutoMapper;
using BLL;
using BLL.Dto;
using BLL.Interfaces;
using BLL.RN;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //Unity Injection Dependencia
            services.AddMvc().AddControllersAsServices()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //Conección
            services.AddEntityFrameworkSqlServer();
            services.AddDbContext<NewShoreContext>(config =>
            {
                config.UseSqlServer(Configuration.GetConnectionString("NewShoreConnection"));
            });
            services.AddDbContext<DbContext>(config =>
            {
                config.UseSqlServer(Configuration.GetConnectionString("CorrespoConnection"));
            });


            services.AddAutoMapper(typeof(Startup));

            #region Registrar Dependencias

            services.AddScoped<IBusinessLogic<JourneyDTO>, JourneyBLL>();
            services.AddScoped<IBusinessLogic<FlightDTO>, FlightBLL>();
            services.AddScoped<IBusinessLogic<TransportDTO>, TransportBLL>();
            services.AddScoped<IBusinessLogic<JourneyFlightDTO>, JourneyFlightBLL>();

            #endregion

            #region Mapper
            services.AddScoped(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            }).CreateMapper());
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
