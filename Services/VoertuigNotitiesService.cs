using WPR_project.Models;
using WPR_project.Repositories;

namespace WPR_project.Services
{
    // Service voor het beheren van voertuignotities
    public class VoertuigNotitiesService
    {
        private readonly IVoertuigNotitiesRepository _repository;

        // Constructor: initialiseert de repository
        public VoertuigNotitiesService(IVoertuigNotitiesRepository repository)
        {
            _repository = repository;
        }

        // Haal alle notities van een bepaald voertuig op
        public IEnumerable<VoertuigNotities> GetVoertuigNotitiesByVoertuigId(Guid voertuigId)
        {
            return _repository.GetVoertuigNotitiesByVoertuigId(voertuigId);
        }

        // Voeg een nieuwe notitie toe aan een voertuig
        public void AddVoertuigNotitie(VoertuigNotities voertuigNotities)
        {
            _repository.AddVoertuigNotitie(voertuigNotities);
            _repository.Save();
        }
    }
}
