using boklista_api.Data;
using boklista_api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Scalar.AspNetCore;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var AllowSpecificOrigins = "_allowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

var port = Environment.GetEnvironmentVariable("PORT") ?? "5163";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "http://localhost:4200";

//var frontendUrl = builder.Configuration["FrontendUrl"];

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200", "https://boklista-frontend.vercel.app")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                      });
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

if (connUrl != null)
{
    var uri = new Uri(connUrl);
    var userInfo = uri.UserInfo.Split(':');
    var connString = $"Host={uri.Host};Port={uri.Port};" +
                     $"Username={userInfo[0]};Password={userInfo[1]};" +
                     $"Database={uri.LocalPath.TrimStart('/')};" +
                     $"SSL Mode=Require;Trust Server Certificate=true";
    builder.Services.AddDbContext<BookListDbContext>(opt =>
        opt.UseNpgsql(connString));
}
else
{
    builder.Services.AddDbContext<BookListDbContext>(options =>
        options.UseNpgsql(connectionString));
}



builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetValue<string>("AppSettings:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("AppSettings:Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
        }
    );

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IQuoteService, QuoteService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider
        .GetRequiredService<BookListDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseHttpsRedirection();
}



app.UseCors(AllowSpecificOrigins);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
