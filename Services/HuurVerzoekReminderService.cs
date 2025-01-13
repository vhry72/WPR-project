using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using WPR_project.Services;

public class HuurverzoekReminderService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public HuurverzoekReminderService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
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
                    var wijzigAbonnement = scope.ServiceProvider.GetRequiredService<HuurverzoekService>();

                    // Voer de herinneringslogica uit
                    wijzigAbonnement.SendReminders();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout in de achtergrondtaak: {ex.Message}");
            }

            // Wacht 1 uur voordat de taak opnieuw wordt uitgevoerd
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}
