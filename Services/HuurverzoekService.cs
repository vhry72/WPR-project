using WPR_project.DTO_s;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.Services.Email;
﻿using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;

namespace WPR_project.Services;

public class HuurverzoekService
{ 


    private readonly IHuurVerzoekRepository _repository;
    private readonly IBedrijfsMedewerkersRepository _zakelijkRepository;
    private readonly IHuurderRegistratieRepository _particulierRepository;
    private readonly IEmailService _emailService;

    // Voeg de standaard ophaallocatie toe
    private const string OphaalLocatie = "Johanna Westerdijkplein 75, 2521 EP Den Haag";

    public HuurverzoekService(
        IHuurVerzoekRepository repository,
        IBedrijfsMedewerkersRepository zakelijkRepository,
        IHuurderRegistratieRepository particulierRepository,
        IEmailService emailService)
    {
        _repository = repository;
        _zakelijkRepository = zakelijkRepository;
        _particulierRepository = particulierRepository;
        _emailService = emailService;
    }

    public bool HasActiveHuurverzoek(Guid huurderId)
    {
        return _repository.GetActiveHuurverzoekenByHuurderId(huurderId).Any();
    }

    public string GetEmailByHuurderId(Guid huurderId)
    {
        var particulier = _particulierRepository.GetById(huurderId);
        if (particulier != null)
        {
            return particulier.particulierEmail;
        }

        var zakelijk = _zakelijkRepository.GetMedewerkerById(huurderId);
        if (zakelijk != null)
        {
            return zakelijk.medewerkerEmail;
        }

        throw new Exception("Geen gebruiker gevonden met het opgegeven ID.");
    }

    private string GenerateEmailBody(DateTime beginDate, string emailType)
    {
        var typeTekst = emailType == "herinnering"
            ? "Dit is een herinnering dat uw huurperiode morgen begint op:"
            : "Uw huurverzoek is succesvol geregistreerd. Startdatum:";

        return $@"Beste gebruiker,<br/><br/>
                  {typeTekst} {beginDate}<br/>
                  Ophaallocatie: {OphaalLocatie}<br/><br/>
                  <strong>Veiligheidsinstructies:</strong><br/>
                  - Zorg ervoor dat u het voertuig controleert op schade vóór gebruik.<br/>
                  - Rijd altijd met een geldig rijbewijs en houd u aan de verkeersregels.<br/>
                  - In geval van problemen, neem contact op met onze klantenservice.<br/><br/>
                  <strong>Praktische informatie:</strong><br/>
                  - De sleutel kan worden opgehaald bij de receptie op de bovengenoemde locatie.<br/>
                  - Breng het voertuig op tijd terug om extra kosten te voorkomen.<br/><br/>
                  Met vriendelijke groet,<br/>Het Team";
    }

    public void Add(Huurverzoek huurVerzoek)
    {
        if (huurVerzoek.beginDate >= huurVerzoek.endDate)
        {
            throw new ArgumentException("De begindatum kan niet na de einddatum liggen.");
        }

        var actieveHuurverzoeken = _repository.GetActiveHuurverzoekenByHuurderId(huurVerzoek.HuurderID);
        if (actieveHuurverzoeken.Any())
        {
            throw new ArgumentException("De huurder heeft al een actief huurverzoek.");
        }

        _repository.Add(huurVerzoek);
        

        var email = GetEmailByHuurderId(huurVerzoek.HuurderID);
        var subject = "Bevestiging van uw huurverzoek";
        var body = GenerateEmailBody(huurVerzoek.beginDate, "bevestiging");

        _emailService.SendEmail(email, subject, body);
    }

    public void SendReminders()
    {
        var reminderTime = DateTime.Now.AddHours(24);
        var huurverzoeken = _repository.GetHuurverzoekenForReminder(reminderTime);

        foreach (var verzoek in huurverzoeken)
        {
            try
            {
                var email = GetEmailByHuurderId(verzoek.HuurderID);
                var subject = "Herinnering: Uw huurperiode begint binnenkort";
                var body = GenerateEmailBody(verzoek.beginDate, "herinnering");

                _emailService.SendEmail(email, subject, body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout bij het versturen van een herinneringsmail: {ex.Message}");
            }
        }
    }

        public IEnumerable<Huurverzoek> GetAllHuurVerzoeken()
        {
            return _repository.GetAllHuurVerzoeken();
        }
        public IEnumerable<Huurverzoek> GetAllActiveHuurVerzoeken()
        {
            return _repository.GetAllActiveHuurVerzoeken();
        }
    public HuurVerzoekDTO GetById(Guid id)
        {
            var huurder = _repository.GetByID(id);
            if (huurder == null) { return null; }

            return new HuurVerzoekDTO
            {
                HuurderID = id,
                beginDate = huurder.beginDate,
                endDate = huurder.endDate,
                approved = huurder.approved
            };
        }
        public void Update(Guid id, HuurVerzoekDTO dto)
        {
            var huurder = _repository.GetByID(id);
            if (huurder == null) throw new KeyNotFoundException("Huurverzoek niet gevonden.");

            huurder.approved = dto.approved;

            _repository.Update(huurder);
            _repository.Save();

        }
    }


