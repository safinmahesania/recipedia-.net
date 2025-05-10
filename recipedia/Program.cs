using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using recipedia.Database;
using recipedia.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//DB Connection
builder.Services.AddDbContext<DBContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<DBContext>();

//builder.Services.AddIdentity<User, IdentityRole>()
//    .AddEntityFrameworkStores<DBContext>()
//    .AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<User>();

app.UseHttpsRedirection();

//app.UseAuthorization();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
