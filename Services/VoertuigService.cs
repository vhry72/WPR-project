using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.DTO_s;


namespace WPR_project.Services
{
    // Service voor het beheren van voertuigen
    public class VoertuigService
    {
        private readonly IVoertuigRepository _voertuigRepository;

        // Constructor: initialiseert de repository
        public VoertuigService(IVoertuigRepository voertuigRepository)
        {
            _voertuigRepository = voertuigRepository;
        }

        // Haal alle voertuigen op basis van een filter
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


        // Haal alle voertuigendetails op
        public Voertuig GetVoertuigDetails(Guid id)
        {
            var voertuig = _voertuigRepository.GetVoertuigById(id);
            if (voertuig == null)
            {
                throw new KeyNotFoundException("Voertuig niet gevonden.");
            }

            return voertuig;
        }

        // Haal alle voertuigen van een bepaald type op
        public IEnumerable<Voertuig> GetVoertuigTypeVoertuigen(string voertuigType)
        { 

            return _voertuigRepository.GetVoertuigTypeVoertuigen(voertuigType);
        }


        // wijzig de gegevens van een voertuig
        public void veranderGegevens(Guid id, VoertuigWijzigingDTO DTO)

        {
            var voertuig = _voertuigRepository.GetVoertuigById(id);
            if (voertuig == null)
            {
                throw new KeyNotFoundException("Voertuig niet gevonden.");
            }

            voertuig.merk = dto.merk;
            voertuig.model = dto.model;
            voertuig.kleur = dto.kleur;
            voertuig.prijsPerDag = dto.prijsPerDag;
            voertuig.bouwjaar = dto.bouwjaar;
            voertuig.kenteken = dto.kenteken;
            voertuig.AantalDeuren = dto.AantalDeuren;
            voertuig.AantalSlaapplekken = dto.AantalSlaapplekken;
            voertuig.Afbeelding = dto.Afbeelding;

            _voertuigRepository.updateVoertuig(voertuig);
        }


        // Wijzig de beschikbaarheid van een voertuig

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
            voertuig.notitie = DTO.notitie;

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

        // verwijder een voertuig
        public void Delete(Guid id)
        {
            var voertuig = _voertuigRepository.GetByID(id);
            if (voertuig == null) throw new KeyNotFoundException("Voertuig niet gevonden.");

            _voertuigRepository.Delete(id);
            _voertuigRepository.Save();
        }

        // Maak een nieuw voertuig aan
        public void newVoertuig(VoertuigDTO voertuig)
        {

            var voertuig1 = new Voertuig
            {
                voertuigId = Guid.NewGuid(),
                startDatum = voertuig.StartDatum,
                eindDatum = voertuig.EindDatum,
                voertuigBeschikbaar = voertuig.voertuigBeschikbaar,
                kleur = voertuig.kleur,
                merk = voertuig.merk,
                model = voertuig.model,
                kenteken = voertuig.kenteken,
                bouwjaar = voertuig.bouwjaar,
                voertuigType = voertuig.voertuigType,
                notitie = voertuig.notitie
                
            };

            _voertuigRepository.Add(voertuig1);
            _voertuigRepository.Save();
             
    }
    }
}
