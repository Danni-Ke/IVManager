using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityManager.Data;
using Microsoft.EntityFrameworkCore;
using IdentityManager.PasswordHash;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using IdentityManager.RolePolicy;
using Microsoft.AspNetCore.Authorization;
using IdentityManager.Rolepolicy;


namespace Client
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
            //加载用户资料连接数据库
            services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            //用自定义的方式hash密码，目前不hash密码，方便开发查看，
            services.AddScoped<IPasswordHasher<IdentityUser>, MyPasswordHasher>();
            //加入HttpContext
            services.AddHttpContextAccessor();

            services.AddMvcCore()
                 .AddAuthorization()
                 .AddJsonFormatters();
            //Tokenvalidation package里面的东西
            services.AddAuthentication(
                options =>{
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    //这个貌似会导致发送401无授权的界面，但是目前不知道事怎么回事
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                     })
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                     {

                         options.Authority = "http://localhost:5000";
                         options.RequireHttpsMetadata = false;
                         //在IVSSO里面注册过的API的名字
                         options.Audience = "Api1";
                         options.SaveToken = true;
                     });
            
            //这边做到了不需要更改具体控制器的授权方式，只需要更改初始化的授权方式就好，添加或者减少里面的决策
            services.AddAuthorization(options =>
            {
                //用户管理员都可以接触
                options.AddPolicy("All-Policy", policy => { 
                    policy.AddRequirements(new RoleRequirement("Admin","User"));  
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
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //如果是开发者模式
            if (env.IsDevelopment())
            {
                //启动数据库错误详情页面
                app.UseDatabaseErrorPage();
                //从管道中捕获同步和异步System.Exception实例并生成HTML错误响应。
                //启动异常详情页面
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

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
