using Hotel.Core.Interfaces;
using Hotel.Core.Services;
using Hotel.Data;
using Hotel.DataLayer.Repositories;
using Hotel.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hotel.Web;

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

        var connectionString = "DataSource=:memory:";
        var connection = new SqliteConnection(connectionString);
        connection.Open();

        services.AddDbContext<HotelContext>(options =>
            options.UseSqlite(connection)
        );

        EnsureDatabaseExists(connection);

        services.AddTransient<IRoomRepository, RoomRepository>();
        services.AddTransient<IRoomReservationRepository, RoomReservationRepository>();
        services.AddTransient<IRoomReservationService, RoomReservationService>();
    }

    private static void EnsureDatabaseExists(SqliteConnection connection)
    {
        var builder = new DbContextOptionsBuilder<HotelContext>();
        builder.UseSqlite(connection);

        using var context = new HotelContext(builder.Options);
        context.Database.EnsureCreated();
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
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
        });
    }
}