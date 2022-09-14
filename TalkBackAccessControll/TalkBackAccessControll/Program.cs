using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TalkBackAccessControll.Date.DB;
using TalkBackAccessControll.Date.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddAuthentication(opt => {
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "https://localhost:7048",
            ValidAudience = "https://localhost:7048",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
        };
    });
builder.Services.AddCors(options =>
{
    options.AddPolicy("EnableCORS", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod().WithOrigins("http://localhost:4200");
    });
});
builder.Services.AddControllers();
builder.Services.AddDbContext<TalkBackDbContext>
    (option => option.UseSqlServer(builder.Configuration.GetConnectionString("TalkBckConnectionString")));
builder.Services.AddScoped<IUserRepository, UserRepositoriy>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});
var app = builder.Build();
app.UseHttpsRedirection();
app.UseCors("EnableCORS");
app.UseAuthentication();
//app.UseStaticFiles(new StaticFileOptions()
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
//    RequestPath = new PathString("/Resources")
//});
app.UseAuthorization();
app.MapControllers();
app.Run();