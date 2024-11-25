using WPR_project.DTO_s;
using WPR_project.Models;
using WPR_project.Repositories;

namespace WPR_project.Services
{
    public class ParticulierHuurderService
    {
        private readonly IHuurderRegistratieRepository _repository;

        //constructor voor het opslaan van de gegevens
        public ParticulierHuurderService (IHuurderRegistratieRepository repository)
        {
            _repository = repository;
        }       

        //public IEnumerable<ParticulierHuurderDTO> GetAll()
        //{

        //}



    }
}
