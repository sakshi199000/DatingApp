using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DatingAppDb>(opt => opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));  

var app = builder.Build();
app.MapControllers();

app.Run();
