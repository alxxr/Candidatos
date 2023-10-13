using Microsoft.EntityFrameworkCore;
using DB;
using Candidatos;

#region Builder
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddServices(builder.Configuration); // Llama al método de extensión sin pasar 'args'
#endregion

#region Container
// Add services to the container.
builder.Services.AddControllersWithViews();
#endregion

#region App
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default",pattern: "{controller=Candidates}/{action=Index}/{id?}");

app.Run();
#endregion