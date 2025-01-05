using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WPR_project.Models;
using WPR_project.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WPR_project.Services.Email;
using Microsoft.EntityFrameworkCore;
using WPR_project.DTO_s;
using Azure.Core;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

public class UserManagerService
{
    private readonly IConfiguration _configuration;
    private readonly GegevensContext _dbContext;
    private readonly IEmailService _emailService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ILogger<UserManagerService> _logger;

    public UserManagerService(
     IConfiguration configuration,
     GegevensContext dbContext,
     IEmailService emailService,
     UserManager<IdentityUser> userManager,
     ILogger<UserManagerService> logger)
    {
        _configuration = configuration;
        _dbContext = dbContext;
        _emailService = emailService;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<ParticulierHuurder> RegisterParticulierHuurder(ParticulierHuurderRegisterDTO dto)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            // Maak een IdentityUser aan
            var user = new IdentityUser
            {
                UserName = dto.particulierEmail,
                Email = dto.particulierEmail,
                EmailConfirmed = false
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
                wachtwoord = hashedPassword, // Zorg ervoor dat dit gehasht is
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




    public async Task<ZakelijkHuurder> RegisterZakelijkeHuurder(ZakelijkeHuurderDTO dto)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            // Maak een IdentityUser aan en gebruik Identity API voor hashing
            var user = new IdentityUser
            {
                UserName = dto.bedrijfsEmail,
                Email = dto.bedrijfsEmail,
                EmailConfirmed = false
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
                wachtwoord = hashedPassword // Handmatig gehasht wachtwoord
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



    public string GenerateJwtToken(string userId, string role)
    {
        // Claims bevatten alleen UserId en Role
        var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, userId), // 'sub' claim voor de unieke gebruikers-ID
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

        // Als de e-mail nergens is gevonden
        _logger.LogWarning("E-mail niet gevonden in ParticulierHuurders of ZakelijkHuurders: {Email}", email);
        return false;
    }

    public async Task<string> EnableTwoFactorAuthenticationAsync(IdentityUser user)
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

        // Genereer QR-code URI
        var qrCodeUri = $"otpauth://totp/{Uri.EscapeDataString("MijnApplicatie")}:{Uri.EscapeDataString(user.Email)}?secret={authenticatorKey}&issuer={Uri.EscapeDataString("MijnApplicatie")}&digits=6";
        return qrCodeUri;
    }



    public async Task<bool> VerifyTwoFactorTokenAsync(IdentityUser user, string token)
    {
        if (user == null)
        {
            _logger.LogError("2FA-verificatie mislukt: Gebruiker is null.");
            throw new ArgumentNullException(nameof(user), "De gebruiker mag niet null zijn.");
        }

        if (string.IsNullOrWhiteSpace(token))
        {
            _logger.LogWarning("2FA-verificatie mislukt: Token is leeg of null.");
            return false;
        }

        try
        {
            var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, _userManager.Options.Tokens.AuthenticatorTokenProvider, token);
            if (!isValid)
            {
                _logger.LogWarning("2FA-verificatie mislukt: Ongeldig token voor gebruiker {UserId}.", user.Id);
            }
            else
            {
                _logger.LogInformation("2FA-verificatie geslaagd voor gebruiker {UserId}.", user.Id);
            }

            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Er is een fout opgetreden tijdens 2FA-verificatie voor gebruiker {UserId}.", user.Id);
            throw;
        }
    }
}

