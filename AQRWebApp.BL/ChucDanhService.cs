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
    public class ChucDanhService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public IEnumerable<ChucDanh> GetAll()
        {
            return unitOfWork.ChucDanhRepository.Get(m => m.IsDisabled == false);
        }

        public IEnumerable<ChucDanhDto> GetAllExposeDto()
        {
            return from c in GetAll()
                   select new ChucDanhDto() { Id = c.Id, Name = c.Name};
        }
    }
}
