//using WPR_project.DTO_s;
//using WPR_project.Models;
//using WPR_project.Repositories;
//using WPR_project.Services.Email;

//namespace WPR_project.Services
//{
//    public class ZakelijkeHuurderService
//    {
//        private readonly IZakelijkeHuurderRepository _repository;
//        private readonly IEmailService _emailService;

//        public ZakelijkeHuurderService(IZakelijkeHuurderRepository repository, IEmailService emailService)
//        {
//            _repository = repository;
//            _emailService = emailService;
//        }

//        public IEnumerable<ZakelijkeHuurderDTO> GetAll()
//        {
//            return _repository.GetAll().Select(h => new ZakelijkeHuurderDTO
//            {
//                Bedrijfsnaam = h.Bedrijfsnaam,
//                Adres = h.Adres,
//                KvKNummer = h.KvKNummer,
//                Abonnementstype = h.Abonnementstype
//            });
//        }

//        public ZakelijkeHuurderDTO GetById(int id)
//        {
//            var huurder = _repository.GetById(id);
//            if (huurder == null)
//            {
//                return null;
//            }
//            return new ZakelijkeHuurderDTO
//            {
//                Bedrijfsnaam = huurder.Bedrijfsnaam,
//                Adres = huurder.Adres,
//                KvKNummer = huurder.KvKNummer,
//                Abonnementstype = huurder.Abonnementstype
//            };
//        }

//        public void Add(ZakelijkeHuurderDTO zDto)
//        {
//            var zakelijkeHuurder = new ZakelijkeHuurder
//            {
//                Bedrijfsnaam = zDto.Bedrijfsnaam,
//                Adres = zDto.Adres,
//                KvKNummer = zDto.KvKNummer,
//                Abonnementstype = zDto.Abonnementstype
//            };
//            _repository.Add(zakelijkeHuurder);
//            _repository.Save();

//            // Stuur een e-mailbevestiging
//            var emailBody = $"Beste {zDto.Bedrijfsnaam},\n\nUw bedrijfsaccount is succesvol aangemaakt.\n\nBedankt!";
//            _emailService.SendEmail(zDto.KvKNummer + "@example.com", "Bevestiging Bedrijfsaccount", emailBody);
//        }

//        public void Update(int id, ZakelijkeHuurderDTO zDto)
//        {
//            var zakelijkeHuurder = _repository.GetById(id);
//            if (zakelijkeHuurder != null)
//            {
//                zakelijkeHuurder.Bedrijfsnaam = zDto.Bedrijfsnaam;
//                zakelijkeHuurder.Adres = zDto.Adres;
//                zakelijkeHuurder.KvKNummer = zDto.KvKNummer;
//                zakelijkeHuurder.Abonnementstype = zDto.Abonnementstype;
//                _repository.Update(zakelijkeHuurder);
//                _repository.Save();
//            }
//        }

//        public void Delete(int id)
//        {
//            _repository.Delete(id);
//            _repository.Save();
//        }

//        public void VoegMedewerkerToe(int zakelijkeHuurderId, MedewerkerDTO medewerkerDto)
//        {
//            var zakelijkeHuurder = _repository.GetById(zakelijkeHuurderId);
//            if (zakelijkeHuurder != null)
//            {
//                var medewerker = new Medewerker
//                {
//                    Naam = medewerkerDto.Naam,
//                    Email = medewerkerDto.Email
//                };
//                zakelijkeHuurder.Medewerkers.Add(medewerker);
//                _repository.Update(zakelijkeHuurder);
//                _repository.Save();

//                // Stuur een e-mailbevestiging naar de medewerker
//                var emailBody = $"Beste {medewerkerDto.Naam},\n\nU bent toegevoegd aan het bedrijfsaccount van {zakelijkeHuurder.Bedrijfsnaam}.";
//                _emailService.SendEmail(medewerkerDto.Email, "Medewerker Toegevoegd", emailBody);
//            }
//        }
//    }
//}
