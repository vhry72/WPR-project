using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WPR_project.DTO_s;
using WPR_project.Models;
using WPR_project.Services.Email;
using Microsoft.Extensions.Logging;
using QRCoder;
using System.Security.Claims;


[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManagerService _userManagerService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IEmailService _emailService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        UserManagerService userManagerService,
        UserManager<IdentityUser> userManager,
        IEmailService emailService,
        ILogger<AccountController> logger)
    {
        _userManagerService = userManagerService;
        _userManager = userManager;
        _emailService = emailService;
        _logger = logger;
    }

    [HttpPost("register-particulier")]
    public async Task<IActionResult> RegisterParticulier([FromBody] ParticulierHuurderRegisterDTO dto)
    {
        if (dto == null)
        {
            return BadRequest("Model mag niet null zijn.");
        }

        try
        {
            // Roep de service aan en registreer de gebruiker
            var huurder = await _userManagerService.RegisterParticulierHuurder(dto);

            // Bouw de bevestigingslink met particulierId en EmailBevestigingToken
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account",
                new { userId = huurder.particulierId, token = huurder.EmailBevestigingToken }, Request.Scheme);

            // Verstuur de bevestigingsmail
            _emailService.SendEmail(dto.particulierEmail, "Bevestig je e-mailadres",
                $"Klik hier om je e-mail te bevestigen: <a href='{confirmationLink}'>Bevestig e-mailadres</a>");

            // Haal de IdentityUser op met AspNetUserId
            var identityUser = await _userManager.FindByIdAsync(huurder.AspNetUserId);
            if (identityUser == null)
            {
                return BadRequest("De gekoppelde IdentityUser is niet gevonden.");
            }

            // Activeer 2FA en genereer QR-code URI
            var qrCodeUri = await _userManagerService.EnableTwoFactorAuthenticationAsync(identityUser);

            return Ok(new
            {
                Message = "Particulier huurder succesvol geregistreerd. Controleer je e-mail voor bevestiging.",
                QrCodeUri = qrCodeUri
            });
        }
        catch (Exception ex)
        {
            return BadRequest($"Er is een fout opgetreden: {ex.Message}");
        }
    }



    [HttpPost("register-zakelijk")]
    public async Task<IActionResult> RegisterZakelijk([FromBody] ZakelijkeHuurderDTO dto)
    {
        if (dto == null)
        {
            return BadRequest("Model mag niet null zijn.");
        }

        try
        {
            // Roep de service aan en registreer de zakelijke huurder
            var huurder = await _userManagerService.RegisterZakelijkeHuurder(dto);

            // Bouw de bevestigingslink met zakelijkeId en EmailBevestigingToken
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account",
                new { userId = huurder.zakelijkeId, token = huurder.EmailBevestigingToken }, Request.Scheme);

            // Verstuur de bevestigingsmail
            _emailService.SendEmail(dto.bedrijfsEmail, "Bevestig je e-mailadres",
                $"Klik hier om je e-mailadres te bevestigen: <a href='{confirmationLink}'>Bevestig e-mailadres</a>");

            // Haal de IdentityUser op met AspNetUserId
            var identityUser = await _userManager.FindByIdAsync(huurder.AspNetUserId);
            if (identityUser == null)
            {
                return BadRequest("De gekoppelde IdentityUser is niet gevonden.");
            }

            // Activeer 2FA en genereer QR-code URI
            var qrCodeUri = await _userManagerService.EnableTwoFactorAuthenticationAsync(identityUser);

            return Ok(new
            {
                Message = "Zakelijke huurder succesvol geregistreerd. Controleer je e-mail voor bevestiging.",
                QrCodeUri = qrCodeUri
            });
        }
        catch (Exception ex)
        {
            return BadRequest($"Er is een fout opgetreden: {ex.Message}");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO model)
    {
        if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
        {
            return BadRequest("E-mail en wachtwoord zijn verplicht.");
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return Unauthorized("Gebruiker bestaat niet.");
        }

        // Controleer of de e-mail is bevestigd via UserManagerService
        var isEmailConfirmed = await _userManagerService.IsEmailConfirmedAsync(model.Email);
        if (!isEmailConfirmed)
        {
            return Unauthorized("E-mail is niet bevestigd. Controleer je inbox voor de bevestigingsmail.");
        }

        var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
        if (!passwordValid)
        {
            return Unauthorized("Onjuist wachtwoord.");
        }

        if (!await _userManager.GetTwoFactorEnabledAsync(user))
        {
            return Unauthorized("Twee-factor authenticatie is vereist.");
        }

        // Stuur alleen een response terug om aan te geven dat 2FA nodig is
        return Ok(new
        {
            RequiresTwoFactor = true,
            UserId = user.Id
        });
    }

    [HttpGet("user-info")]
    [Authorize] // Zorg ervoor dat alleen geauthenticeerde gebruikers toegang hebben
    public IActionResult GetUserInfo()
    {
        try
        {
            // Haal de gebruikersinformatie uit de claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(role))
            {
                return Unauthorized("Gebruikersinformatie niet gevonden in token.");
            }

            return Ok(new { UserId = userId, Role = role });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij het ophalen van gebruikersinformatie.");
            return StatusCode(500, "Er is een fout opgetreden.");
        }
    }





    [HttpPost("verify-2fa")]
    public async Task<IActionResult> VerifyTwoFactor([FromBody] Verify2FADTO model)
    {
        if (string.IsNullOrWhiteSpace(model.UserId) || string.IsNullOrWhiteSpace(model.Code))
        {
            return BadRequest("Gebruikers-ID en verificatiecode zijn verplicht.");
        }

        var user = await _userManager.FindByIdAsync(model.UserId);
        if (user == null)
        {
            return Unauthorized("Gebruiker bestaat niet.");
        }

        var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, model.Code);
        if (!isValid)
        {
            return Unauthorized("Ongeldige verificatiecode.");
        }

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault();

        // Genereer een token met alleen id en role
        var token = _userManagerService.GenerateJwtToken(user.Id, role);

        // Sla de token op in een HttpOnly cookie
        HttpContext.Response.Cookies.Append("jwt", token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true, // Alleen via HTTPS
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddHours(1) // Token geldig voor 1 uur
        });

        return Ok(new { Role = role });
    }



    [HttpPost("logout")]
    public IActionResult Logout()
    {
        try
        {
            HttpContext.Response.Cookies.Delete("jwt");
            _logger.LogInformation("JWT-cookie succesvol verwijderd.");
            return Ok(new { message = "Uitgelogd" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij het verwijderen van de JWT-cookie.");
            return StatusCode(500, "Er is een fout opgetreden bij het uitloggen.");
        }
    }




    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token))
        {
            return BadRequest("Gebruikers-ID en token zijn verplicht.");
        }

        try
        {
            var result = await _userManagerService.ConfirmEmailAsync(userId, token);
            if (!result)
            {
                _logger.LogWarning("E-mailbevestiging mislukt voor gebruiker {UserId}. Mogelijk is de token verlopen of onjuist.", userId);
                return Redirect("https://localhost:5173/email-confirmation?success=false&reason=invalid_token");
            }

            return Redirect($"https://localhost:5173/email-confirmation?success=true");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fout bij e-mailbevestiging voor gebruiker {UserId}.", userId);
            return Redirect("https://localhost:5173/email-confirmation?success=false&reason=exception");
        }
    }


    [HttpPost("enable-2fa")]
    [Authorize]
    public async Task<IActionResult> EnableTwoFactorAuthentication()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return Unauthorized();
        }

        var authenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(authenticatorKey))
        {
            await _userManager.ResetAuthenticatorKeyAsync(user);
            authenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user);
        }

        var qrCodeUri = $"otpauth://totp/{Uri.EscapeDataString("WPR Project")}:{Uri.EscapeDataString(user.Email)}?secret={authenticatorKey}&issuer={Uri.EscapeDataString("WPR Project")}&digits=6";

        return Ok(new
        {
            QrCodeUri = qrCodeUri
        });
    }
}
