using WPR_project.Models;
using System;
using System.Collections.Generic;
namespace WPR_project.Repositories
{
    public interface IBedrijfsMedewerkersRepository
    {
        // Interface voor de repositories om de methodes te erfen en de logica toe te voegen wat opgeslagen wordt in de DB

        void AddMedewerker(BedrijfsMedewerkers medewerker);
        void Delete(Guid medewerkerId); // Gebruik Guid voor ID
        BedrijfsMedewerkers GetByEmailAndPassword(string email, string password);
        BedrijfsMedewerkers GetMedewerkerById(Guid medewerkerId); // Gebruik Guid voor ID
        IEnumerable<BedrijfsMedewerkers> GetAll();

        void Update(BedrijfsMedewerkers bedrijfsMedewerkers);
        void Save();
    }
}