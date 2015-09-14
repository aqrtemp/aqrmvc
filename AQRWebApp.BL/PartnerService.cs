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
    public class PartnerService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public IEnumerable<Partner> GetAll()
        {
            return unitOfWork.PartnerRepository.Get(m => m.IsDisabled == false);
        }

        public Partner GetByPartnerId(string PartnerId)
        {
            return GetAll().FirstOrDefault(m => m.PartnerId.Equals(PartnerId));
        }

        public IEnumerable<PartnerDto> GetAllExposeDto()
        {

            return (from c in GetAll()
                    select new PartnerDto
                    {
                        Id = c.Id,
                        IsDisabled = c.IsDisabled,
                        IsActive = c.IsActive ? "1" : "0",
                        PartnerId = c.PartnerId,
                        PartnerName = c.PartnerName,
                        AddressLine1 = c.AddressLine1,
                        AddressLine2 = c.AddressLine2,
                        City = c.City,
                        State = c.State,
                        Postcode = c.Postcode,
                        Contact = c.Contact,
                        Phone = c.Phone,
                        Fax = c.Fax,
                        SupplierNo = c.SupplierNo,
                        Line = c.Line,
                        EntryName = c.EntryName,
                        Import = c.Import,
                        ImportNote = c.ImportNote
                    });
        }
    }
}
