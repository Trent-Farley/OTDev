using TastyMeals.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TastyMeals.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TastyMeals.Models.Interfaces;
using TastyMeals.Models.Repositories;
using AspNetCore.ReCaptcha;

namespace TastyMeals
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
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection")
                    )
                );
            services.AddDbContext<TastyMealsDbContext>(opts =>
            {
                opts.UseNpgsql(Configuration["ConnectionStrings:MealFridge"] + ";MultipleActiveResultSets=true");
            });
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddReCaptcha(Configuration.GetSection("ReCaptcha"));
            services.AddScoped<IRecipeRepo, RecipeRepo>();
            services.AddScoped<ISavedrecipeRepo, SavedrecipeRepo>();
            services.AddScoped<IFridgeRepo, FridgeRepo>();
            services.AddScoped<IRestrictionRepo, RestrictionRepo>();
            services.AddScoped<IIngredientRepo, IngredientRepo>();
            services.AddScoped<IRecipeIngredRepo, RecipeIngredRepo>();
            services.AddScoped<ISpnApiService, SpnApiService>();
            services.AddScoped<IDietRepo, DietRepo>();
            services.AddScoped<IMealRepo, MealRepo>();
            services.AddAuthentication().AddGoogle(options =>
            {
                IConfigurationSection googleAuthNSection =
                    Configuration.GetSection("Authentication:Google");

                options.ClientId = googleAuthNSection["ClientId"];
                options.ClientSecret = googleAuthNSection["ClientSecret"];
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}