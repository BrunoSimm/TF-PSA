using Infraestrutura.Data;
using MatriculaPUCRS.Data;
using MatriculaPUCRS.Data.Persistencia;
using MatriculaPUCRS.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Persistencia.Interfaces.Repositorios;
using Newtonsoft.Json;

namespace MatriculaPUCRS
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
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<ApplicationUser, IdentityRole>()
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultUI()
             .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 1;
            });

            services.AddDbContext<MatriculaContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("MatriculaPUCRS")));

            services.AddControllersWithViews();

            services.AddControllers().AddNewtonsoftJson(x =>
                {
                    x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddScoped<IDisciplinaRepositorio, DisciplinaRepositorio>();
            services.AddScoped<ITurmaRepositorio, TurmaRepositorio>();
            services.AddScoped<IEstudanteRepositorio, EstudanteRepositorio>();
            services.AddScoped<IMatriculaTurmaRepositorio, MatriculaTurmaRepositorio>();
            services.AddScoped<ISemestreRepositorio, SemestreRepositorio>();
            services.AddScoped<ICurriculoRepositorio, CurriculoRepositorio>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Disciplinas}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
