using DemoRepository.Entities;
using DemoServices;
using DemoServices.Interfaces;
using DemoWebApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


// Configure Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());

    // To Enable authorization using Swagger (JWT)
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme()
    {
        Name = "JWT Authentication",
        Description = "Enter your JWT token in this field",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme
                                }
                            },
                            Array.Empty<string>()
                    }
                });
});


// Configure Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty)),
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? string.Empty,
            ValidAudiences = (builder.Configuration["Jwt:Audience"] ?? string.Empty).Split(','),
            ClockSkew = TimeSpan.Zero
        };
    });

// Configure CORS (Cross-Origin Requests)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.AllowCredentials()
                .AllowAnyHeader()
                .WithOrigins(
                    "https://localhost:44367"
                );
        }
        else
        {
            policy.AllowCredentials()
                .AllowAnyHeader()
                .WithOrigins(
                    "https://www.demo.com"
                );
        }
    });
});

// Configure HTTP client
// https://medium.com/@zeeshan.mustafa91/use-httpclientfactory-right-way-d3fb877913c0
builder.Services.AddHttpClient<HttpClient>("Generic", x => { x.Timeout = TimeSpan.FromSeconds(5); })
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        using (var handler = new HttpClientHandler())
        {
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true;
            return handler;
        }
    });
builder.Services.AddHttpClient<HttpClient>("AllowRedirect", x => { x.Timeout = TimeSpan.FromSeconds(5); })
    .ConfigurePrimaryHttpMessageHandler(() =>
    {
        using (var handler = new HttpClientHandler())
        {
            handler.AllowAutoRedirect = true;
            handler.MaxAutomaticRedirections = 2;
            return handler;
        }
    });

builder.Services.AddDbContext<DemoSqlContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddMemoryCache();

// Register services
builder.Services.AddTransient<IAuditService, AuditService>();
builder.Services.AddTransient<IClientService, ClientService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IWorkItemService, WorkItemService>();

ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<JwtMiddleware>();
app.UseHttpsRedirection();
app.MapSwagger().RequireAuthorization();
app.UseCors();
app.UseAuthorization();
app.MapControllers();

app.Run();
