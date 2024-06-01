using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

string[] allowedDomains = builder.Configuration["AppSettings:AllowedOrigions"].Split(",");
string cognitoAppClientId = builder.Configuration["AppSettings:Cognito:AppClientId"].ToString();
string cognitoUserPoolId = builder.Configuration["AppSettings:Cognito:UserPoolId"].ToString();
string cognitoAWSRegion = builder.Configuration["AppSettings:Cognito:AWSRegion"].ToString();

string validIssuer = $"https://cognito-idp.{cognitoAWSRegion}.amazonaws.com/{cognitoUserPoolId}";
string validAudience = cognitoAppClientId;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CORSPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
