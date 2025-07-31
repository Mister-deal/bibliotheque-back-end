using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using bibliotheque_back_end.Models.Service.Interface;
using Microsoft.IdentityModel.Tokens;

namespace bibliotheque_back_end.Models.Service;

public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmployeService _employeService; 
        private readonly IMembreService _membreService;

        public AuthService(IConfiguration configuration, IEmployeService employeService, IMembreService membreService)
        {
            _configuration = configuration;
            _employeService = employeService;
            _membreService = membreService;
        }

        public async Task<string?> AuthenticateUser(string email, string password)
        {
            // Tenter de trouver l'utilisateur en tant qu'Employé
            Employe? employe = await _employeService.GetEmployeeByEmailAsync(email);
            Membre? membre = null;
            bool isValidPassword = false;
            string? userId = null;
            string? userEmail = null;
            string? userRole = null;
            string? userType = null;

            if (employe != null)
            {
                // Vérifier le mot de passe haché de l'employé
                isValidPassword = BCrypt.Net.BCrypt.Verify(password, employe.MotDePasse);
                if (isValidPassword)
                {
                    userId = employe.Id.ToString();
                    userEmail = employe.Email;
                    userRole = employe.Role.ToString();
                    userType = "Employe";
                }
            }
            else 
            {
                membre = await _membreService.GetMemberByEmailAsync(email);
                if (membre != null)
                {
                    isValidPassword = BCrypt.Net.BCrypt.Verify(password, membre.MotDePasse);
                    if (isValidPassword)
                    {
                        userId = membre.Id.ToString();
                        userEmail = membre.Email;
                        userRole = "Membre";
                        userType = "Membre";
                    }
                }
            }

            if (!isValidPassword || (employe == null && membre == null))
            {
                return null;
            }

            // --- Génération du token JWT si l'authentification est réussie ---
            var jwtConfig = _configuration.GetSection("JwtConfig");
            var key = Encoding.ASCII.GetBytes(jwtConfig["Key"] ?? throw new InvalidOperationException("JWT Key is not configured."));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId!),
                new Claim(ClaimTypes.Email, userEmail!),      
            };

            // Ajouter le rôle si disponible
            if (!string.IsNullOrEmpty(userRole))
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            if (employe != null)
            {
                claims.Add(new Claim(ClaimTypes.GivenName, employe.Prenom));
                claims.Add(new Claim(ClaimTypes.Surname, employe.Nom));
            }
            else if (membre != null)
            {
                claims.Add(new Claim(ClaimTypes.GivenName, membre.Prenom));
                claims.Add(new Claim(ClaimTypes.Surname, membre.Nom));
            }


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtConfig["TokenValidityMins"] ?? "20")),
                Issuer = jwtConfig["Issuer"],
                Audience = jwtConfig["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> RegisterUser(string nom, string prenom, string email, string password)
        {
            // 1. Vérifier si l'email existe déjà
            Membre? existingMembre = await _membreService.GetMemberByEmailAsync(email);
            if (existingMembre != null)
            {
                throw new InvalidOperationException("Cette adresse email est déjà utilisée.");
            }

            // 2. Hacher le mot de passe
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            // 3. Créer un nouvel objet Membre
            Membre newMembre = new Membre
            {
                Nom = nom,
                Prenom = prenom,
                Email = email,
                MotDePasse = hashedPassword // Stocker le mot de passe haché
            };

            // 4. Ajouter le nouveau membre à la base de données via le service
            // Assurez-vous que IMembreService a une méthode AddMemberAsync qui retourne le membre créé ou un booléen
            try
            {
                await _membreService.AddMemberAsync(newMembre, password); // Assurez-vous que cette méthode existe et est implémentée
                return true;
            }
            catch (Exception ex)
            {
                // Log de l'exception si quelque chose ne va pas lors de l'ajout à la DB
                Console.WriteLine($"Erreur lors de l'ajout du nouveau membre à la base de données: {ex.Message}");
                return false; // Indique que l'enregistrement a échoué
            }
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }