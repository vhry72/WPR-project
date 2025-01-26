using System.ComponentModel.DataAnnotations;
using WPR_project.DTO_s;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.Services.Email;

namespace WPR_project.Services
{
    // Service voor het beheren van particuliere huurders
    public class ParticulierHuurderService
    {
        // Constructor: initialiseert de repository en de e-mailservice
        private readonly IHuurderRegistratieRepository _repository;
        private readonly IEmailService _emailService;

        // Haal de gegevens van een particuliere huurder op voor bewerking
        public ParticulierHuurderService(IHuurderRegistratieRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        // Haal de basisgegevens van een particuliere huurder op
        public ParticulierHuurderWijzigDTO GetGegevensById(Guid id)
        {
            var huurder = _repository.GetById(id);
            if (huurder == null) return null;

            return new ParticulierHuurderWijzigDTO
            {
                particulierEmail = huurder.particulierEmail,
                particulierNaam = huurder.particulierNaam,
                adress = huurder.adress,
                postcode = huurder.postcode,
                woonplaats = huurder.woonplaats,
                telefoonnummer = huurder.telefoonnummer

            };

        }
        // Haal de basisgegevens van een particuliere huurder op
        public ParticulierHuurderDTO GetById(Guid id)
        {
            var huurder = _repository.GetById(id);
            if (huurder == null) return null;

            return new ParticulierHuurderDTO
            {
                particulierId = huurder.particulierId,
                particulierNaam = huurder.particulierNaam,
                particulierEmail = huurder.particulierEmail,
                IsEmailBevestigd = huurder.IsEmailBevestigd
            };
        }

        // Werk de gegevens van een particuliere huurder bij
        public void Update(Guid id, ParticulierHuurderWijzigDTO dto)
        {
            var huurder = _repository.GetById(id);
            if (huurder == null) throw new KeyNotFoundException("Huurder niet gevonden.");

            huurder.particulierNaam = dto.particulierNaam;
            huurder.particulierEmail = dto.particulierEmail;
            huurder.adress = dto.adress;
            huurder.postcode = dto.postcode;
            huurder.woonplaats = dto.woonplaats;
            huurder.telefoonnummer = dto.telefoonnummer;



            _repository.Update(huurder);
            _repository.Save();
        }

        // Verwijder (deactiveer) een particuliere huurder en stuur een e-mailbevestiging
        public void Delete(Guid id)
        {
            var huurder = _repository.GetById(id);
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID is verplicht.");
            }
            _repository.DectivateParticulier(id);
            string bericht = $"Beste {huurder.particulierNaam},\n\n Uw account wordt verwijderd, \n\n Vriendelijke Groet, \n CarAndAll";
            _emailService.SendEmail(huurder.particulierEmail, "Account verwijderd", bericht);
        }
    }
}
