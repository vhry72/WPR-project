using WPR_project.Models;
using System;
using System.Collections.Generic;
namespace WPR_project.Repositories
{
    public interface IBedrijfsMedewerkersRepository
    {
        // Interface voor de repositories om de methodes te erfen en de logica toe te voegen wat opgeslagen wordt in de DB

        void Delete(Guid medewerkerId);

        void Deactivate(Guid medewerkerId);
        BedrijfsMedewerkers GetMedewerkerById(Guid medewerkerId); 

        void Update(BedrijfsMedewerkers bedrijfsMedewerkers);
        void Save();
    }
}