using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PicuDrugsCore.Data;
using PicuDrugsCore.Models;
using PicuDrugsCore.Services;
using PicuDrugsCore.AuthorizationHandlers;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using cloudscribe.Web.Navigation;
using cloudscribe.Web.SiteMap;

namespace PicuDrugsCore
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            var customAppModelProvider = new CustomApplicationModelProvider();
            services.AddSingleton<IApplicationModelProvider>(customAppModelProvider);
            services.AddSingleton<IActionFilterMap>(customAppModelProvider);
            //our autopermission resolver must be added before call to AddCloudscribeNavigation
            services.AddScoped<INavigationNodePermissionResolver, NavigationNodeAutoPermissionResolver>();
            services.AddScoped<ISiteMapNodeService, NavigationTreeSiteMapNodeService>();
            services.AddCloudscribeNavigation(Configuration.GetSection("NavigationOptions"));

            services.AddMvc()
                .AddRazorOptions(options =>
                {
                    // if you download the cloudscribe.Web.Navigation Views and put them in your views folder
                    // then you don't need this line and can customize the views (recommended)
                    // you can find them here:
                    // https://github.com/joeaudette/cloudscribe.Web.Navigation/tree/master/src/cloudscribe.Web.Navigation/Views
                    options.AddCloudscribeNavigationBootstrap3Views();
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
