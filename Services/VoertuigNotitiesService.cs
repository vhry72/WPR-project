using WPR_project.Models;
using WPR_project.Repositories;

namespace WPR_project.Services
{
    public class VoertuigNotitiesService
    {
        private readonly IVoertuigNotitiesRepository _repository;

        public VoertuigNotitiesService(IVoertuigNotitiesRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<VoertuigNotities> GetVoertuigNotitiesByVoertuigId(Guid voertuigId)
        {
            return _repository.GetVoertuigNotitiesByVoertuigId(voertuigId);
        }

        public void AddVoertuigNotitie(VoertuigNotities voertuigNotities)
        {
            _repository.AddVoertuigNotitie(voertuigNotities);
            _repository.Save();
        }
    }
}
