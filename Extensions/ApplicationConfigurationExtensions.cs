using Expense_WEB.Data;

namespace Expense_WEB.Extensions
{
    public static class ApplicationConfigurationExtensions
    {
        public static WebApplication ConfigureDevelopmentPipeline(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Expense Tracker API V1");
                    c.RoutePrefix = string.Empty;
                });
            }

            return app;
        }

        public static WebApplication ConfigureProductionPipeline(this WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                // Add production-specific middleware here
                // app.UseHttpsRedirection();
            }

            return app;
        }

        public static WebApplication ConfigureMiddleware(this WebApplication app)
        {
            app.UseCors("AllowAngularApp");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            return app;
        }

        public static WebApplication InitializeDatabase(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                // Initialize ApplicationDbContext
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                try
                {
                    context.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Database creation error: {ex.Message}");
                }

                // Initialize BudgetContext and seed data
                var budgetContext = scope.ServiceProvider.GetRequiredService<BudgetContext>();
                try
                {
                    SeedData.Initialize(budgetContext);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Seeding error: {ex.Message}");
                }
            }

            return app;
        }
    }
}