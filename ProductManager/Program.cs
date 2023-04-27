using ProductManager.BusinessLogic.ServiceCollections;
using ProductManager.DBMap;

var builder = WebApplication.CreateBuilder(args);

// Get the configuration instance
///var configuration = builder.Build().Services.GetRequiredService<IConfiguration>();

// Register services
builder.Services.ConfigureServices(builder.Configuration);

// Register the SignalRHub
builder.Services.AddSignalR();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc().AddRazorRuntimeCompilation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Product}/{action=Index}/{id?}");

app.Run();