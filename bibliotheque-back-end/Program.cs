using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using bibliotheque_back_end.Data;
using bibliotheque_back_end.Models.repositery;
using bibliotheque_back_end.Models.Service;
using bibliotheque_back_end.Models.Service.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// -- SWAGGER --
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Bibliothèque SIMPLON",
        Version = "V.1",
        Description = "Application de gestion numérique pour une bibliothèque avec ASP.Net (MVC)",
    });
    c.EnableAnnotations();
});

// JSON options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        //permet d'éviter les boucles dans les requêtes JSON
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });

// Lecture du mot de passe depuis un fichier
//le password.txt doit être créé, car il ne peut être récupéré dans le projet. il doit contenir le mot de passe d'un superUser de PostgreSql
var password = File.ReadAllText("password.txt").Trim();

// Connexion PostgreSQL
var connectionString = $"Host=localhost;Database=bibliotheque_db;Username=postgres;Password={password};Port=5432;Include Error Detail=true;Trust Server Certificate=true";
builder.Services.AddDbContext<BibliothequeDb>(options =>
    options.UseNpgsql(connectionString));

// Services métier (Service layer)
builder.Services.AddScoped<IEmployeService, EmployeService>();
builder.Services.AddScoped<IEmpruntService, EmpruntService>();
builder.Services.AddScoped<ILivreService, LivreService>();
builder.Services.AddScoped<IMembreService, MembreService>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IStatistiquesService, StatistiquesService>();

// Repository layer
builder.Services.AddScoped<IEmployeRepository, EmployeRepository>();
builder.Services.AddScoped<IEmpruntRepository, EmpruntRepository>();
builder.Services.AddScoped<ILivreRepository, LivreRepository>();
builder.Services.AddScoped<IMembreRepository, MembreRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

// AuthService
builder.Services.AddScoped<IAuthService, AuthService>();

// Authentification (Cookie + JWT)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    //ajout cookie Authweb pour validation JWT et conservation sous forme cookies
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.LoginPath = "/AuthWeb/Login";
    options.LogoutPath = "/AuthWeb/Logout";
    options.AccessDeniedPath = "/AuthWeb/AccessDenied";
    options.SlidingExpiration = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
})
    //paramètres JWT pour initialisation
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
        ValidAudience = builder.Configuration["JwtConfig:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Migration automatique en relation à seed.sql avec vérification si BDD dispose données ou non
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<BibliothequeDb>();

        Console.WriteLine(" Migration en cours...");
        db.Database.Migrate();
        Console.WriteLine(" Migration réussie !");

        bool hasData = db.Employes.Any();
        Console.WriteLine($" Données existantes: {hasData}");

        if (!hasData)
        {
            Console.WriteLine(" Insertion des données de test...");
            var connection = db.Database.GetDbConnection();
            connection.Open();

            var sqlFile = Path.Combine(AppContext.BaseDirectory, "Scripts", "seed.sql");
            Console.WriteLine($"Attempting to load seed.sql from: {sqlFile}");
            if (File.Exists(sqlFile))
            {
                var sqlScript = File.ReadAllText(sqlFile);
                var commands = sqlScript.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var commandText in commands)
                {
                    var trimmedCmd = commandText.Trim();
                    if (!string.IsNullOrEmpty(trimmedCmd))
                    {
                        using var command = connection.CreateCommand();
                        command.CommandText = trimmedCmd;
                        command.ExecuteNonQuery();
                    }
                }
                Console.WriteLine(" Données insérées !");
            }
            else
            {
                Console.WriteLine(" Fichier seed.sql introuvable");
            }

            connection.Close();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($" Erreur de migration: {ex.Message}");
        Console.WriteLine(" L'application continue sans migration...");
    }
}

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biblitheque-Simplon v1");
    });
}
//auth pour jwt
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
