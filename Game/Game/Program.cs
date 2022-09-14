
using Game.HubConfig;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Add services to the container.

builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .WithOrigins("http://localhost:4200")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});
builder.Services.AddEndpointsApiExplorer();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseCors("CorsPolicy");

app.MapControllers();
app.MapFallbackToFile("index.html"); ;

app.MapHub<GameHub>("/game");
app.Run();
