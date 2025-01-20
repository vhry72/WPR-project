using System;
using System.Collections.Generic;
using WPR_project.DTO_s;
using WPR_project.Models;

namespace WPR_project.Repositories
{
    public interface IVoertuigRepository
    {
        // Interface voor de repositories om de methodes te erfen en de logica toe te voegen wat opgeslagen wordt in de DB

        IEnumerable<Voertuig> GetAvailableVoertuigen(string voertuigType = null);
        IQueryable<Voertuig> GetAllVoertuigen();
        Voertuig GetFilteredVoertuigById(Guid id);
        IEnumerable<Voertuig> GetFilteredVoertuigen(string voertuigType, DateTime? startDatum, DateTime? eindDatum, string sorteerOptie);

        IEnumerable<Voertuig> GetVoertuigTypeVoertuigen(string voertuigType);

        Voertuig GetVoertuigById(Guid id);
        VoertuigStatus GetVoertuigStatus(Guid voertuigId);
        Voertuig GetVoertuigByKenteken(string kenteken);
        public Voertuig GetByID(Guid Id);
        void updateVoertuig(Voertuig voertuig);
        void Delete(Guid id);
        void Add(Voertuig voertuig);
        void Save();
        
    }
}
