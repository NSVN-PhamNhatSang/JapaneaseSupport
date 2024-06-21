using JLearning.Extensions;
using JLearning.Repository;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using JLearning;
using System.Globalization;
using JLearning.Data;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using JLearning.Interfaces;
using JLearning.Services;
using JLearning.Models;
using newmanJapanese.Controllers;
using Autofac.Core;
using JLearning.Repositories;
using static JLearning.Repositories.UserCourseRepository;

var MyAllowSpecificOrigins = "AllowAllOrigins";
var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);
builder.Configuration.Bind("GoogleCloud", new GoogleCloudSetting());
builder.Services.Configure<GoogleCloudSetting>(builder.Configuration.GetSection("GoogleCloud"));
//builder.Services.AddSession();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
                      });
});

// Add services to the container.

builder.Services.AddJWTServices(builder.Configuration);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });
});
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<WebContext>(options =>

    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
    
) ;

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IUserCourseRepository, UserCoursesRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IUserDetailRepository, UserDetailRepository>();
//builder.Services.AddScoped<User,usersController>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization(); 

app.MapControllers();
//app.UseSession();

app.Run();
