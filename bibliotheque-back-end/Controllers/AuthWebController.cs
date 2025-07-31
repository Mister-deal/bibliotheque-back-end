using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using bibliotheque_back_end.Models.Service.Interface;
using BibliothequeBackEnd.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace bibliotheque_back_end.Controllers;

public class AuthWebController : Controller
    {
        private readonly IAuthService _authService;

        public AuthWebController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Affiche la page de connexion.
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            // Si l'utilisateur est déjà authentifié, redirigez-le vers le tableau de bord
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View(); // Cherchera Views/AuthWeb/Register.cshtml
        }
        [HttpPost]
    [ValidateAntiForgeryToken] // Très recommandé pour les formulaires POST
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            // Si le modèle n'est pas valide (ex: champs requis manquants, mots de passe non identiques),
            // retourne la vue avec les erreurs de validation.
            return View(model);
        }

        try
        {
            // TODO: Appelez ici votre service d'authentification ou de gestion des utilisateurs
            // pour enregistrer le nouvel utilisateur.
            // Cette méthode devrait prendre le RegisterViewModel et créer un nouvel utilisateur
            // dans votre base de données, potentiellement en hachant le mot de passe.
            // Exemple (vous devrez implémenter RegisterUser dans IAuthService ou un nouveau IUserService) :
            bool registrationSuccess = await _authService.RegisterUser(model.Nom, model.Prenom, model.Email, model.Password);

            if (registrationSuccess)
            {
                // _logger.LogInformation("Utilisateur {Email} enregistré avec succès.", model.Email);
                TempData["SuccessMessage"] = "Votre compte a été créé avec succès ! Veuillez vous connecter.";
                return RedirectToAction("Login", "AuthWeb"); // Redirige vers la page de connexion
            }
            else
            {
                // Si l'enregistrement échoue pour une raison métier (ex: email déjà utilisé)
                ModelState.AddModelError(string.Empty, "L'inscription a échoué. L'adresse email est peut-être déjà utilisée ou une autre erreur est survenue.");
                return View(model);
            }
        }
        catch (InvalidOperationException ex)
        {
            // Gère les erreurs spécifiques que votre service pourrait lever
            ModelState.AddModelError(string.Empty, ex.Message);
            // _logger.LogError(ex, "Erreur d'opération lors de l'inscription de l'utilisateur {Email}.", model.Email);
            return View(model);
        }
        catch (Exception ex)
        {
            // Gère toutes les autres erreurs inattendues
            ModelState.AddModelError(string.Empty, $"Une erreur inattendue est survenue lors de l'inscription : {ex.Message}");
            // _logger.LogError(ex, "Erreur inattendue lors de l'inscription de l'utilisateur {Email}.", model.Email);
            return View(model);
        }
    }

        /// <summary>
        /// Gère la soumission du formulaire de connexion.
        /// Authentifie l'utilisateur et crée un cookie d'authentification.
        /// </summary>
        /// <param name="model">Le ViewModel contenant l'email/nom d'utilisateur et le mot de passe.</param>
        /// <returns>Redirige vers le tableau de bord en cas de succès, ou retourne la vue de connexion avec des erreurs.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken] // Protège contre les attaques CSRF
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Si le modèle n'est pas valide, retourne la vue avec les erreurs de validation
                return View(model);
            }

            try
            {
                // Tente d'authentifier l'utilisateur via le service d'authentification
                // Ce service doit communiquer avec votre API ou directement avec la base de données
                // et retourner le token JWT si l'authentification est réussie.
                var jwtTokenString = await _authService.AuthenticateUser(model.EmailOrUsername, model.Password);

                if (string.IsNullOrEmpty(jwtTokenString))
                {
                    // Si le token est nul ou vide, les identifiants sont invalides
                    ModelState.AddModelError(string.Empty, "Identifiants invalides (email/nom d'utilisateur ou mot de passe incorrect).");
                    return View(model);
                }

                // Le token JWT est valide, maintenant nous allons l'utiliser pour créer les claims
                // et authentifier l'utilisateur côté client via un cookie.
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(jwtTokenString);

                // Crée une liste de claims à partir du token JWT
                var claims = new List<Claim>();
                claims.AddRange(jwtToken.Claims); // Ajoute tous les claims du JWT

                // Vous pouvez ajouter des claims supplémentaires si nécessaire, par exemple le token lui-même
                claims.Add(new Claim("jwt", jwtTokenString));

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    // Définissez ici les propriétés d'authentification, par exemple la persistance
                    IsPersistent = model.RememberMe // 'RememberMe' du ViewModel
                };

                // Connecte l'utilisateur en utilisant le schéma d'authentification par cookies
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);


                // Redirige vers le tableau de bord après une connexion réussie
                return RedirectToAction("Index", "Dashboard");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Une erreur inattendue est survenue lors de la connexion. Veuillez réessayer.");
                return View(model);
            }
        }

        /// <summary>
        /// Déconnecte l'utilisateur.
        /// </summary>
        [HttpGet]
        [HttpPost] // Permettre GET et POST pour la déconnexion
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth"); // Redirige vers la page de connexion
        }
    }