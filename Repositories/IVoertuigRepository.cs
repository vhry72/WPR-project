using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IVoertuigRepository
    {
        IEnumerable<Voertuig> GetAvailableVoertuigen(string voertuigType = null);
        Voertuig GetVoertuigById(int id);

        IEnumerable<Voertuig> GetFilteredVoertuigen(string voertuigType, DateTime? startDatum, DateTime? eindDatum, string sorteerOptie);
        Voertuig GetFilteredVoertuigById(int id);

    }
}
