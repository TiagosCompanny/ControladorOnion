using ControladorOnion.Context;
using Microsoft.EntityFrameworkCore;

namespace ControladorOnion
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

#pragma warning disable CS8604 // Possível argumento de referência nula.
            builder.Services.AddDbContext<ControladorContext>(option => 
            option.UseInMemoryDatabase(builder.Configuration.GetConnectionString("DbOnion")));
#pragma warning restore CS8604 // Possível argumento de referência nula.


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Home}/{id?}");

            app.Run();
        }
    }
}
