using GP.Data;
using GP.Hubs;
using GP.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GP
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
            services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")).EnableSensitiveDataLogging(), ServiceLifetime.Transient);
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<AppUser, IdentityRole>(o =>
            {
                o.SignIn.RequireConfirmedEmail = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders().AddDefaultUI();
            services.Configure<Service.EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddTransient<Service.MyEmailService>();


            services.AddControllersWithViews();

            services.AddScoped<IEstate, ProductManage>();
            services.AddScoped<ICategory, CategoryManage>();
            services.AddScoped<IReplaies, ManageReplayies>();
            services.AddScoped<ICity, CityManage>();
            services.AddScoped<IOpinion, OpinionManage>();

            services.AddScoped<ICurrency, CurrencyManage>();
            services.AddScoped<IPhotoEstate, PhotoEstateManage>();
            services.AddScoped<IService, ServicesManage>();
            services.AddScoped<IAdvertisement, ManageAdvertisement>();
            services.AddScoped<IService_Estate, ManageService_Estate>();
            services.AddScoped<ICommments, CommentsManagments>();
            services.AddScoped<IState, StateManage>();
            services.AddScoped<IType, TypeManage>();
            services.AddScoped<IContract, ContractManage>();
            services.AddScoped<IInformationGen, InformationGenManage>();
            services.AddScoped<IlikedEstates, likedEstatesManage>();
            services.AddScoped<INotification, NotificationManagement>();
            services.AddTransient<IContact, ContactManagments>();
            services.AddSignalR();
            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                //options.Lockout.MaxFailedAccessAttempts = 5;
                //options.Lockout.AllowedForNewUsers = true;
                options.Password.RequireDigit = false;

                options.Password.RequireUppercase = false;

                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;


            });

            services.AddAuthentication().AddCookie(options =>
            {
                options.LoginPath = "/Account/Unauthorized/";
                options.AccessDeniedPath = "/Account/Forbidden/";
            }).AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = false;
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidAudience = Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))
                };
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ICategory category,
            IType type,
            IState state,
            ICity city,
            IService service ,
            IInformationGen information
            )
        {
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

            SeedData.Seed(userManager, roleManager,category,type,state,city,service,information);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapHub<NotificationHub>("/notification");
            });
            app.UseEndpoints(endpoints =>
            {

                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "Admin",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });
        }
    }
}
