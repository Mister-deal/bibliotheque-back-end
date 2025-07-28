using System.Text.Json.Serialization;
using bibliotheque_back_end.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Partie Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

//paramétrage sqlite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BibliothequeDb>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

// Migration automatique à chaque démarrage
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BibliothequeDb>();
    db.Database.Migrate();

    // Check si la table Employes contient déjà des données
    bool hasData = db.Employes.Any();

    if (!hasData)
    {
        var connection = db.Database.GetDbConnection();
        connection.Open();

        var sqlFile = Path.Combine(AppContext.BaseDirectory, "seed.sql");
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
        }

        connection.Close();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//redirection

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Partie Swagger 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
