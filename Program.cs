using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Server.Data;
using Server.Models;
using Server.common.Middlewares;
using Server.Interfaces.Services;
using Server.Services;
using Server.Interfaces.Repositories;
using Server.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// builder.Logging.ClearProviders();
// builder.Logging.AddConsole();
// builder.Logging.AddDebug();

builder.Services.AddControllers()/*.AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve )*/;
// configure JSON serialization to be case-insensitive
// builder.Services.AddControllers()
//     .AddJsonOptions(options =>
//     {
//         options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
//     });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("connection string not found");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
);

//IdentityFramework
builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//Jwt configuration
var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>() ?? throw new InvalidOperationException("jwt issuer not found");
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>() ?? throw new InvalidOperationException("jwt key not found");
// var webAppUrl = builder.Configuration.GetValue<string>("Frontend:Url") ?? throw new InvalidOperationException("FrontEnd url not found");
var mobileAppUrl = builder.Configuration.GetValue<string>("MobileApp:Url") ?? throw new InvalidOperationException("Mobile App url not found");

// creating CORS policy
builder.Services.AddCors(options =>
{
    // options.AddPolicy("CorsWebAppPolicy", corsBuilder =>
    // {
    //     corsBuilder
    //     .WithOrigins(webAppUrl/*[frontEndUrl, mobileAppUrl]*/) // Allow specific origin or an array[] of origins
    //     .AllowAnyHeader()
    //     .AllowAnyMethod()
    //     .AllowCredentials(); // for cookie...
    // });
    options.AddPolicy("CorsMobileAppPolicy", corsBuilder =>
    {
        corsBuilder
        .WithOrigins(mobileAppUrl/*[frontEndUrl, mobileAppUrl]*/) // Allow specific origin or an array[] of origins
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials(); // for cookie...
    });
});

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        options.Cookie.Name = "access_token";
        options.Cookie.HttpOnly = true; // XSS protection by preventing browser javascript from accessing the cookie
        // options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Always use HTTPS
        options.Cookie.SameSite = SameSiteMode.Strict; // CSRF protection
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
        options.Events = new JwtBearerEvents
        {
        /*
            Since Im storing the JWT token in a cookie rather than as a Bearer token in the Authorization header
            (where the JwtBearer middleware typically expects it), I implemented a custom extraction logic.
            This logic will retrieve the token from the cookie in the HTTP request when a user accesses a route that requires authorization.
        */
            OnMessageReceived = context =>
            {
                // if cookie named access_token was found, it will be stored inside a variable named accessToken
                context.Request.Cookies.TryGetValue("access_token", out var accessToken);
                if (!string.IsNullOrEmpty(accessToken))
                    // set the token for authorization
                    context.Token = accessToken;

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                return Task.CompletedTask;
            }
        };
    });
    /*.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
    {
        options.Authority = $"https://login.microsoftonline.com/{builder.Configuration["AzureAd:TenantId"]}";
        options.ClientId = builder.Configuration["AzureAd:ClientId"];
        options.ClientSecret = builder.Configuration["AzureAd:ClientSecret"]; // Only if required
        options.ResponseType = OpenIdConnectResponseType.Code; // Use "id_token" for implicit flow or "code" for authorization code flow
        options.SaveTokens = true; // Save tokens for further use
        options.CallbackPath = builder.Configuration["AzureAd:CallbackPath"] ?? "/signin-oidc";
    });*/

// makes the HttpContext available for dependency injection
builder.Services.AddHttpContextAccessor();

// registering services for dependency injection with AddScoped method
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IGraphApiService, GraphApiService>();

builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();

builder.Services.AddScoped<IProductPresentationRepository, ProductPresentationRepository>();
builder.Services.AddScoped<IProductPresentationService, ProductPresentationService>();

builder.Services.AddScoped<IProductSlideRepository, ProductSlideRepository>();
builder.Services.AddScoped<IProductSlideService, ProductSlideService>();

builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();

builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services.AddHttpClient();

var app = builder.Build();

app.Use(async (context, next) =>
{
    Console.WriteLine($"{context.Request.Method} {context.Request.Path}");
    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Use((context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 200;
        return Task.CompletedTask;
    }
    return next();
});

app.UseMiddleware<ExceptionHandlerMiddleware>();

// app.UseCors("CorsWebAppPolicy");
app.UseCors("CorsMobileAppPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

/* TESTS

    order of creation between appointment, productPresentation, productSlides:
    - appointment created on appointment click -> independent
    - productPresentation created on productClick -> dependent on appointment
    - productSlides are created when their productPresentation is created -> dependent on productPresentation

    expected results by case (user, doctor, product):
    - user never did visited a doctro -> never did a presentation:
        - appointments all appointments should appear not-visited
        - presentation products should appear as not-presented
    - user did a visit today with doctor = 1, presented 3 slides of product = 1 that has 5 slides:
        - appointment with doctor = 1 as the date of today should appear Visited
        - presentation product = 1 should appear presented
    - user have a visit today with doctor = 1: // DO A DATA REPRESENTATION OF THIS
        - appointment with doctor = 1 should appear not-visited, but previous one shoud appear visited
        - presentation proudct = 1 should appear as continue
    - user continued the presentation of 5 slides of product = 1 with doctor = 1 // DO A DATA REPRESENTATION OF THIS
        - appointment should appear visited
        - presentation product = 1 should appear as presented
    - a day passed and user have a visit with doctor = 1:
        - appointment should appear as not-visited
        - product presentation of product = 1 should appear as replay // DO A DATA REPRESENTATION OF THIS
*/