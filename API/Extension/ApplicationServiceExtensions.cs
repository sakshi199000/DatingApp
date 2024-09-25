using System;
using API.Data;
using API.Services;
using API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace API.Extension;

public static class ApplicationServiceExtensions
{

    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config){
        services.AddControllers();
        services.AddDbContext<DatingAppDb>(opt => opt.UseSqlite(config.GetConnectionString("DefaultConnection")));  
        services.AddCors();
        services.AddScoped<ITokenService,TokenService>();
        return services;
    }
}
