using WPR_project.DTO_s;
using WPR_project.Models;
using WPR_project.Repositories;
using WPR_project.Services.Email;

namespace WPR_project.Services
{
    public class BedrijfsMedewerkersService
    {
        private readonly IBedrijfsMedewerkersRepository _repository;
        private readonly IEmailService _emailService;
        private readonly ISchademeldingRepository _schaderepository;

        public BedrijfsMedewerkersService(IBedrijfsMedewerkersRepository repository, IEmailService emailService, ISchademeldingRepository schademeldingRepository)
        {
            _repository = repository;
            _emailService = emailService;
            _schaderepository = schademeldingRepository;
        }

        public BedrijfsMedewerkersDTO GetById(Guid id)
        {
            var medewerker = _repository.GetMedewerkerById(id);
            if (medewerker == null) return null;

            return new BedrijfsMedewerkersDTO
            {
                bedrijfsMedewerkerId = medewerker.bedrijfsMedewerkerId,
                medewerkerNaam = medewerker.medewerkerNaam,
                medewerkerEmail = medewerker.medewerkerEmail,
                abonnementId = medewerker.AbonnementId,
            };
        }

        public BedrijfsMedewerkerWijzigDTO GetGegevensById(Guid id)
        {
            var huurder = _repository.GetMedewerkerById(id);
            if (huurder == null) return null;

            return new BedrijfsMedewerkerWijzigDTO
            {
                medewerkerEmail = huurder.medewerkerEmail,
                medewerkerNaam = huurder.medewerkerNaam,
            };

        }



        public void Update(Guid id, BedrijfsMedewerkerWijzigDTO dto)
        {
            var huurder = _repository.GetMedewerkerById(id);
            if (huurder == null) throw new KeyNotFoundException("Huurder niet gevonden.");

            huurder.medewerkerNaam = dto.medewerkerNaam;
            huurder.medewerkerEmail = dto.medewerkerEmail;

            _repository.Update(huurder);
            _repository.Save();
        }


        public void Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("ID is verplicht.");
            }

            try
            {
                var bedrijfsMedewerker = _repository.GetMedewerkerById(id);
                if (bedrijfsMedewerker == null)
                {
                    throw new KeyNotFoundException("Wagenparkbeheerder niet gevonden.");
                }

                _repository.Deactivate(id);

                string bericht = $"Beste {bedrijfsMedewerker.medewerkerNaam},\n\nUw account is verwijderd.\n\nVriendelijke Groet,\nCarAndAll";
                _emailService.SendEmail(bedrijfsMedewerker.medewerkerEmail, "Account verwijderd", bericht);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
