using WPR_project.Models;
using System;
using System.Collections.Generic;
namespace WPR_project.Repositories
{
    public interface IBedrijfsMedewerkersRepository
    {
        void AddMedewerker(BedrijfsMedewerkers medewerker);
        void Delete(Guid medewerkerId); // Gebruik Guid voor ID
        BedrijfsMedewerkers GetMedewerkerById(Guid medewerkerId); // Gebruik Guid voor ID
        IEnumerable<BedrijfsMedewerkers> GetAll();
        void Save();
    }
}