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
using Microsoft.EntityFrameworkCore;
using WPR_project.Data;
using NuGet.Common;


[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManagerService _userManagerService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly GegevensContext _dbContext;
    private readonly IEmailService _emailService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(
        UserManagerService userManagerService,
        UserManager<ApplicationUser> userManager,
        IEmailService emailService,
        GegevensContext dbcontext,
        ILogger<AccountController> logger)
    {
        _userManagerService = userManagerService;
        _userManager = userManager;
        _dbContext = dbcontext;
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

    [HttpPost("register-backoffice")]
    public async Task<IActionResult> RegisterBackoffice([FromBody] BackofficeMedewerker dto)
    {
        if (dto == null)
        {
            return BadRequest("Model mag niet null zijn.");
        }

        try
        {
            var medewerker = await _userManagerService.RegisterBackofficeMedewerker(dto);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account",
                new { userId = medewerker.BackofficeMedewerkerId, token = medewerker.EmailBevestigingToken }, Request.Scheme);

            _emailService.SendEmail(dto.medewerkerEmail, "Bevestig je e-mailadres",
                $"Klik hier om je e-mailadres te bevestigen: <a href='{confirmationLink}'>Bevestig e-mailadres</a>");

            var identityUser = await _userManager.FindByIdAsync(medewerker.AspNetUserId);

            if (identityUser == null)
                return BadRequest("Identity gebruiker niet gevonden.");

            // QR Code voor 2FA genereren
            var qrCodeUri = await _userManagerService.EnableTwoFactorAuthenticationAsync(identityUser);
            byte[] qrCodeImage = _userManagerService.GenerateQrCode(qrCodeUri);

            // Verstuur QR code als e-mailbijlage
            await _emailService.SendEmailWithImage(dto.medewerkerEmail, "2FA QR-code",
                "Hierbij je QR-code voor 2FA. Open de bijlage om de QR-code te scannen en in te stellen.", qrCodeImage);

            return Ok(new { Message = "Backoffice medewerker succesvol geregistreerd." });
        }
        catch (Exception ex)
        {
            return BadRequest($"Er is een fout opgetreden: {ex.Message}");
        }
    }

    [HttpPost("register-frontoffice")]
    public async Task<IActionResult> RegisterFrontoffice([FromBody] FrontofficeMedewerker dto)
    {
        if (dto == null)
        {
            return BadRequest("Model mag niet null zijn.");
        }

        try
        {
            var medewerker = await _userManagerService.RegisterFrontofficeMedewerker(dto);

            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account",
                new { userId = medewerker.FrontofficeMedewerkerId, token = medewerker.EmailBevestigingToken }, Request.Scheme);

            // Verstuur de bevestigingsmail
            _emailService.SendEmail(dto.medewerkerEmail, "Bevestig je e-mailadres",
                $"Klik hier om je e-mailadres te bevestigen: <a href='{confirmationLink}'>Bevestig e-mailadres</a>");


            var identityUser = await _userManager.FindByIdAsync(medewerker.AspNetUserId);

            if (identityUser == null)
                return BadRequest("Identity gebruiker niet gevonden.");

            // QR Code voor 2FA genereren
            var qrCodeUri = await _userManagerService.EnableTwoFactorAuthenticationAsync(identityUser);
            byte[] qrCodeImage = _userManagerService.GenerateQrCode(qrCodeUri);

            // Verstuur QR code als e-mailbijlage
            await _emailService.SendEmailWithImage(dto.medewerkerEmail, "2FA QR-code",
                "Hierbij je QR-code voor 2FA. Open de bijlage om de QR-code te scannen en in te stellen.", qrCodeImage);

            return Ok(new { Message = "Frontoffice medewerker succesvol geregistreerd." });
        }
        catch (Exception ex)
        {
            return BadRequest($"Er is een fout opgetreden: {ex.Message}");
        }
    }

    [HttpPost("register-bedrijfsmedewerker")]
    public async Task<IActionResult> RegisterBedrijfsMedewerker([FromBody] BedrijfsmedewerkerRegDTO dto)
    {
        if (dto == null)
        {
            return BadRequest("Model mag niet null zijn.");
        }

        try
        {
            var medewerker = await _userManagerService.RegisterBedrijfsMedewerker(dto);

            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account",
                new { userId = medewerker.bedrijfsMedewerkerId, token = medewerker.EmailBevestigingToken }, Request.Scheme);

            // Verstuur de bevestigingsmail
            _emailService.SendEmail(dto.medewerkerEmail, "Bevestig je e-mailadres",
                $"Klik hier om je e-mailadres te bevestigen: <a href='{confirmationLink}'>Bevestig e-mailadres</a>");


            var identityUser = await _userManager.FindByIdAsync(medewerker.AspNetUserId);

            if (identityUser == null)
                return BadRequest("Identity gebruiker niet gevonden.");

            // QR Code voor 2FA genereren
            var qrCodeUri = await _userManagerService.EnableTwoFactorAuthenticationAsync(identityUser);
            byte[] qrCodeImage = _userManagerService.GenerateQrCode(qrCodeUri);

            // Verstuur QR code als e-mailbijlage
            await _emailService.SendEmailWithImage(dto.medewerkerEmail, "2FA QR-code",
                "Hierbij je QR-code voor 2FA. Open de bijlage om de QR-code te scannen en in te stellen.", qrCodeImage);

            return Ok(new { Message = "Bedrijfsmedewerker succesvol geregistreerd." });
        }
        catch (Exception ex)
        {
            return BadRequest($"Er is een fout opgetreden: {ex.Message}");
        }
    }


    [HttpPost("register-wagenparkbeheerder")]
    public async Task<IActionResult> RegisterWagenparkbeheerder([FromBody] WagenparkBeheerderDTO dto)
    {
        if (dto == null)
        {
            return BadRequest("Model mag niet null zijn.");
        }

        try
        {
            var beheerder = await _userManagerService.RegisterWagenParkBeheerder(dto);

            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account",
                new { userId = beheerder.beheerderId, token = beheerder.EmailBevestigingToken }, Request.Scheme);

            // Verstuur de bevestigingsmail
            _emailService.SendEmail(dto.bedrijfsEmail, "Bevestig je e-mailadres",
                $"Klik hier om je e-mailadres te bevestigen: <a href='{confirmationLink}'>Bevestig e-mailadres</a>");

            var identityUser = await _userManager.FindByIdAsync(beheerder.AspNetUserId);

            if (identityUser == null)
                return BadRequest("Identity gebruiker niet gevonden.");

            // QR Code voor 2FA genereren
            var qrCodeUri = await _userManagerService.EnableTwoFactorAuthenticationAsync(identityUser);
            byte[] qrCodeImage = _userManagerService.GenerateQrCode(qrCodeUri);

            // Verstuur QR code als e-mailbijlage
            await _emailService.SendEmailWithImage(dto.bedrijfsEmail, "2FA QR-code",
                "Hierbij je QR-code voor 2FA. Open de bijlage om de QR-code te scannen en in te stellen.", qrCodeImage);

            return Ok(new { Message = "Wagenparkbeheerder succesvol geregistreerd." });
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

        if (user.IsActive == false)
        {
            return Unauthorized("Account is Gedeactiveerd");
        }

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

        // Controleer of de gebruiker een van de bekende rollen heeft
        var particulierHuurder = await _dbContext.ParticulierHuurders.FirstOrDefaultAsync(h => h.AspNetUserId == user.Id);
        var zakelijkHuurder = await _dbContext.ZakelijkHuurders.FirstOrDefaultAsync(h => h.AspNetUserId == user.Id);
        var backofficeMedewerker = await _dbContext.BackofficeMedewerkers.FirstOrDefaultAsync(h => h.AspNetUserId == user.Id);
        var frontofficeMedewerker = await _dbContext.FrontofficeMedewerkers.FirstOrDefaultAsync(h => h.AspNetUserId == user.Id);
        var bedrijfsMedewerker = await _dbContext.BedrijfsMedewerkers.FirstOrDefaultAsync(h => h.AspNetUserId == user.Id);
        var wagenparkBeheerder = await _dbContext.WagenparkBeheerders.FirstOrDefaultAsync(h => h.AspNetUserId == user.Id);

        string huurderId;
        string role;

        if (particulierHuurder != null)
        {
            huurderId = particulierHuurder.particulierId.ToString();
            role = "ParticuliereHuurder";
        }
        else if (zakelijkHuurder != null)
        {
            huurderId = zakelijkHuurder.zakelijkeId.ToString();
            role = "ZakelijkeHuurder";
        }
        else if (backofficeMedewerker != null)
        {
            huurderId = backofficeMedewerker.BackofficeMedewerkerId.ToString();
            role = "BackofficeMedewerker";
        }
        else if (frontofficeMedewerker != null)
        {
            huurderId = frontofficeMedewerker.FrontofficeMedewerkerId.ToString();
            role = "FrontofficeMedewerker";
        }
        else if (bedrijfsMedewerker != null)
        {
            huurderId = bedrijfsMedewerker.bedrijfsMedewerkerId.ToString();
            role = "BedrijfsMedewerker";
        }
        else if (wagenparkBeheerder != null)
        {
            huurderId = wagenparkBeheerder.beheerderId.ToString();
            role = "WagenparkBeheerder";
        }
        else
        {
            return Unauthorized("Huurder niet gevonden.");
        }

        // Genereer een token met de huurder-ID en rol
        var token = _userManagerService.GenerateJwtToken(huurderId, role);

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

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] string email)
    {
        try
        {
            var (success, message) = await _userManagerService.ForgotPassword(email);
            if (success)
            {
                return Ok(message);
            }
            else
            {
                _logger.LogError($"Fout bij het resetten van wachtwoord: {message}");
                return BadRequest(message);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Onverwachte fout bij het resetten van wachtwoord: {ex.Message}");
            return StatusCode(500, "Er is een interne serverfout opgetreden.");
        }
    }

    [HttpPost("password-reset-beheerder/{beheerderEmail}")]
    public async Task<IActionResult> passwordResetLinkBeheerder([FromBody] string email, string beheerderEmail)
    {
        try
        {
            var (success, message) = await _userManagerService.ForgotPasswordBeheerder(email, beheerderEmail);
            if (success)
            {
                return Ok(message);
            }
            else
            {
                _logger.LogError($"Fout bij het resetten van wachtwoord: {message}");
                return BadRequest(message);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Onverwachte fout bij het resetten van wachtwoord: {ex.Message}");
            return StatusCode(500, "Er is een interne serverfout opgetreden.");
        }
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetDTO dto)
    {

        if (string.IsNullOrWhiteSpace(dto.UserId) || string.IsNullOrWhiteSpace(dto.Token) || string.IsNullOrWhiteSpace(dto.Password))
        {
            return BadRequest("Invalid input parameters");
        }

        var result = await _userManagerService.ResetPasswordAsync(dto.UserId, dto.Token, dto.Password);

        if (!result.Success)
        {
            return BadRequest(result.Errors);
        }

        return Ok("Wachtwoord succesvol gereset");
    }



    [HttpPost("reset-2fa")]
    public async Task<IActionResult> ResetTwoFactorAuthentication([FromBody] LoginDTO dto)
    {
        try
        {
            await _userManagerService.ResetTwoFactorAuthentication(dto.Email, dto.Password);
            return Ok("2FA is gereset en een nieuwe QR-code is verzonden.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Fout bij het resetten van 2FA: {ex.Message}");
            return BadRequest("Er is iets fout gegaan bij het resetten van 2FA.");
        }
    }

}
