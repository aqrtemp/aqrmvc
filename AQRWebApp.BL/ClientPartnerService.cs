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
    public class ClientPartnerService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public IEnumerable<ClientPartner> GetAll()
        {
            return unitOfWork.ClientPartnerRepository.Get(m => m.IsDisabled == false);
        }

        public IEnumerable<ClientPartnerDto> GetAllExposeDto()
        {

            return (from c in GetAll()
                    select new ClientPartnerDto
                    {
                        Id = c.Id,
                        IsDisabled = c.IsDisabled,
                        ClientId = c.Client != null ? c.Client.ClientId : String.Empty,
                        ClientName = c.Client != null ? c.Client.ClientName : String.Empty,
                        PartnerId  = c.Partner != null ? c.Partner.PartnerId : String.Empty,
                        CarrierId = c.CarrierId,
                        PartnerEDINo = c.PartnerEDINo,
                        EDIDocCode = c.EDIDocCode,
                        EDIDocType = c.EDIDocType,
                        Directory = c.Directory,
                        Line = c.Line,
                        EntryName = c.EntryName,
                        Import = c.Import,
                        ImportNote = c.ImportNote
                    });
        }
    }
}
