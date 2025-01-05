using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.DTO_s;
using NuGet.Protocol.Core.Types;

namespace WPR_project.Services
{
    public class VoertuigStatusService
    {
        private readonly IVoertuigStatusRepository _repo;

        public VoertuigStatusService(IVoertuigStatusRepository voertuigStatusRepository)
        {
            _repo = voertuigStatusRepository;
        }
        public IQueryable<VoertuigStatusDTO> GetAllVoertuigStatussen()
        {
            return _repo.GetAllVoertuigenStatussen().Select(h => new VoertuigStatusDTO
            {
                VoertuigStatusId = h.VoertuigStatusId,
                onderhoud = h.onderhoud,
                schade = h.schade,
                verhuurd = h.verhuurd,
                voertuigId = h.voertuigId
            });
        }
        public VoertuigStatusDTO GetById(Guid id)
        {
            var status = _repo.GetByID(id);
            if (status == null) { return null; }

            return new VoertuigStatusDTO
            {
                VoertuigStatusId = id,
                verhuurd = status.verhuurd,
                onderhoud = status.onderhoud,
                schade = status.schade,
                voertuig = status.voertuig,
                voertuigId = status.voertuigId

            };
        }
        public void Update(Guid id, VoertuigStatusDTO dto)
        {
            var status = _repo.GetByID(id);
            if (status == null) throw new KeyNotFoundException("Status niet gevonden.");
            status.verhuurd = dto.verhuurd;           
            _repo.Update(status);
            _repo.Save();
        }
    }
}
