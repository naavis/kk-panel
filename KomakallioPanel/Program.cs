using Hangfire;
using KomakallioPanel;
using KomakallioPanel.ImageTools;
using KomakallioPanel.JobManagement;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<LocationOptions>(
    builder.Configuration.GetSection(LocationOptions.Location));

builder.Services.AddHangfire(config =>
    config.UseInMemoryStorage()
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings());
builder.Services.AddHangfireServer();

builder.Services.AddSingleton<IImageManager, ImageManager>();
builder.Services.AddTransient<IImageDownloader, ImageDownloader>();
builder.Services.AddTransient<IImageUpdater, ImageUpdater>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.ConfigureJobs();

app.Run();
