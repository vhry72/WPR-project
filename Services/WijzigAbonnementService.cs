using WPR_project.Repositories;
using WPR_project.Services.Email;
using WPR_project.Models;
using WPR_project.Data;
using WPR_project.Services;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace WPR_project.Services
{
    public class WijzigAbonnementService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IAbonnementRepository _abonnementRepository;
        private readonly IZakelijkeHuurderRepository _zakelijkeHuurderRepository;
        private readonly IWagenparkBeheerderRepository _wagenparkBeheerderRepository;

        public WijzigAbonnementService(
            IAbonnementRepository abonnementRepository,
            IZakelijkeHuurderRepository zakelijkeHuurderRepository,
            IWagenparkBeheerderRepository wagenparkBeheerderRepository,
            IEmailService emailService,
            AbonnementService abonnementService)
        {
            _abonnementRepository = abonnementRepository;
            _zakelijkeHuurderRepository = zakelijkeHuurderRepository;
            _wagenparkBeheerderRepository = wagenparkBeheerderRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        // Haal de scoped service op binnen een nieuw scope
                        var huurVerzoekService = scope.ServiceProvider.GetRequiredService<HuurverzoekService>();

                        // Voer de herinneringslogica uit
                        huurVerzoekService.SendReminders();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Fout in de achtergrondtaak: {ex.Message}");
                }

                // Wacht 1 dag voordat de taak opnieuw wordt uitgevoerd
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
            }
        }
        public void WijzigAbonnement(Guid beheerderId, Guid abonnementId, AbonnementType abonnementType)
        {
            var beheerder = _wagenparkBeheerderRepository.GetBeheerderById(beheerderId);
            if (beheerder == null)
                throw new KeyNotFoundException("Wagenparkbeheerder niet gevonden");

            var huidigAbonnement = beheerder.HuidigAbonnement;
            if (huidigAbonnement != null && DateTime.Now < huidigAbonnement.vervaldatum)
                throw new InvalidOperationException("Het huidige abonnement kan pas na de vervaldatum worden gewijzigd.");

            var nieuwAbonnement = _abonnementRepository.GetAbonnementById(abonnementId);
            if (nieuwAbonnement == null)
                throw new KeyNotFoundException("Abonnement niet gevonden.");

            beheerder.updateDatumAbonnement = huidigAbonnement.vervaldatum;
            _wagenparkBeheerderRepository.UpdateWagenparkBeheerder(beheerder);
            _wagenparkBeheerderRepository.Save();
        }

        public void WijzigAbonnementMetDirecteKosten(Guid beheerderId, Guid abonnementId, AbonnementType abonnementType)
        {
            var beheerder = _wagenparkBeheerderRepository.GetBeheerderById(beheerderId);
            if (beheerder == null) throw new KeyNotFoundException("Beheerder niet gevonden.");

            var huidigAbonnement = beheerder.HuidigAbonnement;
            if (huidigAbonnement != null && huidigAbonnement.updateDatum < huidigAbonnement.vervaldatum)
                throw new InvalidOperationException("Het huidige abonnement kan pas na de vervaldatum worden ingevoerd.");

            var abonnement = _abonnementRepository.GetAbonnementById(abonnementId);
            if (abonnement == null) throw new KeyNotFoundException("Abonnement niet gevonden.");

            beheerder.updateDatumAbonnement = huidigAbonnement.vervaldatum;
            _wagenparkBeheerderRepository.UpdateWagenparkBeheerder(beheerder);
            _wagenparkBeheerderRepository.Save();
        }

        public void WijzigAbonnementVanafVolgendePeriode(Guid beheerderId, Guid abonnementId, AbonnementType abonnementType)
        {
            var beheerder = _wagenparkBeheerderRepository.GetBeheerderById(beheerderId);
            if (beheerder == null) throw new KeyNotFoundException("Beheerder niet gevonden.");

            var huidigAbonnement = beheerder.HuidigAbonnement;
            if (huidigAbonnement != null && huidigAbonnement.updateDatum < huidigAbonnement.vervaldatum)
                throw new InvalidOperationException("Het huidige abonnement kan pas na de vervaldatum worden gewijzigd.");

            var abonnement = _abonnementRepository.GetAbonnementById(abonnementId);
            if (abonnement == null) throw new KeyNotFoundException("Abonnement niet gevonden.");

            beheerder.updateDatumAbonnement = huidigAbonnement.vervaldatum;
            _wagenparkBeheerderRepository.UpdateWagenparkBeheerder(beheerder);
            _wagenparkBeheerderRepository.Save();
        }
    }
}
