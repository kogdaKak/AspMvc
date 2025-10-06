using AspMVC.Data;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<AspMVC.Services.PostsRepository>();

builder.Services.AddTransient<AspMVC.Services.UploadMedia>();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// Создаем FileExtensionContentTypeProvider для MKV
var provider = new FileExtensionContentTypeProvider();
provider.Mappings[".mkv"] = "video/x-matroska";

app.UseStaticFiles(new StaticFileOptions
{
    ContentTypeProvider = provider
});
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Posts}/{action=Index}/{id?}"
);

app.Run();
