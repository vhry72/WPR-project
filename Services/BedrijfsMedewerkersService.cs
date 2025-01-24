using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
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

        public IEnumerable<BedrijfsMedewerkersDTO> GetAll()
        {
            return _repository.GetAll().Select(m => new BedrijfsMedewerkersDTO
            {
                bedrijfsMedewerkerId = m.bedrijfsMedewerkerId,
                medewerkerNaam = m.medewerkerNaam,
                medewerkerEmail = m.medewerkerEmail,
            });
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
            };
        }

        public BedrijfsMedewerkersDTO GetByEmailAndPassword(string email, string wachtwoord)
        {
            var medewerker = _repository.GetByEmailAndPassword(email, wachtwoord);
            if (medewerker == null) return null;

            return new BedrijfsMedewerkersDTO
            {
                bedrijfsMedewerkerId = medewerker.bedrijfsMedewerkerId,
                medewerkerNaam = medewerker.medewerkerNaam,
                medewerkerEmail = medewerker.medewerkerEmail,
            };
        }

        public void Register(BedrijfsMedewerkers medewerker)
        {
            var validationContext = new ValidationContext(medewerker);
            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(medewerker, validationContext, validationResults, true))
            {
                var errors = validationResults.Select(vr => vr.ErrorMessage).ToList();
                throw new ArgumentException($"Validatie mislukt: {string.Join(", ", errors)}");
            }

            medewerker.bedrijfsMedewerkerId = Guid.NewGuid();

            _repository.AddMedewerker(medewerker);
            _repository.Save();

            
            var emailBody = $"Beste {medewerker.medewerkerNaam},<br><br>U bent ingeschreven op een bedrijfsAbonnement:<br><a href= >Bedankt voor uw registratie</a>";
            _emailService.SendEmail(medewerker.medewerkerEmail, "Veel plezier", emailBody);
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

        public IQueryable<SchademeldingDTO> GetAllSchademeldingen()
        {
            return _schaderepository.GetAllSchademeldingen().Select(h => new SchademeldingDTO
            {
                SchademeldingId = h.SchademeldingId,
                Beschrijving = h.Beschrijving,
                Datum = h.Datum,
                Status = h.Status,
                Opmerkingen = h.Opmerkingen,
                VoertuigId = h.VoertuigId,
                
            });
        }


        public List<SchademeldingDTO> GetSchademeldingByVoertuigId(Guid voertuigId)
        {
            
            var schademeldingen = _schaderepository.GetSchademeldingByVoertuigId(voertuigId);

            
            var schademeldingDtos = schademeldingen.Select(s => new SchademeldingDTO
            {
                SchademeldingId = s.SchademeldingId,
                Beschrijving = s.Beschrijving,
                Datum = s.Datum,
                Status = s.Status,
                Opmerkingen = s.Opmerkingen,
                VoertuigId= s.VoertuigId,
                
            }).ToList();

            return schademeldingDtos;
        }
        public void updateSchademelding(Guid id, SchademeldingDTO DTO)
        {
            var schademelding = _schaderepository.GetSchademeldingById(id);
            if (schademelding == null)
            {
                throw new KeyNotFoundException("Voertuig niet gevonden.");
            }
            schademelding.SchademeldingId= id;
            schademelding.Beschrijving= DTO.Beschrijving;
            schademelding.Datum= DTO.Datum;
            schademelding.Status= DTO.Status;
            schademelding.Opmerkingen= DTO.Opmerkingen;
            schademelding.VoertuigId = DTO.VoertuigId;

            _schaderepository.updateSchademelding(schademelding);
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

            _schaderepository.Add(melding);
            _schaderepository.Save();            
        }
    }
}
