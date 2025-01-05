using NuGet.Protocol.Core.Types;
using WPR_project.DTO_s;
using WPR_project.Repositories;
using WPR_project.Services.Email;

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
    }
}
