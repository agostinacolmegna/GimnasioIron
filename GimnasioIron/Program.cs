using Microsoft.EntityFrameworkCore;
using GimnasioIron.Context;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<GimnasioIronContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar servicios MVC + sesiones
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); 
var app = builder.Build();

// Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); 

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
