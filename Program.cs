using Expense_WEB.Controllers;
using Expense_WEB.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure services using extension methods
builder.Services.ConfigureControllers();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureDatabase(builder.Configuration);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureCors();
builder.Services.ConfigureCustomServices();

var app = builder.Build();

// Configure pipeline using extension methods
app.ConfigureDevelopmentPipeline();
app.ConfigureProductionPipeline();
app.ConfigureMiddleware();

// Configure endpoints using extension methods
app.ConfigureAuthEndpoints(builder.Configuration);
app.MapAccountEndpoints();
// Initialize database
app.InitializeDatabase();

app.Run();