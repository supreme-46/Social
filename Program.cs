using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Social.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();

app.UseAuthorization();

//api use cors

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("*"));

app.MapControllers();

app.Run();
