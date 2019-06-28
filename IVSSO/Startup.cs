using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using IVSSO.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IVSSO.PasswordHash;
using IVSSO.RolePolicy;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Reflection;
using System;
using Microsoft.AspNetCore.Authorization;
using IdentityServer4.Services;
using IVSSO.AccessToken;
using IdentityServer4.EntityFramework.DbContexts;
using IVSSO.Configuration;
using System.Linq;

namespace WebApplication1
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
               
               
            });

            //Access denied page，设置拒绝访问页面
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
            });
            //添加数据库上下文
            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            //role service
            //添加了 AuthenticationService和 CookieAuthenticationHandler
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            //Email服务
            services.AddTransient<IEmailSender, AuthMessageSender>();
            //Disable the Password Hash 方便测试
            services.AddScoped<IPasswordHasher<IdentityUser>, MyPasswordHasher>();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            //using the aspnetIdentity, the server will read the userdata via identity API

            //认证=Authentication，身份认证服务
            services.AddAuthentication("Bearer");

            //授权，添加基于角色认证的policy，对应控制器的Authorize特性
            services.AddAuthorization(options =>
            {
                //允许管理员和用户
                options.AddPolicy("All-Policy", policy => {
                    policy.AddRequirements(new RoleRequirement("Admin", "User"));
                });
                //只允许管理员
                options.AddPolicy("Admin-policy", policy =>
                {
                    policy.AddRequirements(new RoleRequirement("Admin"));
                });
                //只允许用户
                options.AddPolicy("User-policy", policy =>
                {
                    policy.AddRequirements(new RoleRequirement("User"));
                });
            });
            services.AddSingleton<IAuthorizationHandler, MyAuthorizationHandler>();
            
            //添加IS4服务，和相关资源资料读取
            services.AddIdentityServer()
                  //生成令牌的签发证书
                  .AddDeveloperSigningCredential()
                  .AddConfigurationStore(options =>
                  {
                      options.ConfigureDbContext = builder =>
                       builder.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
                       sql => sql.MigrationsAssembly(migrationsAssembly));
                  })
                  .AddOperationalStore(options =>
                   {
                       options.ConfigureDbContext = builder =>
                           builder.UseMySql(Configuration.GetConnectionString("DefaultConnection"),
                           sql => sql.MigrationsAssembly(migrationsAssembly));

                       // this enables automatic token cleanup. this is optional.
                       options.EnableTokenCleanup = false;
                       options.TokenCleanupInterval = 30;
                   })
                   .AddAspNetIdentity<IdentityUser>();

                   //自定义令牌相关接口注入，自定义的接口要写在后面避免被覆盖！！！由于这个是IdentityServer4
                   //的相关接和EmailSender不一样！！！！EmailSender可以写在identityServer前面，但是也必须写在
                   //identity后面避免被覆盖。
                   services.AddTransient<IProfileService, MyProfileService>();


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        private void AddAspNetIdentity<T>()
        {
            throw new NotImplementedException();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                //InitDatabase(app);
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

        
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //UseAuthentication not needed, UseIdentityServer adds the authentications middleware
            app.UseIdentityServer();
            app.UseCookiePolicy();
            
           
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            //CreateRoles(serviceProvider).Wait();
            
        }
        /*
        /// <summary>
        /// 初始化角色管理
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            IdentityResult roleResult;
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await RoleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
            await AddRoles("pineappleman520@gmail.com", "Admin", serviceProvider);
            await AddRoles("1111@qq.com", "User", serviceProvider);
            await AddRoles("985@qq.com", "User", serviceProvider);
            await AddRoles("dannike19980521@gmail.com", "User", serviceProvider);
            await AddRoles("dxk5418@psu.edu", "User", serviceProvider);

        }
        private async Task AddRoles(string Email,string Role ,IServiceProvider serviceProvider)
        {
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            IdentityUser user = await UserManager.FindByNameAsync(Email);
            await UserManager.AddToRoleAsync(user, Role);
        }
        */

         /*
        //可以用来更新数据库从config里面调取数据
        private void InitDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();

                if (!context.Clients.Any())
                {
                    foreach (var client in Config.Clients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }
                else
                {

                    foreach (var client in Config.Clients())
                    {
                        var item = context.Clients.Where(c => c.ClientId == client.ClientId).FirstOrDefault();
                        if (item == null)
                            context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }
                if (!context.IdentityResources.Any())
                {
                    foreach (var identityResources in Config.IdentityResources())
                    {
                        context.IdentityResources.Add(identityResources.ToEntity());
                    }
                    context.SaveChanges();
                }
                if (!context.ApiResources.Any())
                {
                    foreach (var apiResources in Config.ApiResources())
                    {
                        context.ApiResources.Add(apiResources.ToEntity());
                    }
                    context.SaveChanges();
                }
            }

        }*/
    }
}
