using WebAPI;
using DataAccess.DbAccess;
using Microsoft.Extensions.FileProviders;
using System.ComponentModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using WebAPI.Global.Hashing;
using WebAPI.Global.Token;
using WebAPI.Global.Encode;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Add BackgroundWorker
//builder.Services.AddHostedService<BackgroundWorkerService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});



IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    var jwtSection = configuration.GetSection("Jwt");

    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = jwtSection.GetValue<bool>("ValidateIssuer"),
        ValidateAudience = jwtSection.GetValue<bool>("ValidateAudience"),
        ValidateLifetime = jwtSection.GetValue<bool>("ValidateLifetime"),
        ValidateIssuerSigningKey = jwtSection.GetValue<bool>("ValidateIssuerSigningKey"),
        ValidIssuer = jwtSection["Issuer"],
        ValidAudience = jwtSection["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]))
    };
});







builder.Services.AddAuthorization();



builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<ITokenService, TokenService>();

builder.Services.AddTransient<IHashing, BCryptHashing>();
builder.Services.AddTransient<IToken, Token>();
builder.Services.AddSingleton<IEncode, UTF8>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}
//

//

app.UseHttpsRedirection();

//Use Cors
//app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

//use for minial api
//app.ConfigureApi();

//use for upload files
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
                   Path.Combine(Directory.GetCurrentDirectory(), "Photos")),
    RequestPath = "/Photos"
});



app.Run();








