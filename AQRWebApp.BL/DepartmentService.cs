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
    public class DepartmentService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public IEnumerable<Department> GetAll()
        {
            return unitOfWork.DepartmentRepository.Get(m => m.IsDisabled == false);
        }

        public IEnumerable<DepartmentDto> GetAllExposeDto()
        {
            return from c in GetAll()
                   select new DepartmentDto() { Id=c.Id, Name = c.Name};
        }
    }
}
