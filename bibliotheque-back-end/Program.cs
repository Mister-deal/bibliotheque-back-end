using System.Text;
using bibliotheque_back_end.Data;
using bibliotheque_back_end.Models.repositery;
using bibliotheque_back_end.Models.Service;
using bibliotheque_back_end.Models.Service.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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
    // Activer les annotations Swagger
    c.EnableAnnotations();
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Pour lire le mot de passe dans un fichier.txt 
var password = File.ReadAllText("password.txt").Trim();

// Connection PostgreSQL
var connectionString = $"Host=localhost;Database=bibliotheque_db;Username=postgres;Password={password};Port=5432;Include Error Detail=true;Trust Server Certificate=true";
builder.Services.AddDbContext<BibliothequeDb>(options =>
    options.UseNpgsql(connectionString));

// Enregistrement des services de la couche 'Service'
builder.Services.AddScoped<IEmployeService, EmployeService>();
builder.Services.AddScoped<IEmpruntService, EmpruntService>();
builder.Services.AddScoped<ILivreService, LivreService>();
builder.Services.AddScoped<IMembreService, MembreService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

// Enregistrement des services de la couche 'Repository'
builder.Services.AddScoped<IEmployeRepository, EmployeRepository>();
builder.Services.AddScoped<IEmpruntRepository, EmpruntRepository>();
builder.Services.AddScoped<ILivreRepository, LivreRepository>();
builder.Services.AddScoped<IMembreRepository, MembreRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();

//Enregistrement service AuthService JWT
builder.Services.AddScoped<IAuthService, AuthService>();

//ajout logique JWT

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

    }).AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey =
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]!)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
        };
    });
builder.Services.AddAuthorization();


var app = builder.Build();

// Migration automatique à chaque démarrage TODO: à chercher comment faire pour auto migrer
//using (var scope = app.Services.CreateScope())
//{
//    try
//    {
//        var db = scope.ServiceProvider.GetRequiredService<BibliothequeDb>();

//        // Vérifier la connexion et migrer
//        Console.WriteLine(" Migration en cours...");
//        db.Database.Migrate();
//        Console.WriteLine(" Migration réussie !");

//        // Vérifier si des données existent déjà
//        bool hasData = db.Employes.Any();
//        Console.WriteLine($" Données existantes: {hasData}");

//        if (!hasData)
//        {
//            Console.WriteLine(" Insertion des données de test...");
//            var connection = db.Database.GetDbConnection();
//            connection.Open();

//            var sqlFile = Path.Combine(AppContext.BaseDirectory, "seed.sql");
//            if (File.Exists(sqlFile))
//            {
//                var sqlScript = File.ReadAllText(sqlFile);
//                var commands = sqlScript.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

//                foreach (var commandText in commands)
//                {
//                    var trimmedCmd = commandText.Trim();
//                    if (!string.IsNullOrEmpty(trimmedCmd))
//                    {
//                        using var command = connection.CreateCommand();
//                        command.CommandText = trimmedCmd;
//                        command.ExecuteNonQuery();
//                    }
//                }
//                Console.WriteLine(" Données insérées !");
//            }
//            else
//            {
//                Console.WriteLine(" Fichier seed.sql introuvable");
//            }

//            connection.Close();
//        }
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine($" Erreur de migration: {ex.Message}");
//        Console.WriteLine(" L'application continue sans migration...");
//    }
//}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// -- SWAGGER --
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biblitheque-Simplon v1");
        //c.RoutePrefix = ""; est responsable du swagger qui se lance au démarrage en route principale
    });
}

app.UseAuthentication(); // Active l'authentification jwt
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
//test jwt