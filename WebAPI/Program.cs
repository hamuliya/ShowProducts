using DataAccess.DbAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using WebAPI.Global.Encode;
using WebAPI.Global.ExceptionFilter;
using WebAPI.Global.Hashing;

using WebAPI.Global.Token;

// Create a logger factory
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder
        .AddConsole()
        .AddFilter(level => level >= LogLevel.Debug);
});

var builder = WebApplication.CreateBuilder(args);

// Add the logger factory to the container
builder.Services.AddSingleton(loggerFactory);

// Add Controllers
builder.Services.AddControllers();

//Add Swagger/OpenAPI
//The AddEndpointsApiExplorer method is used to add the IApiDescriptionGroupCollectionProvider service to the application's service container.
//This service is used to provide information about the application's endpoints for use in generating the OpenAPI specification.

builder.Services.AddEndpointsApiExplorer();

//The AddSwaggerGen method is used to configure the Swagger generation options. The method takes a callback function that is used to configure the options.
//Inside the callback function, the AddSecurityDefinition method is used to add a security definition named "oauth2" to the options.
//The security definition is of type OpenApiSecurityScheme and it describes the standard authorization header using the Bearer scheme,
//it also sets the In property to ParameterLocation.Header, Name to "Authorization" and Type to SecuritySchemeType.ApiKey

//The OperationFilter<SecurityRequirementsOperationFilter>() method is used to add an operation filter
//that sets the security requirements for the API based on the security definitions that were added to the options.
//This filter will add the "oauth2" security definition to all the endpoints that are decorated with the Authorize attribute.

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

//This code creates a new instance of the IConfiguration interface using the ConfigurationBuilder class.
//The AddJsonFile method is used to load configuration data from a JSON file named "appsettings.json".
//The optional parameter is set to true, which means that the file is not required and the application will still run if it is not found.
//The reloadOnChange parameter is set to true, which means that the configuration will be automatically reloaded if the file changes.
//The AddEnvironmentVariables method is used to load configuration data from environment variables.
//The AddCommandLine method is used to load configuration data from command line arguments passed to the application.
//Finally, the Build method is used to create an instance of the IConfiguration interface
//with all of the configuration data loaded from the various sources.


// Add Authentication and Authorization using JWT

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

// Add Services

//Transient objects are always different; a new instance is provided to every controller and every service.

//Scoped objects are the same within a request, but different across different requests.

//Singleton objects are the same for every object and every request.


builder.Services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
builder.Services.AddSingleton<IProductService, ProductService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<ITokenService, TokenService>();
builder.Services.AddTransient<IHashing, BCryptHashing>();
builder.Services.AddTransient<IToken, Token>();
builder.Services.AddTransient<IEncode, UTF8>();
builder.Services.AddScoped<IExceptionFilter,ExceptionFilter>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// Enable HTTPS redirection for all requests
app.UseHttpsRedirection();

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Map controllers for handling HTTP requests
app.MapControllers();

// Serve files in the "Photos" directory
// the FileProvider property is used to specify where the files are located on the file system, 
//and the RequestPath property is used to specify the URL path that should be used to access the files.
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Photos")),
    RequestPath = "/Photos"
});

// Run the application
app.Run();


