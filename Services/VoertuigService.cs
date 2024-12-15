﻿using System;
using System.Collections.Generic;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.DTO_s;

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

        public Voertuig GetVoertuigDetails(Guid id)
        {
            var voertuig = _voertuigRepository.GetVoertuigById(id);
            if (voertuig == null)
            {
                throw new KeyNotFoundException("Voertuig niet gevonden.");
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
            voertuig.voertuigBeschikbaar = DTO.VoertuigBeschikbaar;

            // Sla de wijzigingen op
            _voertuigRepository.updateVoertuig(voertuig);
        }

    }
}
