using System;
using System.Collections.Generic;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.DTO_s;
using NuGet.Protocol.Core.Types;


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
            if (!startDatum.HasValue || !eindDatum.HasValue)
            {
                throw new ArgumentException("Startdatum en einddatum moeten beide ingevuld zijn.");
            }

            if (startDatum >= eindDatum)
            {
                throw new ArgumentException("Startdatum mag niet later of gelijk zijn aan de einddatum.");
            }

            return _voertuigRepository.GetFilteredVoertuigen(voertuigType, startDatum, eindDatum, sorteerOptie);
        }


        public Voertuig GetVoertuigDetails(Guid id)
        {
            var voertuig = _voertuigRepository.GetVoertuigById(id);
            if (voertuig == null)
            {
                throw new KeyNotFoundException("Voertuig niet gevonden.");
            }

            return voertuig;
        }
        public Voertuig GetVoertuigByKenteken(String kenteken)
        {
            var voertuig = _voertuigRepository.GetVoertuigByKenteken(kenteken);
            if (voertuig == null)
            {
                throw new KeyNotFoundException("Voertuig niet gevonden");
            }

            return voertuig;
        }

        public IEnumerable<Voertuig> GetVoertuigTypeVoertuigen(string voertuigType)
        { 

            return _voertuigRepository.GetVoertuigTypeVoertuigen(voertuigType);
        }

        public VoertuigStatus GetVoertuigStatus(Guid voertuigId)
        {
            return _voertuigRepository.GetVoertuigStatus(voertuigId);
        }

        public void UpdateUpVoertuig(Guid id, VoertuigUpDTO DTO)
        {
            var voertuig = _voertuigRepository.GetVoertuigById(id);
            if (voertuig == null)
            {
                throw new KeyNotFoundException("Voertuig niet gevonden.");
            }

            // Pas alleen de velden aan die in de DTO zijn opgenomen
            voertuig.startDatum = DTO.StartDatum;
            voertuig.eindDatum = DTO.EindDatum;
            voertuig.voertuigBeschikbaar = DTO.voertuigBeschikbaar;

            // Sla de wijzigingen op
            _voertuigRepository.updateVoertuig(voertuig);
        }
        public void UpdateVoertuig(Guid id, VoertuigDTO DTO)
        {
            var voertuig = _voertuigRepository.GetVoertuigById(id);
            if (voertuig == null)
            {
                throw new KeyNotFoundException("Voertuig niet gevonden.");
            }

            // Pas alleen de velden aan die in de DTO zijn opgenomen
            voertuig.startDatum = DTO.StartDatum;
            voertuig.eindDatum = DTO.EindDatum;
            voertuig.voertuigBeschikbaar = DTO.voertuigBeschikbaar;

            // Sla de wijzigingen op
            _voertuigRepository.updateVoertuig(voertuig);
        }
        public VoertuigDTO GetById(Guid id)
        {
            var voertuig = _voertuigRepository.GetByID(id);
            if (voertuig == null) { return null; }

            return new VoertuigDTO
            {
                voertuigId = voertuig.voertuigId,
                merk = voertuig.merk,
                model = voertuig.model, 
                voertuigBeschikbaar = voertuig.voertuigBeschikbaar,               
            };
        }
        
        public IEnumerable<Voertuig> GetAllVoertuigen()
        {
            return _voertuigRepository.GetAllVoertuigen();
        }
        public void Delete(Guid id)
        {
            var voertuig = _voertuigRepository.GetByID(id);
            if (voertuig == null) throw new KeyNotFoundException("Voertuig niet gevonden.");

            _voertuigRepository.Delete(id);
            _voertuigRepository.Save();
        }
        public void newVoertuig(VoertuigDTO voertuig)
        {

            var voertuig1 = new Voertuig
            {
                voertuigId = Guid.NewGuid(),
                voertuigBeschikbaar = voertuig.voertuigBeschikbaar,
                voertuigType = voertuig.voertuigType
            };

            _voertuigRepository.Add(voertuig1);
            _voertuigRepository.Save();
        }
    }
}
