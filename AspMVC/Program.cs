using AspMVC.Data;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//builder.Services.AddSingleton<AspMVC.Services.PostsRepository>();
//builder.Services.AddTransient<AspMVC.Services.UploadMedia>();

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

using (var scrope = app.Services.CreateScope())
{
    var db = scrope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".mkv"] = "video/x-matroska";

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});
app.MapControllerRoute(name: "default", pattern: "{controller=Posts}/{action=Index}/{id?}");

app.Run();