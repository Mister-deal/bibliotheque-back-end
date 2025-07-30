using bibliotheque_back_end.Models.DTO;
using bibliotheque_back_end.Models.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace bibliotheque_back_end.Controllers;

[ApiController]
    [Route("api/[controller]")]
    [SwaggerTag("Contrôleur d'authentification et de gestion des tokens JWT")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [SwaggerSchema("Modèle de réponse pour l'authentification")]
        public class AuthResponse
        {
            [SwaggerSchema("Token JWT en cas de succès.")]
            public string? Token { get; set; }

            [SwaggerSchema("Message décrivant le résultat de l'opération.")]
            public string Message { get; set; } = string.Empty;

            [SwaggerSchema("Indique si l'opération a réussi.")]
            public bool IsSuccess { get; set; }
        }

        [HttpPost("login")]
        [SwaggerOperation(
            Summary = "Authentifie un utilisateur et retourne un token JWT",
            Description = "Utilise l'email et le mot de passe pour obtenir un token JWT si les identifiants sont valides. Le token doit être utilisé dans l'en-tête 'Authorization: Bearer [token]' pour accéder aux endpoints protégés."
        )]
        [SwaggerResponse(200, "Authentification réussie, token JWT retourné.", typeof(AuthResponse))]
        [SwaggerResponse(401, "Identifiants invalides.", typeof(AuthResponse))]
        [SwaggerResponse(400, "Requête invalide (par exemple, champs manquants ou mal formés).", typeof(AuthResponse))]
        [SwaggerResponse(500, "Erreur interne du serveur.", typeof(AuthResponse))]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                // Retourne les erreurs de validation du modèle
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new AuthResponse { IsSuccess = false, Message = string.Join(" | ", errors) });
            }

            try
            {
                var token = await _authService.AuthenticateUser(request.Email, request.MotDePasse);

                if (token == null)
                {
                    return Unauthorized(new AuthResponse { IsSuccess = false, Message = "Email ou mot de passe invalide." });
                }

                return Ok(new AuthResponse { Token = token, Message = "Authentification réussie.", IsSuccess = true });
            }
            catch (Exception ex)
            {
                // Log l'exception (utilisez un logger réel en production)
                Console.WriteLine($"Erreur lors de la tentative de connexion: {ex.Message}");
                return StatusCode(500, new AuthResponse { IsSuccess = false, Message = "Une erreur interne du serveur est survenue lors de la connexion." });
            }
        }
    }