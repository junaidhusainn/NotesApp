using Microsoft.EntityFrameworkCore;
using NotesApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add MVC services
builder.Services.AddControllersWithViews();

// Add database context with SQLite
builder.Services.AddDbContext<NotesDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Ensure database is created and migrations are applied
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<NotesDbContext>();
    context.Database.Migrate();
}

// Configure middleware pipeline
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Set default route to Notes/Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Notes}/{action=Index}/{id?}");

app.Run();
