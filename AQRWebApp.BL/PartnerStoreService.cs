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
    public class PartnerStoreService
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        public IEnumerable<PartnerStore> GetAll()
        {
            return unitOfWork.PartnerStoreRepository.Get(m => m.IsDisabled == false);
        }

        public IEnumerable<PartnerStoreDto> GetAllExposeDto()
        {

            return (from c in GetAll()
                    select new PartnerStoreDto
                    {
                        Id = c.Id,
                        IsDisabled = c.IsDisabled,
                        PartnerId = c.Partner != null ? c.Partner.PartnerId : String.Empty,
                        PartnerName = c.Partner != null ? c.Partner.PartnerName : String.Empty,
                        StoreId = c.StoreId,
                        StoreName = c.StoreName,
                        AddressLine1 = c.AddressLine1,
                        AddressLine2 = c.AddressLine2,
                        City = c.City,
                        State = c.State,
                        Postcode = c.Postcode,
                        Country = c.Country,
                        Comment1 = c.Comment1,
                        Comment2 = c.Comment2,
                        Comment3 = c.Comment3,
                        Email = c.Email,
                        PhoneNo = c.PhoneNo,
                        FaxNo = c.FaxNo,
                        DCI_MyerStore = c.DCI_MyerStore,
                        ReceiverID2 = c.ReceiverID2,
                        Udf1 = c.Udf1,
                        Udf2 = c.Udf2,
                        Udf3 = c.Udf3,
                        Udf4 = c.Udf4,
                        Udf5 = c.Udf5,
                        Udf6 = c.Udf6 != null ? c.Udf6.Value.ToString() : String.Empty,
                        Udf7 = c.Udf7 != null ? c.Udf7.Value.ToString() : String.Empty,
                        Udf8 = c.Udf8 != null ? c.Udf8.Value.ToString() : String.Empty,
                        Udf9 = c.Udf9 != null ? c.Udf9.Value.ToString() : String.Empty,
                        Udf10 = c.Udf10 != null ? c.Udf10.Value.ToString() : String.Empty,
                        Line = c.Line,
                        EntryName = c.EntryName,
                        Import = c.Import,
                        ImportNote = c.ImportNote
                    });
        }
    }
}
