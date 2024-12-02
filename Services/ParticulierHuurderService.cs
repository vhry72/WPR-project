using Microsoft.AspNetCore.Http.HttpResults;
using WPR_project.DTO_s;
using WPR_project.Models;
using WPR_project.Repositories;

namespace WPR_project.Services
{
    public class ParticulierHuurderService
    {
        private readonly IHuurderRegistratieRepository _repository;

        //constructor voor het opslaan van de gegevens
        public ParticulierHuurderService(IHuurderRegistratieRepository repository)
        {
            _repository = repository;
        }

        //weergeef alle gegevens van DTO
        public IEnumerable<ParticulierHuurderDTO> GetAll()
        {
            return _repository.GetAll().Select(h => new ParticulierHuurderDTO
            {
                particulierId = h.particulierId,
                particulierNaam = h.particulierNaam,
                particulierEmail = h.particulierEmail
            });
        }

        public ParticulierHuurderDTO GetById(int id)
        {
            var huurder = _repository.GetById(id);
            if (huurder == null)
            {
                return null;
            }
            return new ParticulierHuurderDTO
            {
                particulierId = huurder.particulierId,
                particulierNaam = huurder.particulierNaam,
                particulierEmail = huurder.particulierEmail
            };
        }


        public void Add(ParticulierHuurderDTO pdto)
        {
            var huurder = new ParticulierHuurder
            {
                particulierId = pdto.particulierId,
                particulierNaam = pdto.particulierNaam,
                particulierEmail = pdto.particulierEmail
            };
            _repository.Add(huurder);
            _repository.Save();
        }
    }
}
