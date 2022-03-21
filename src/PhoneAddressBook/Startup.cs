namespace PhoneAddressBook
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using DNTScheduler.Core;
    using PhoneAddressBook.BackgroundServices.CountriesUpdater;
    using PhoneAddressBook.Data;
    using PhoneAddressBook.Data.Models;
    using PhoneAddressBook.Services.Countries;
    using PhoneAddressBook.Services.Records;
    using PhoneAddressBook.Services.Search;

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
            services.AddDNTScheduler(options =>
            {
                // DNTScheduler needs a ping service to keep it alive.
                // If you don't need it, don't add it!
                options.AddPingTask(siteRootUrl: "https://localhost:80");

                options.AddScheduledTask<CountriesUpdater>(
                    runAt: utcNow =>
                    {
                        var now = utcNow.AddHours(3.5);
                        return now.Day % 3 == 0 && now.Hour == 0 && now.Minute == 1 && now.Second == 1;
                    },
                    order: 2);
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();

            // Application services
            services.AddTransient<ISearchService, SearchService>();
            services.AddTransient<IRecordsService, RecordsService>();
            services.AddTransient<ICountriesService, CountriesService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ICountriesService countriesService)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
            }

            countriesService.UpdateAsync().GetAwaiter().GetResult();
            app.UseDNTScheduler();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
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
