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
    public class ClientService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public IEnumerable<Client> GetAll()
        {
            return unitOfWork.ClientRepository.Get(m => m.IsDisabled == false);
        }

        public Client GetByClientId(string clientId)
        {
            return GetAll().FirstOrDefault(m => m.ClientId.Equals(clientId));
        }

        public IEnumerable<ClientDto> GetAllExposeDto()
        {
          
            return (from c in GetAll()
                    select new ClientDto
                    {
                        Id = c.Id,
                        IsDisabled = c.IsDisabled,
                        IsActive = c.IsActive ? "1" : "0",
                        ClientId = c.ClientId,
                        ClientName = c.ClientName,
                        ShortName = c.ShortName,
                        AddressLine1 = c.AddressLine1,
                        AddressLine2 = c.AddressLine2,
                        City = c.City,
                        State = c.State,
                        Postcode = c.Postcode,
                        EDINo = c.EDINo,
                        Mfrid = c.Mfrid,
                        NextPack = c.NextPack != null ? c.NextPack.Value.ToString() : String.Empty,
                        EndPack = c.EndPack != null ? c.EndPack.Value.ToString() : String.Empty,
                        NextASN = c.NextASN != null ? c.NextASN.Value.ToString() : String.Empty,
                        SupplierNo = c.SupplierNo,
                        TaxNo = c.TaxNo,
                        ABN = c.ABN,
                        Phone = c.Phone,
                        ClientContactEmail = c.ClientContactEmail,
                        AQRAMEmail = c.AQRAMEmail,
                        FreightforwarderEmail = c.FreightforwarderEmail,
                        Line = c.Line,
                        EntryName = c.EntryName,
                        Import = c.Import,
                        ImportNote = c.ImportNote
                    });
        }
    }
}
