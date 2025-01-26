using WPR_project.DTO_s;
using WPR_project.Models;
using WPR_project.Repositories;


namespace WPR_project.Services
{
    // Service voor het beheren van schademeldingen
    public class SchademeldingService
    {
        private readonly ISchademeldingRepository _repo;


        // Constructor: initialiseert de repository
        public SchademeldingService(
            ISchademeldingRepository repository)
        {
            _repo = repository;
        }
        // Haal een specifieke schademelding op via ID
        public SchademeldingDTO GetById(Guid id)
        {
            var schademelding = _repo.GetSchademeldingById(id);
            if (schademelding == null) { return null; }

            return new SchademeldingDTO
            {
                SchademeldingId = id,
                Beschrijving = schademelding.Beschrijving,
                Opmerkingen = schademelding.Opmerkingen,
                Status = schademelding.Status

            };
        }
        // Werk de status van een schademelding bij
        public void Update(Guid id, SchademeldingDTO dto)
        {
            var schademelding = _repo.GetSchademeldingById(id);
            if (schademelding == null) throw new KeyNotFoundException("Huurverzoek niet gevonden.");

            schademelding.Status = dto.Status;

            _repo.Update(schademelding);
            _repo.Save();
        }

        // Haal alle schademeldingen op
        public IQueryable<SchadeMeldingInfoDTO> GetAllSchademeldingen()
        {
            return _repo.GetAllSchademeldingen().Select(h => new SchadeMeldingInfoDTO
            {
                SchademeldingId = h.SchademeldingId,
                Beschrijving = h.Beschrijving,
                Datum = h.Datum,
                Status = h.Status,
                Opmerkingen = h.Opmerkingen,
                VoertuigId = h.VoertuigId,
                IsAfgehandeld = h.IsAfgehandeld,

            });
        }


        // Maak een nieuwe schademelding aan
        public void newSchademelding(SchademeldingDTO schademelding)
        {

            var melding = new Schademelding
            {
                SchademeldingId = Guid.NewGuid(),
                Beschrijving = schademelding.Beschrijving,
                Datum = schademelding.Datum,
                Opmerkingen = schademelding.Opmerkingen,
                Status = schademelding.Status,
                VoertuigId = schademelding.VoertuigId
            };

            _repo.Add(melding);
            _repo.Save();
        }
    }
}
