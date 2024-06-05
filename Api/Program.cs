using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Data;
using Api.Interfaces;
using Api.Repository;
using Models;
 
 
var builder = WebApplication.CreateBuilder(args);
 
string[] allowedDomains = builder.Configuration["AppSettings:AllowedOrigions"].Split(",");
var cognitoAppClientId = builder.Configuration["AppSettings:Cognito:AppClientId"];
var cognitoUserPoolId = builder.Configuration["AppSettings:Cognito:UserPoolId"];
var cognitoAWSRegion = builder.Configuration["AppSettings:Cognito:AWSRegion"];
 
string validIssuer = $"https://cognito-idp.{cognitoAWSRegion}.amazonaws.com/{cognitoUserPoolId}";
string validAudience = cognitoAppClientId;
 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);
 
var host = builder.Configuration["AppSettings:DB:Host"];
var port = builder.Configuration["AppSettings:DB:Port"];
var database = builder.Configuration["AppSettings:DB:Name"];
var username = builder.Configuration["AppSettings:DB:Username"];
var password = builder.Configuration["AppSettings:DB:Password"];
 
string connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";
 
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString)
);
 
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPipelineRepository, PipelineStageRepository>();
builder.Services.AddScoped<IRolesRepository, RolesRepository>();
builder.Services.AddScoped<ISalesOpportunitiesRepository, SalesOpportunitiesRepository>();
 
builder.Services.AddControllers();
 
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
    {
        options.Authority = validIssuer;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidAudience = validAudience,
            ValidateAudience = true,
            RoleClaimType = "cognito:groups"
        };
    });
 
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(UserRoles.Manager, policy => policy.RequireRole(UserRoles.Manager))
    .AddPolicy(UserRoles.SalesRep, policy => policy.RequireRole(UserRoles.SalesRep))
    .AddPolicy(UserRoles.GeneralUser, policy => policy.RequireRole(UserRoles.GeneralUser));
 
 
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "CRM Tool", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
 
    option.EnableAnnotations();
});
 
var app = builder.Build();
app.UseCors(builder => builder
     .AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());
 
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