using WPR_project.DTO_s;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.Services.Email;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using Hangfire;

namespace WPR_project.Services;

public class HuurverzoekService 
{


    private readonly IHuurVerzoekRepository _repository;
    private readonly IBedrijfsMedewerkersRepository _zakelijkRepository;
    private readonly IHuurderRegistratieRepository _particulierRepository;
    private readonly IEmailService _emailService;

    
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
    public bool HasAnsweredHuurverzoek(Guid huurderId)
    {
        return _repository.GetBeantwoordeHuurverzoekenByHuurderId(huurderId).Any();
    }

    public IEnumerable<Voertuig> GetAvailableVehicles(DateTime start, DateTime end)
    {
        return _repository.GetAvailableVehicles(start, end).ToList();
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
                  Met vriendelijke groet,<br/>Het CarAndAll Team";
    }

    public void Add(Huurverzoek huurVerzoek)
    {
        if (huurVerzoek.beginDate <= DateTime.Now)
        {
            throw new ArgumentException("De begindatum moet in de toekomst liggen.");
        }
        if (huurVerzoek.beginDate >= huurVerzoek.endDate)
        {
            throw new ArgumentException("De begindatum kan niet na de einddatum liggen.");
        }

        _repository.Add(huurVerzoek);

        var email = GetEmailByHuurderId(huurVerzoek.HuurderID);
        var subject = "Bevestiging van uw huurverzoek";
        var body = GenerateEmailBody(huurVerzoek.beginDate, "bevestiging");

        var delay = huurVerzoek.beginDate.AddDays(-1) - DateTime.Now;

       
        if (delay.TotalSeconds <= 0)
        {
            
            _emailService.SendEmail(email, subject, body);
        }
        else
        {
            BackgroundJob.Schedule(() => _emailService.SendEmail(email, subject, body), delay);
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

    public IEnumerable<Huurverzoek> GetAllBeantwoordeHuurVerzoeken()
    {
        return _repository.GetAllBeantwoorde();
    }
    public IEnumerable<Huurverzoek> GetAllAfgekeurde()
    {
        return _repository.GetAllAfgekeurde();
    }

    public IEnumerable<Huurverzoek> GetHuurverzoekByHuurderID(Guid id)
    {
        return _repository.GetHuurverzoekenByHuurderID(id);
    }
    public IEnumerable<Huurverzoek> GetAllGoedGekeurde()
    {
        return _repository.GetAllGoedGekeurde();
    }
    public HuurverzoekIdDTO GetById(Guid id)
    {
        var huurder = _repository.GetByID(id);
        if (huurder == null) { return null; }

        return new HuurverzoekIdDTO
        {
            HuurVerzoekId = id,
            HuurderID = huurder.HuurderID,
            beginDate = huurder.beginDate,
            endDate = huurder.endDate,
            isBevestigd = huurder.isBevestigd,
            approved = huurder.approved
        };
    }
    public void Update(Guid id, HuurverzoekIdDTO dto)
    {
        var huurder = _repository.GetByID(id);
        if (huurder == null) throw new KeyNotFoundException("Huurverzoek niet gevonden.");


        huurder.approved = dto.approved;
        huurder.isBevestigd = dto.isBevestigd;


        _repository.Update(huurder);
        _repository.Save();
    }
        
}

