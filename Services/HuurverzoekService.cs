using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using WPR_project.DTO_s;
using WPR_project.Models;
using WPR_project.Repositories;


namespace WPR_project.Services
{
    public class HuurverzoekService
    {
        private readonly IHuurVerzoekRepository _huurverzoekRepository;

        public HuurverzoekService(IHuurVerzoekRepository huurVerzoekRepository)
        {
            _huurverzoekRepository = huurVerzoekRepository;
        }

        public IEnumerable<HuurVerzoek> GetAllHuurVerzoeken()
        {
            return _huurverzoekRepository.GetAllHuurVerzoeken();
        }
        public HuurVerzoekDTO GetById(Guid id)
        {
            var huurder = _huurverzoekRepository.GetByID(id);
            if (huurder == null) { return null; }

            return new HuurVerzoekDTO
            {
                HuurderID = id,
                beginDate = huurder.beginDate,
                endDate = huurder.endDate,
                approved = huurder.approved
            };
        }
        public void Update(Guid id, HuurVerzoekDTO dto)
        {
            var huurder = _huurverzoekRepository.GetByID(id);
            if (huurder == null) throw new KeyNotFoundException("Huurverzoek niet gevonden.");

            huurder.approved = dto.approved;

            _huurverzoekRepository.Update(huurder);
          
        }
    }
}
