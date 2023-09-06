using System.Data.Common;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using MySqlConnector;
using MISA.CUKCUK.Infrastructure;
using MISA.CUKCUK.Application;
using MISA.CUKCUK.Domain;
using MISA.CUKCUK.Infrastructure.UnitOfWork;

namespace MISA.CUKCUK.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Services

            // Add controller
            builder.Services.AddControllers().AddJsonOptions(options =>
                options.JsonSerializerOptions.PropertyNamingPolicy = null);

            // Add dbconnection
            builder.Services.AddTransient<DbConnection>(options =>
                    new MySqlConnection(builder.Configuration.GetConnectionString("AmisDb")));

            // Add unit of work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add services
            builder.Services.AddScoped<IMaterialService, MaterialService>();
            builder.Services.AddScoped<IUnitService, UnitService>();
            builder.Services.AddScoped<IWarehouseService, WarehouseService>();

            // Add domain services
            builder.Services.AddScoped<IMaterialDomainService, MaterialDomainService>();
            builder.Services.AddScoped<IUnitDomainService, UnitDomainService>();
            builder.Services.AddScoped<IWarehouseDomainService, WarehouseDomainService>();
            builder.Services.AddScoped<IConversionUnitDomainService, ConversionUnitDomainService>();

            // Add repositories
            builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
            builder.Services.AddScoped<IUnitRepository, UnitRepository>();
            builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
            builder.Services.AddScoped<IConversionUnitRepository, ConversionUnitRepository>();

            // Add mapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Add middleware
            builder.Services.AddSingleton<ExceptionMiddleware>();

            // Add localization services
            builder.Services.AddLocalization();

            // Configure supported cultures
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("vi-VN"),
                };
                options.DefaultRequestCulture = new RequestCulture("vi-VN");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
                options.ApplyCurrentCultureToResponseHeaders = true;
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(p => p.AddPolicy("MyCors", build =>
            {
                build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
            }));

            #endregion

            var app = builder.Build();

            #region Configures

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Enable localization middleware
            app.UseRequestLocalization();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseCors("MyCors");

            app.MapControllers();

            app.UseMiddleware<ExceptionMiddleware>();

            app.Run();

            #endregion
        }
    }
}