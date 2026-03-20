using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using mifichaje.Aplicacion.Interfaces;
using mifichaje.Aplicacion.Services;
using mifichaje.Infraestructura.Database;
using mifichaje.Infraestructura.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ===============================
// Controllers + OpenAPI
// ===============================
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// ===============================
// 🔹 CORS (IMPORTANTE para MAUI)
// ===============================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// ===============================
// 🔹 CONNECTION STRING
// ===============================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(new DBConnectionFactory(connectionString));

// ===============================
// 🔹 REPOS / SERVICES
// ===============================
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<ProductoService>();

builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<UsuarioService>();

builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<RolService>();

builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<AuthService>();

builder.Services.AddScoped<IFichajeRepository, FichajeRepository>();
builder.Services.AddScoped<FichajeService>();

// ===============================
// 🔹 JWT AUTH
// ===============================
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwt = builder.Configuration.GetSection("Jwt");
        var key = jwt["Key"] ?? throw new Exception("Falta Jwt:Key en appsettings.json");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),

            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();

// ===============================
// 🔹 PUERTO DINÁMICO (RENDER)
// ===============================
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.UseUrls($"http://*:{port}");

var app = builder.Build();

// ===============================
// 🔹 OPENAPI
// ===============================
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// ===============================
// 🔹 PIPELINE
// ===============================
app.UseRouting();

// CORS antes de auth
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "API funcionando perfectamente perfectamente");

app.Run();

