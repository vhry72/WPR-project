using WPR_project.Models;
using WPR_project.Repositories;

namespace WPR_project.Services
{
    public class WagenparkBeheerderService
    {
        private readonly IWagenparkBeheerderRepository _repository;

        public WagenparkBeheerderService(IWagenparkBeheerderRepository repository)
        {
            _repository = repository;
        }
        //save
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

        public void VoegMedewerkerToe(Guid beheerderId, BedrijfsMedewerkers medewerker)
        {
            var beheerder = _repository.getBeheerderById(beheerderId);
            if (beheerder == null)
            {
                throw new KeyNotFoundException("Beheerder niet gevonden.");
            }

            if (beheerder.MedewerkerLijst.Any(m => m.BedrijfsMedewerkId == medewerker.BedrijfsMedewerkId))
            {
                throw new InvalidOperationException("Medewerker is al gekoppeld aan deze beheerder.");
            }

            beheerder.MedewerkerLijst.Add(medewerker);
            _repository.UpdateWagenparkBeheerder(beheerder);
            _repository.Save();
        }

        public void VerwijderMedewerker(Guid beheerderId, int medewerkerId)
        {
            var beheerder = _repository.getBeheerderById(beheerderId);
            if (beheerder == null)
            {
                throw new KeyNotFoundException("Beheerder niet gevonden.");
            }

            var medewerker = beheerder.MedewerkerLijst.FirstOrDefault(m => m.BedrijfsMedewerkId == medewerkerId);
            if (medewerker == null)
            {
                throw new KeyNotFoundException("Medewerker niet gevonden.");
            }

            beheerder.MedewerkerLijst.Remove(medewerker);
            _repository.UpdateWagenparkBeheerder(beheerder);
            _repository.Save();
        }
    }
}