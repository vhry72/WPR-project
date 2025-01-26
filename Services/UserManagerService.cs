using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WPR_project.Models;
using WPR_project.DTO_s;
using WPR_project.Data;
using Microsoft.AspNetCore.Identity;
using WPR_project.Services.Email;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using System.Text.RegularExpressions;
using System.Data;
using Microsoft.Identity.Client;

public class UserManagerService
{
    private readonly IConfiguration _configuration;
    private readonly GegevensContext _dbContext;
    private readonly IEmailService _emailService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<UserManagerService> _logger;
    private string huurderID;

    public UserManagerService(
     IConfiguration configuration,
     GegevensContext dbContext,
     IEmailService emailService,
     UserManager<ApplicationUser> userManager,
     ILogger<UserManagerService> logger)
    {
        _configuration = configuration;
        _dbContext = dbContext;
        _emailService = emailService;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<string> BepaalHuurderId(string aspnetUserId)
    {
        var particulierHuurder = await _dbContext.ParticulierHuurders.FirstOrDefaultAsync(h => h.AspNetUserId == aspnetUserId);
        var zakelijkHuurder = await _dbContext.ZakelijkHuurders.FirstOrDefaultAsync(h => h.AspNetUserId == aspnetUserId);
        var backofficeMedewerker = await _dbContext.BackofficeMedewerkers.FirstOrDefaultAsync(h => h.AspNetUserId == aspnetUserId);
        var frontofficeMedewerker = await _dbContext.FrontofficeMedewerkers.FirstOrDefaultAsync(h => h.AspNetUserId == aspnetUserId);
        var bedrijfsMedewerker = await _dbContext.BedrijfsMedewerkers.FirstOrDefaultAsync(h => h.AspNetUserId == aspnetUserId);
        var wagenparkBeheerder = await _dbContext.WagenparkBeheerders.FirstOrDefaultAsync(h => h.AspNetUserId == aspnetUserId);

        if (particulierHuurder != null)
        {
            huurderID = particulierHuurder.particulierId.ToString();
        }
        else if (zakelijkHuurder != null)
        {
            huurderID = zakelijkHuurder.zakelijkeId.ToString();
        }
        else if (backofficeMedewerker != null)
        {
            huurderID = backofficeMedewerker.BackofficeMedewerkerId.ToString();
        }
        else if (frontofficeMedewerker != null)
        {
            huurderID = frontofficeMedewerker.FrontofficeMedewerkerId.ToString();
        }
        else if (bedrijfsMedewerker != null)
        {
            huurderID = bedrijfsMedewerker.bedrijfsMedewerkerId.ToString();
        }
        else if (wagenparkBeheerder != null)
        {
            huurderID = wagenparkBeheerder.beheerderId.ToString();
        }

        return huurderID;
    }

    public async Task<ParticulierHuurder> RegisterParticulierHuurder(ParticulierHuurderRegisterDTO dto)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            // Maak een IdentityUser aan
            var user = new ApplicationUser
            {
                UserName = dto.particulierEmail,
                Email = dto.particulierEmail,
                EmailConfirmed = false,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, dto.wachtwoord);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Fout bij het aanmaken van de gebruiker: {errors}");
            }

            var passwordHasher = new PasswordHasher<object>();
            string hashedPassword = passwordHasher.HashPassword(null, dto.wachtwoord);

            // Voeg de gebruiker toe aan de rol 'Particuliere Huurder'
            var roleResult = await _userManager.AddToRoleAsync(user, "ParticuliereHuurder");
            if (!roleResult.Succeeded)
            {
                throw new Exception("Fout bij het toewijzen van de rol 'Particuliere Huurder' aan de gebruiker.");
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);

            // Maak een ParticulierHuurder-object
            var huurder = new ParticulierHuurder
            {
                particulierId = Guid.NewGuid(),
                particulierEmail = dto.particulierEmail,
                particulierNaam = dto.particulierNaam,
                wachtwoord = dto.wachtwoord, // Zorg ervoor dat dit gehasht is
                adress = dto.adress,
                postcode = dto.postcode,
                woonplaats = dto.woonplaats,
                telefoonnummer = dto.telefoonnummer,
                AspNetUserId = user.Id, // Koppel IdentityUser ID
                IsEmailBevestigd = false,
                EmailBevestigingToken = Guid.NewGuid()
            };

            // Sla ParticulierHuurder op in de database
            _dbContext.ParticulierHuurders.Add(huurder);
            await _dbContext.SaveChangesAsync();

            // Commit de transactie
            await transaction.CommitAsync();

            return huurder;
        }
        catch (Exception ex)
        {
            // Rollback de transactie bij een fout
            await transaction.RollbackAsync();

            // Verwijder de gebruiker uit AspNetUsers als er een fout optreedt
            var user = await _userManager.FindByEmailAsync(dto.particulierEmail);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            throw new Exception($"Er is een fout opgetreden tijdens de registratie: {ex.Message}", ex);
        }
    }

    private async Task<IdentityUser> RegisterUser(string email, string password, string role)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = false,
            IsActive = true
        };

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new Exception($"Fout bij het aanmaken van de gebruiker: {errors}");
        }

        var roleResult = await _userManager.AddToRoleAsync(user, role);
        if (!roleResult.Succeeded)
        {
            throw new Exception($"Fout bij het toewijzen van de rol '{role}' aan de gebruiker.");
        }

        return user;
    }

    public async Task<BackofficeMedewerker> RegisterBackofficeMedewerker(BackofficeMedewerker dto)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            var passwordHasher = new PasswordHasher<object>();
            string hashedPassword = passwordHasher.HashPassword(null, dto.wachtwoord);

            var user = await RegisterUser(dto.medewerkerEmail, dto.wachtwoord, "Backofficemedewerker");

            var medewerker = new BackofficeMedewerker
            {
                BackofficeMedewerkerId = Guid.NewGuid(),
                medewerkerEmail = dto.medewerkerEmail,
                medewerkerNaam = dto.medewerkerNaam,
                wachtwoord = dto.wachtwoord,
                AspNetUserId = user.Id,
                IsEmailBevestigd = false,
                EmailBevestigingToken = Guid.NewGuid()
            };

            _dbContext.BackofficeMedewerkers.Add(medewerker);
            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
            return medewerker;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<FrontofficeMedewerker> RegisterFrontofficeMedewerker(FrontofficeMedewerker dto)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            var passwordHasher = new PasswordHasher<object>();
            string hashedPassword = passwordHasher.HashPassword(null, dto.wachtwoord);

            var user = await RegisterUser(dto.medewerkerEmail, dto.wachtwoord, "Frontofficemedewerker");

            var medewerker = new FrontofficeMedewerker
            {
                FrontofficeMedewerkerId = Guid.NewGuid(),
                medewerkerEmail = dto.medewerkerEmail,
                medewerkerNaam = dto.medewerkerNaam,
                wachtwoord = dto.wachtwoord,
                AspNetUserId = user.Id,
                IsEmailBevestigd = false,
                EmailBevestigingToken = Guid.NewGuid()
            };

            _dbContext.FrontofficeMedewerkers.Add(medewerker);
            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
            return medewerker;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<BedrijfsMedewerkers> RegisterBedrijfsMedewerker(BedrijfsmedewerkerRegDTO dto)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            var passwordHasher = new PasswordHasher<object>();
            string hashedPassword = passwordHasher.HashPassword(null, dto.wachtwoord);

            var user = await RegisterUser(dto.medewerkerEmail, dto.wachtwoord, "Bedrijfsmedewerker");

            var medewerker = new BedrijfsMedewerkers
            {
                bedrijfsMedewerkerId = Guid.NewGuid(),
                medewerkerEmail = dto.medewerkerEmail,
                medewerkerNaam = dto.medewerkerNaam,
                wachtwoord = dto.wachtwoord,
                zakelijkeId = dto.zakelijkeHuurderId,
                beheerderId = dto.WagenparkBeheerderId,
                AspNetUserId = user.Id,
                IsEmailBevestigd = false,
                AbonnementId = null,
                EmailBevestigingToken = Guid.NewGuid()
            };

            _dbContext.BedrijfsMedewerkers.Add(medewerker);
            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
            return medewerker;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<WagenparkBeheerder> RegisterWagenParkBeheerder(WagenparkBeheerderDTO dto)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            var passwordHasher = new PasswordHasher<object>();
            string hashedPassword = passwordHasher.HashPassword(null, dto.wachtwoord);

            var user = await RegisterUser(dto.bedrijfsEmail, dto.wachtwoord, "Wagenparkbeheerder");

            var beheerder = new WagenparkBeheerder
            {
                beheerderId = Guid.NewGuid(),
                bedrijfsEmail = dto.bedrijfsEmail,
                beheerderNaam = dto.beheerderNaam,
                Adres = dto.Adres,
                KVKNummer = dto.kvkNummer,
                AbonnementId = dto.AbonnementId,
                AbonnementType = dto.AbonnementType,
                telefoonNummer = dto.telefoonNummer,
                wachtwoord = dto.wachtwoord,
                zakelijkeId = dto.zakelijkeId,
                AspNetUserId = user.Id,
                IsEmailBevestigd = false,
                EmailBevestigingToken = Guid.NewGuid()
            };

            _dbContext.WagenparkBeheerders.Add(beheerder);
            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
            return beheerder;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }


    public async Task<ZakelijkHuurder> RegisterZakelijkeHuurder(ZakelijkeHuurderDTO dto)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            // Maak een IdentityUser aan en gebruik Identity API voor hashing
            var user = new ApplicationUser
            {
                UserName = dto.bedrijfsEmail,
                Email = dto.bedrijfsEmail,
                EmailConfirmed = false,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, dto.wachtwoord); // Identity API hasht dit automatisch
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Fout bij het aanmaken van de gebruiker: {errors}");
            }

            // Hash het wachtwoord handmatig voordat je het opslaat in ZakelijkeHuurders
            var passwordHasher = new PasswordHasher<object>();
            string hashedPassword = passwordHasher.HashPassword(null, dto.wachtwoord);

            // Voeg de gebruiker toe aan de rol 'Particuliere Huurder'
            var roleResult = await _userManager.AddToRoleAsync(user, "ZakelijkeHuurder");
            if (!roleResult.Succeeded)
            {
                throw new Exception("Fout bij het toewijzen van de rol 'Zakelijke Huurder' aan de gebruiker.");
            }

            await _userManager.SetTwoFactorEnabledAsync(user, true);

            // Maak een ZakelijkeHuurder-object
            var huurder = new ZakelijkHuurder
            {
                zakelijkeId = Guid.NewGuid(),
                bedrijfsEmail = dto.bedrijfsEmail,
                bedrijfsNaam = dto.bedrijfsNaam,
                telNummer = dto.telNummer,
                adres = dto.adres,
                KVKNummer = dto.KVKNummer,
                AspNetUserId = user.Id, // Koppel IdentityUser ID
                IsEmailBevestigd = false,
                EmailBevestigingToken = Guid.NewGuid(),
                wachtwoord = dto.wachtwoord // Handmatig gehasht wachtwoord
            };

            _dbContext.ZakelijkHuurders.Add(huurder);
            await _dbContext.SaveChangesAsync();

            await transaction.CommitAsync();
            return huurder;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();

            // Rollback: Verwijder de gebruiker uit AspNetUsers als het proces faalt
            var user = await _userManager.FindByEmailAsync(dto.bedrijfsEmail);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            throw new Exception($"Er is een fout opgetreden tijdens de registratie: {ex.Message}", ex);
        }
    }



    public string GenerateJwtToken(string huurderId, string role)
    {
        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, huurderId), // 'sub' claim voor de unieke huurder-ID
        new Claim(ClaimTypes.Role, role) // Claim voor de gebruikersrol
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }




    public async Task<bool> ConfirmEmailAsync(string userId, string token)
    {
        if (!Guid.TryParse(userId, out Guid parsedUserId) || !Guid.TryParse(token, out Guid parsedToken))
        {
            _logger.LogWarning("Ongeldige gebruiker of token. GebruikerId: {UserId}, Token: {Token}", userId, token);
            throw new ArgumentException("Ongeldige gebruiker of token. Beide moeten een geldig GUID-formaat hebben.");
        }

        // Zoek in ParticulierHuurders
        var particulierHuurder = await _dbContext.ParticulierHuurders
            .FirstOrDefaultAsync(h => h.particulierId == parsedUserId && h.EmailBevestigingToken == parsedToken);

        if (particulierHuurder != null)
        {
            particulierHuurder.IsEmailBevestigd = true;
            _dbContext.ParticulierHuurders.Update(particulierHuurder);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("E-mailbevestiging geslaagd voor particulier gebruiker {UserId}.", userId);
            return true;
        }

        // Zoek in ZakelijkHuurders
        var zakelijkHuurder = await _dbContext.ZakelijkHuurders
            .FirstOrDefaultAsync(h => h.zakelijkeId == parsedUserId && h.EmailBevestigingToken == parsedToken);

        if (zakelijkHuurder != null)
        {
            zakelijkHuurder.IsEmailBevestigd = true;
            _dbContext.ZakelijkHuurders.Update(zakelijkHuurder);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("E-mailbevestiging geslaagd voor zakelijk gebruiker {UserId}.", userId);
            return true;
        }

        // Zoek in BackofficeMedewerkers
        var backofficeMedewerker = await _dbContext.BackofficeMedewerkers
            .FirstOrDefaultAsync(h => h.BackofficeMedewerkerId == parsedUserId && h.EmailBevestigingToken == parsedToken);

        if (backofficeMedewerker != null)
        {
            backofficeMedewerker.IsEmailBevestigd = true;
            _dbContext.BackofficeMedewerkers.Update(backofficeMedewerker);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("E-mailbevestiging geslaagd voor backoffice medewerker {UserId}.", userId);
            return true;
        }

        // Zoek in FrontofficeMedewerkers
        var frontofficeMedewerker = await _dbContext.FrontofficeMedewerkers
            .FirstOrDefaultAsync(h => h.FrontofficeMedewerkerId == parsedUserId && h.EmailBevestigingToken == parsedToken);

        if (frontofficeMedewerker != null)
        {
            frontofficeMedewerker.IsEmailBevestigd = true;
            _dbContext.FrontofficeMedewerkers.Update(frontofficeMedewerker);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("E-mailbevestiging geslaagd voor frontoffice medewerker {UserId}.", userId);
            return true;
        }

        // Zoek in BedrijfsMedewerkers
        var bedrijfsMedewerker = await _dbContext.BedrijfsMedewerkers
            .FirstOrDefaultAsync(h => h.bedrijfsMedewerkerId == parsedUserId && h.EmailBevestigingToken == parsedToken);

        if (bedrijfsMedewerker != null)
        {
            bedrijfsMedewerker.IsEmailBevestigd = true;
            _dbContext.BedrijfsMedewerkers.Update(bedrijfsMedewerker);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("E-mailbevestiging geslaagd voor bedrijfsmedewerker {UserId}.", userId);
            return true;
        }

        // Zoek in WagenparkBeheerders
        var wagenparkBeheerder = await _dbContext.WagenparkBeheerders
            .FirstOrDefaultAsync(h => h.beheerderId == parsedUserId && h.EmailBevestigingToken == parsedToken);

        if (wagenparkBeheerder != null)
        {
            wagenparkBeheerder.IsEmailBevestigd = true;
            _dbContext.WagenparkBeheerders.Update(wagenparkBeheerder);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("E-mailbevestiging geslaagd voor wagenparkbeheerder {UserId}.", userId);
            return true;
        }

        _logger.LogWarning("E-mailbevestiging mislukt: geen match gevonden voor gebruiker {UserId} en token {Token}.", userId, token);
        throw new InvalidOperationException("Ongeldige gebruiker of token.");
    }

    public async Task<bool> IsEmailConfirmedAsync(string email)
    {
        // Controleer in ParticulierHuurders
        var particulierHuurder = await _dbContext.ParticulierHuurders
            .FirstOrDefaultAsync(h => h.particulierEmail == email);

        if (particulierHuurder != null)
        {
            return particulierHuurder.IsEmailBevestigd;
        }

        // Controleer in ZakelijkHuurders
        var zakelijkHuurder = await _dbContext.ZakelijkHuurders
            .FirstOrDefaultAsync(h => h.bedrijfsEmail == email);

        if (zakelijkHuurder != null)
        {
            return zakelijkHuurder.IsEmailBevestigd;
        }

        // Controleer in BackofficeMedewerkers
        var backofficeMedewerker = await _dbContext.BackofficeMedewerkers
            .FirstOrDefaultAsync(h => h.medewerkerEmail == email);

        if (backofficeMedewerker != null)
        {
            return backofficeMedewerker.IsEmailBevestigd;
        }

        // Controleer in FrontofficeMedewerkers
        var frontofficeMedewerker = await _dbContext.FrontofficeMedewerkers
            .FirstOrDefaultAsync(h => h.medewerkerEmail == email);

        if (frontofficeMedewerker != null)
        {
            return frontofficeMedewerker.IsEmailBevestigd;
        }

        // Controleer in BedrijfsMedewerkers
        var bedrijfsMedewerker = await _dbContext.BedrijfsMedewerkers
            .FirstOrDefaultAsync(h => h.medewerkerEmail == email);

        if (bedrijfsMedewerker != null)
        {
            return bedrijfsMedewerker.IsEmailBevestigd;
        }

        // Controleer in WagenparkBeheerders
        var wagenparkBeheerder = await _dbContext.WagenparkBeheerders
            .FirstOrDefaultAsync(h => h.bedrijfsEmail == email);

        if (wagenparkBeheerder != null)
        {
            return wagenparkBeheerder.IsEmailBevestigd;
        }

        // Als de e-mail nergens is gevonden
        _logger.LogWarning("E-mail niet gevonden in bekende tabellen: {Email}", email);
        return false;
    }


    public async Task<string> EnableTwoFactorAuthenticationAsync(ApplicationUser user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        // Genereer een authenticator key
        var authenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(authenticatorKey))
        {
            await _userManager.ResetAuthenticatorKeyAsync(user);
            authenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user);
        }

        await _userManager.SetTwoFactorEnabledAsync(user, true);

        // Genereer QR-code URI
        var qrCodeUri = $"otpauth://totp/{Uri.EscapeDataString("MijnApplicatie")}:{Uri.EscapeDataString(user.Email)}?secret={authenticatorKey}&issuer={Uri.EscapeDataString("MijnApplicatie")}&digits=6";
        return qrCodeUri;
    }



    public byte[] GenerateQrCode(string qrCodeUri)
    {
        using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
        using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrCodeUri, QRCodeGenerator.ECCLevel.Q))
        using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
        {
            return qrCode.GetGraphic(20);
        }
    }

    public async Task<(bool Success, string Message)> ForgotPassword(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return (false, $"Gebruiker niet gevonden of e-mail niet bevestigd voor: {email}");
        }

        var roles = await _userManager.GetRolesAsync(user);
        if (roles.Contains("Bedrijfsmedewerker"))
        {
            return (false, $"Gebruikers met de de rol BedrijfsMedewerker kunnen hun wachtwoord niet resetten. Neem contact op met je Wagenparkbeheerder");
        }

        if (roles.Contains("Frontofficemedewerker"))
        {
            return (false, $"Gebruikers met de rollen FrontofficeMedewerker kunnen hun wachtwoord niet resetten. Neem contact op met een Backoffice medewerker");
        }

        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            return (false, $"Ongeldige e-mailformat: {email}");
        }

        try
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = $"https://localhost:5173/wachtwoord-reset?userId={user.Id}&code={Uri.EscapeDataString(code)}";

            await _emailService.SendEmailAsync(email, "Reset Wachtwoord",
                $"Reset je wachtwoord door op de volgende link te klikken: <a href='{callbackUrl}'>link</a>");

            return (true, $"Een e-mail om het wachtwoord te resetten is verzonden naar: {email}. Volg de instructies in de e-mail om je wachtwoord te resetten.");
        }
        catch (Exception ex)
        {
            return (false, $"Er is iets fout gegaan bij het resetten van het wachtwoord voor {email}: {ex.Message}");
        }
    }


    public async Task<(bool Success, string Message)> ForgotPasswordBeheerder(string email, string beheerderEmail)
    {
        if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            return (false, $"Ongeldige e-mailformat: {email}");
        }

        var user = await _userManager.FindByEmailAsync(email);

        
        if (user == null)
        {
            return (false, $"Gebruiker niet gevonden of e-mail niet bevestigd voor: {email}");
        }

        try
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = $"https://localhost:5173/wachtwoord-reset?userId={user.Id}&code={Uri.EscapeDataString(code)}";

            await _emailService.SendEmailAsync(beheerderEmail, "Reset Wachtwoord",
                $"Reset de wachtwoord voor je medewerker door op de volgende link te klikken: <a href='{callbackUrl}'>link</a>");

            return (true, $"Een e-mail om het wachtwoord te resetten is verzonden naar: {beheerderEmail}. Volg de instructies in de e-mail om je wachtwoord te resetten.");
        }
        catch (Exception ex)
        {
            return (false, $"Er is iets fout gegaan bij het resetten van het wachtwoord voor {beheerderEmail}: {ex.Message}");
        }
    }

    public async Task<(bool Success, IEnumerable<string> Errors)> ResetPasswordAsync(string userId, string token, string password)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return (false, new List<string> { "Geen gebruiker gevonden." });
        }

        var result = await _userManager.ResetPasswordAsync(user, token, password);
        if (result.Succeeded)
        {
            return (true, Enumerable.Empty<string>());
        }

        return (false, result.Errors.Select(e => e.Description));
    }


    public async Task ResetTwoFactorAuthentication(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            _logger.LogError("Gebruiker niet gevonden.");
            return;
        }

        // Verifieer het wachtwoord
        var passwordCheck = await _userManager.CheckPasswordAsync(user, password);
        if (!passwordCheck)
        {
            _logger.LogError("Ongeldig wachtwoord.");
            return;
        }

        
        await _userManager.SetTwoFactorEnabledAsync(user, false);
        await _userManager.ResetAuthenticatorKeyAsync(user);
        await _userManager.SetTwoFactorEnabledAsync(user, true);


        var qrCodeUri = await EnableTwoFactorAuthenticationAsync(user);
        if (string.IsNullOrEmpty(qrCodeUri))
        {
            return;
        }

        byte[] qrCodeImage = GenerateQrCode(qrCodeUri);



        await _emailService.SendEmailWithImage(email, "2FA QR-code",
            "Hierbij je reset QR-code voor 2FA. Open de bijlage om de QR-code te scannen en in te stellen.", qrCodeImage);
    }
}

