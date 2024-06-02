using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Data;

var builder = WebApplication.CreateBuilder(args);

string[] allowedDomains = builder.Configuration["AppSettings:AllowedOrigions"].Split(",");
string cognitoAppClientId = builder.Configuration["AppSettings:Cognito:AppClientId"].ToString();
string cognitoUserPoolId = builder.Configuration["AppSettings:Cognito:UserPoolId"].ToString();
string cognitoAWSRegion = builder.Configuration["AppSettings:Cognito:AWSRegion"].ToString();

string validIssuer = $"https://cognito-idp.{cognitoAWSRegion}.amazonaws.com/{cognitoUserPoolId}";
string validAudience = cognitoAppClientId;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

builder.Services.AddCors(item =>
{
    item.AddPolicy("CORSPolicy", builder =>
    {
        builder.WithOrigins(allowedDomains)
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

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
});
 var host = Environment.GetEnvironmentVariable("CRM_DB_HOST");
    var port = Environment.GetEnvironmentVariable("CRM_DB_PORT");
    var database = Environment.GetEnvironmentVariable("CRM_DB_DATABASE");
    var username = Environment.GetEnvironmentVariable("CRM_DB_USERNAME");
    var password = Environment.GetEnvironmentVariable("CRM_DB_PASSWORD");

    var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";


    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(connectionString));


builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CORSPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();