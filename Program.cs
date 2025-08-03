
using MeetingRoomManagement;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MeetingRoomManagement.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MeetingRoomManagement.DataBaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MeetingRoomManagement.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });//3m nzid title w r2m w esm l nas5a bi wejhet l swagger
    //Authentication setup
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme//3m n3aref no3 l 7imeye l ra7 nst5dema esma "Bearer"
    {
        In = ParameterLocation.Header,//3m n7aded wen ra7 yen7at l token
        Description = "Please enter token with Bearer prefix",//text bibayen bel message box l 7a 7at fiha l token
        Name = "Authorization",//esm lal header l ra8 yenzed bel request
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"//3m n3aref enu no3 l token l ra8 neb3to huwe Bearer
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement//3m n2oul enu l endpoints l fiha [authorize] ra7 tst3ml hayda l no3 men l token bala hay l swagger ma bytlb token
    {
        {
        new OpenApiSecurityScheme
        {
            Reference= new OpenApiReference
            {
                Type=ReferenceType.SecurityScheme,
                Id="Bearer"
            }
        },
        new string[] {}//hay 5assa bi oAuth ma 3m nst3mlo fa 5alayneha fadye
        }
    });
});
builder.Services.AddScoped<TokenServices>();//la 5alle l .net yjahez l tokenservices lama yentalab juwet l controller aw class
builder.Services.AddDbContext<StoreDBContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
var JwtSecition = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(JwtSecition);
builder.Services.AddAuthentication(Options =>
{
    Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(Options =>
{
    Options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],//jeha l jeye menna lma user yeb3at token l api gonna check min ba3at l token and compare the name l bi alb l token ma3 l ValidIssuer and if they aren't the same l token is refused
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JwtSettings:Key"))
    };
    builder.Services.AddAuthorization();
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
