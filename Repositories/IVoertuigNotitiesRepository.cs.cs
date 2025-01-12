using Microsoft.EntityFrameworkCore;
using WPR_project.Models;
using WPR_project.DTO_s;

namespace WPR_project.Repositories
{
    public interface IVoertuigNotitiesRepository
    {
        IEnumerable<VoertuigNotities> GetVoertuigNotitiesByVoertuigId(Guid voertuigId);

        void AddVoertuigNotitie(VoertuigNotities voertuigNotities);

        void Save();
        
    }
}
