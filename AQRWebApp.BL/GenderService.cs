using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AQRWebApp.DAL;
using AQRWebApp.Model.Entity;
using AQRWebApp.Model.EntityDto;

namespace AQRWebApp.BL
{
    public class GenderService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public IEnumerable<Gender> GetAll()
        {
            return unitOfWork.GenderRepository.Get(m => m.IsDisabled == false);
        }

        public IEnumerable<GenderDto> GetAllExposeDto()
        {
            return from c in GetAll()
                   select new GenderDto() { Id = c.Id, Name = c.Name };
        }
    }
}
