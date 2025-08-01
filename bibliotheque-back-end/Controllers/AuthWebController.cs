using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using bibliotheque_back_end.Models.Service.Interface;
using BibliothequeBackEnd.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.IdentityModel.Tokens.Jwt;

namespace bibliotheque_back_end.Controllers
{
    public class AuthWebController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthWebController> _logger;

        public AuthWebController(IAuthService authService, ILogger<AuthWebController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Affichage de la page de connexion
        /// </summary>
        [HttpGet]
        public IActionResult Login()
        {
            // Si l'utilisateur est déjà connecté, le rediriger vers la page d'accueil
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            // Récupérer les messages de succès depuis TempData
            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View();
        }


        /// <summary>
        /// Traitement de la connexion
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Vérifier la validité du modèle selon vos annotations
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _logger.LogInformation("Tentative de connexion pour: {Email}", model.EmailOrUsername);

                // Utiliser votre méthode AuthenticateUser
                var jwtToken = await _authService.AuthenticateUser(model.EmailOrUsername, model.Password);

                if (!string.IsNullOrEmpty(jwtToken))
                {
                    // Extraire les informations du JWT pour créer les claims de session
                    var claims = ExtractClaimsFromJwt(jwtToken);

                    // Créer l'identité pour l'authentification par cookies
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Configurer les propriétés d'authentification selon votre modèle
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = model.RememberMe
                            ? DateTimeOffset.UtcNow.AddDays(30)
                            : DateTimeOffset.UtcNow.AddHours(8),
                        AllowRefresh = true,
                        RedirectUri = Url.Action("Index", "Home")
                    };

                    // Connecter l'utilisateur avec les cookies
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    _logger.LogInformation("Connexion réussie pour: {Email}", model.EmailOrUsername);

                    TempData["SuccessMessage"] = "Connexion réussie ! Bienvenue dans votre espace.";

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    _logger.LogWarning("Échec de connexion pour: {Email}", model.EmailOrUsername);
                    ModelState.AddModelError(string.Empty, "Email ou mot de passe incorrect.");
                    return View(model);
                }
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Erreur d'invalidation lors de la connexion pour: {Email}", model.EmailOrUsername);
                ModelState.AddModelError(string.Empty, "Une erreur s'est produite lors de la connexion. Veuillez réessayer.");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur générale lors de la connexion pour: {Email}", model.EmailOrUsername);
                ModelState.AddModelError(string.Empty, "Une erreur inattendue s'est produite. Veuillez réessayer.");
                return View(model);
            }
        }

        /// <summary>
        /// Affichage de la page d'inscription
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            // Si l'utilisateur est déjà connecté, le rediriger vers le tableau de bord
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        /// <summary>
        /// Traitement de l'inscription
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _logger.LogInformation("Tentative d'inscription pour: {Email}", model.Email);

                // Appeler votre service d'inscription (à implémenter dans IAuthService)
                var result = await _authService.RegisterUser(model.Nom, model.Prenom, model.Email, model.Password);

                if (result)
                {
                    _logger.LogInformation("Inscription réussie pour: {Email}", model.Email);
                    TempData["SuccessMessage"] = "Inscription réussie ! Vous pouvez maintenant vous connecter.";
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Une erreur s'est produite lors de l'inscription.");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'inscription pour: {Email}", model.Email);
                ModelState.AddModelError(string.Empty, "Une erreur s'est produite lors de l'inscription.");
                return View(model);
            }
        }

        /// <summary>
        /// Déconnexion de l'utilisateur (POST)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                if (User.Identity?.IsAuthenticated == true)
                {
                    _logger.LogInformation("Déconnexion de l'utilisateur: {UserName}", User.Identity.Name);

                    // Déconnecter l'utilisateur
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    // Message de confirmation
                    TempData["SuccessMessage"] = "Vous avez été déconnecté avec succès.";
                }

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la déconnexion");
                TempData["ErrorMessage"] = "Une erreur s'est produite lors de la déconnexion.";
                return RedirectToAction("Login");
            }
        }

        /// <summary>
        /// Déconnexion via GET (pour liens directs)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> LogoutGet()
        {
            try
            {
                if (User.Identity?.IsAuthenticated == true)
                {
                    _logger.LogInformation("Déconnexion GET de l'utilisateur: {UserName}", User.Identity.Name);

                    // Déconnecter l'utilisateur
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    // Message de confirmation
                    TempData["SuccessMessage"] = "Vous avez été déconnecté avec succès.";
                }

                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de la déconnexion GET");
                TempData["ErrorMessage"] = "Une erreur s'est produite lors de la déconnexion.";
                return RedirectToAction("Login");
            }
        }

        /// <summary>
        /// Page d'accès refusé
        /// </summary>
        [HttpGet]
        public IActionResult AccessDenied()
        {
            ViewBag.Message = "Vous n'avez pas les autorisations nécessaires pour accéder à cette ressource.";
            ViewBag.ReturnUrl = Request.Headers["Referer"].ToString();
            return View();
        }

        /// <summary>
        /// Vérification du statut d'authentification pour AJAX
        /// </summary>
        [HttpGet]
        public IActionResult CheckAuthStatus()
        {
            var isAuthenticated = User.Identity?.IsAuthenticated ?? false;

            return Json(new
            {
                isAuthenticated = isAuthenticated,
                userName = isAuthenticated ? User.Identity?.Name : null,
                userRole = isAuthenticated ? User.FindFirst(ClaimTypes.Role)?.Value : null,
                userEmail = isAuthenticated ? User.FindFirst(ClaimTypes.Email)?.Value : null,
                userId = isAuthenticated ? User.FindFirst(ClaimTypes.NameIdentifier)?.Value : null
            });
        }

        /// <summary>
        /// Page de confirmation de déconnexion
        /// </summary>
        [HttpGet]
        public IActionResult ConfirmLogout()
        {
            if (!User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Login");
            }

            ViewBag.UserName = User.Identity.Name;
            return View();
        }

        /// <summary>
        /// Extraire les claims du token JWT pour la session cookie
        /// </summary>
        private List<Claim> ExtractClaimsFromJwt(string jwtToken)
        {
            var claims = new List<Claim>();

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jsonToken = tokenHandler.ReadJwtToken(jwtToken);

                // Copier tous les claims du JWT vers notre session
                foreach (var claim in jsonToken.Claims)
                {
                    claims.Add(new Claim(claim.Type, claim.Value));
                }

                // Ajouter des informations supplémentaires pour la session
                claims.Add(new Claim("JwtToken", jwtToken));
                claims.Add(new Claim("LoginTime", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")));
                claims.Add(new Claim("SessionId", Guid.NewGuid().ToString()));

                _logger.LogDebug("Extraction réussie de {Count} claims depuis le JWT", claims.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur lors de l'extraction des claims du token JWT");

                // Claims minimaux en cas d'erreur
                claims.Add(new Claim("JwtToken", jwtToken));
                claims.Add(new Claim("LoginTime", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")));
                claims.Add(new Claim("ErrorOccurred", "true"));
            }

            return claims;
        }
    }
}
