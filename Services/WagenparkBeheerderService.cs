using WPR_project.Models;
using WPR_project.Repositories;

namespace WPR_project.Services
{
    public class WagenparkBeheerderService
    {
        private readonly IWagenparkBeheerderRepository _repository;
        private readonly IZakelijkeHuurderRepository _zakelijkeHuurderRepository;
        private readonly IBedrijfsMedewerkersRepository _bedrijfsMedewerkersRepository;
        private IWagenparkBeheerderRepository @object;

        public WagenparkBeheerderService(
            IWagenparkBeheerderRepository repository,
            IZakelijkeHuurderRepository zakelijkeHuurderRepository,
            IBedrijfsMedewerkersRepository bedrijfsMedewerkersRepository)
        {
            _repository = repository;
            _zakelijkeHuurderRepository = zakelijkeHuurderRepository;
            _bedrijfsMedewerkersRepository = bedrijfsMedewerkersRepository;
            
        }

        public WagenparkBeheerderService(IWagenparkBeheerderRepository @object)
        {
            this.@object = @object;
        }

        public IEnumerable<WagenparkBeheerder> GetWagenparkBeheerders()
        {
            return _repository.GetWagenparkBeheerders();
        }

        public WagenparkBeheerder GetBeheerderById(Guid id)
        {
            return _repository.getBeheerderById(id);
        }

        public void AddWagenparkBeheerder(WagenparkBeheerder beheerder)
        {
            _repository.AddWagenparkBeheerder(beheerder);
            _repository.Save();
        }

        public void UpdateWagenparkBeheerder(Guid id, WagenparkBeheerder beheerder)
        {
            var existingBeheerder = _repository.getBeheerderById(id);
            if (existingBeheerder != null)
            {
                existingBeheerder.beheerderNaam = beheerder.beheerderNaam;
                existingBeheerder.email = beheerder.email;
                existingBeheerder.telefoonNummer = beheerder.telefoonNummer;

                _repository.UpdateWagenparkBeheerder(existingBeheerder);
                _repository.Save();
            }
            else
            {
                throw new KeyNotFoundException("Beheerder niet gevonden.");
            }
        }

        public void DeleteWagenparkBeheerder(Guid id)
        {
            var beheerder = _repository.getBeheerderById(id);
            if (beheerder != null)
            {
                _repository.DeleteWagenparkBeheerder(id);
                _repository.Save();
            }
            else
            {
                throw new KeyNotFoundException("Beheerder niet gevonden.");
            }
        }

        public void VoegMedewerkerToe(Guid zakelijkeHuurderId, BedrijfsMedewerkers medewerker)
        {
            var zakelijkeHuurder = _zakelijkeHuurderRepository.GetZakelijkHuurderById(zakelijkeHuurderId);
            if (zakelijkeHuurder == null)
            {
                throw new KeyNotFoundException("Zakelijke huurder niet gevonden.");
            }

            if (!medewerker.medewerkerEmail.EndsWith("@bedrijf.nl"))
            {
                throw new InvalidOperationException("Medewerker moet een bedrijfs-e-mailadres hebben.");
            }

            _bedrijfsMedewerkersRepository.Add(medewerker);
            _bedrijfsMedewerkersRepository.Save();
     
        }

        public void VerwijderMedewerker(Guid zakelijkeHuurderId, int medewerkerId)
        {
            var zakelijkeHuurder = _zakelijkeHuurderRepository.GetZakelijkHuurderById(zakelijkeHuurderId);
            if (zakelijkeHuurder == null)
            {
                throw new KeyNotFoundException("Zakelijke huurder niet gevonden.");
            }

            var medewerker = _bedrijfsMedewerkersRepository.GetMedewerkerById(medewerkerId);
            if (medewerker == null)
            {
                throw new KeyNotFoundException("Medewerker niet gevonden.");
            }

            _bedrijfsMedewerkersRepository.Delete(medewerkerId);
            _bedrijfsMedewerkersRepository.Save();

            
        }
    }
}
