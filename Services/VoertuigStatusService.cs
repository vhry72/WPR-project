﻿using WPR_project.Repositories;
using WPR_project.DTO_s;

namespace WPR_project.Services
{
    // Service voor het beheren van voertuigstatussen
    public class VoertuigStatusService
    {
        private readonly IVoertuigStatusRepository _repo;

        // Constructor: initialiseert de repository
        public VoertuigStatusService(IVoertuigStatusRepository voertuigStatusRepository)
        {
            _repo = voertuigStatusRepository;
        }

        // Haal alle voertuigstatussen op
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

        // Haal een specifieke voertuigstatus op via ID
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

        // Werk de status van een voertuig bij
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
