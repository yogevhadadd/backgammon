using HomeAccessControll.Data.DB;
using HomeAccessControll.Data.Interface;
using HomeAccessControll.Data.Service;
using HomeAccessControll.HubConfig;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<HomeDbContext>
    (option => option.UseSqlServer(builder.Configuration.GetConnectionString("ContactConnectionString")));
builder.Services.AddScoped<IHomeRepositoriy, HomeRepositoriy>();
builder.Services.AddSignalR();
builder.Services.AddSwaggerGen();
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
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/request")))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }

        };
    });
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(option =>
{
    option.AddPolicy("CorsPolicy", o => o.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins("http://localhost:4200"));
});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<RequestHub>("/request");
app.Run();
