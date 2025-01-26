using WPR_project.DTO_s;
using WPR_project.Models;
using WPR_project.Repositories;


namespace WPR_project.Services
{
    public class SchademeldingService
    {
        private readonly ISchademeldingRepository _repo;



        public SchademeldingService(
            ISchademeldingRepository repository)
        {
            _repo = repository;
        }

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
        public void Update(Guid id, SchademeldingDTO dto)
        {
            var schademelding = _repo.GetSchademeldingById(id);
            if (schademelding == null) throw new KeyNotFoundException("Huurverzoek niet gevonden.");

            schademelding.Status = dto.Status;

            _repo.Update(schademelding);
            _repo.Save();
        }

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
