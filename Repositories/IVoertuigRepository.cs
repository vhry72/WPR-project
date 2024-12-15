using System;
using System.Collections.Generic;
using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IVoertuigRepository
    {
        IEnumerable<Voertuig> GetAvailableVoertuigen(string voertuigType = null);
        Voertuig GetFilteredVoertuigById(Guid id);
        IEnumerable<Voertuig> GetFilteredVoertuigen(string voertuigType, DateTime? startDatum, DateTime? eindDatum, string sorteerOptie);

        IEnumerable<Voertuig> GetVoertuigTypeVoertuigen(string voertuigType);

        Voertuig GetVoertuigById(Guid id);
        VoertuigStatus GetVoertuigStatus(Guid voertuigId);

        void updateVoertuig(Voertuig voertuig);
    }
}
