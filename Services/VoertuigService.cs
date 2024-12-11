using System;
using System.Collections.Generic;
using WPR_project.Models;
using WPR_project.Repositories;

namespace WPR_project.Services
{
    public class VoertuigService
    {
        private readonly IVoertuigRepository _voertuigRepository;

        public VoertuigService(IVoertuigRepository voertuigRepository)
        {
            _voertuigRepository = voertuigRepository;
        }

        public IEnumerable<Voertuig> GetFilteredVoertuigen(string voertuigType, DateTime? startDatum, DateTime? eindDatum, string sorteerOptie)
        {
            if (startDatum.HasValue && eindDatum.HasValue && startDatum >= eindDatum)
            {
                throw new ArgumentException("Startdatum mag niet later of gelijk zijn aan de einddatum.");
            }

            return _voertuigRepository.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, sorteerOptie);
        }

        public Voertuig GetVoertuigDetails(int id)
        {
            var voertuig = _voertuigRepository.GetVoertuigById(id);
            if (voertuig == null)
            {
                throw new KeyNotFoundException("Voertuig niet gevonden.");
            }

            return voertuig;
        }

        public VoertuigStatus GetVoertuigStatus(int voertuigId)
        {
            return _voertuigRepository.GetVoertuigStatus(voertuigId);
        }
    }
}
