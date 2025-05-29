using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Expense_WEB;
using Expense_WEB.Data;
using Expense_WEB.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger with JWT support
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Expense Tracker API", 
        Version = "v1",
        Description = "API for managing budgets and expenses with JWT authentication"
    });
    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Add Entity Framework
builder.Services.AddDbContext<BudgetContext>(options =>
    options.UseInMemoryDatabase("ExpenseTrackerDB"));

// Add JWT Authentication
var jwtKey = builder.Configuration["JwtSettings:SecretKey"] ?? "YourSuperSecretKeyThatShouldBeChangedInProduction123!";
var key = Encoding.ASCII.GetBytes(jwtKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Add Authorization with custom policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireReadPermission", policy =>
        policy.RequireClaim("permission", "READ"));
    
    options.AddPolicy("RequireWritePermission", policy =>
        policy.RequireClaim("permission", "WRITE"));
    
    options.AddPolicy("RequireDeletePermission", policy =>
        policy.RequireClaim("permission", "DELETE"));
    
    options.AddPolicy("RequireAdminRole", policy =>
        policy.RequireClaim("role", "ADMIN"));
});

// Add custom services
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Expense Tracker API V1");
        c.RoutePrefix = string.Empty; 
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAngularApp");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BudgetContext>();
    SeedData.Initialize(context);
}

app.Run();