using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQRWebApp.Model.EntityDto
{
    public class PartnerDto:BaseEntityDto
    {
        public string IsActive { set; get; }
        public string PartnerId { set; get; }
        public string PartnerName { set; get; }
        public string AddressLine1 { set; get; }
        public string AddressLine2 { set; get; }
        public string City { set; get; }
        public string State { set; get; }
        public string Postcode { set; get; }
        public string Contact { set; get; }
        public string Phone { set; get; }
        public string Fax { set; get; }
        public string SupplierNo { set; get; }
        public int Line { set; get; }
        public string EntryName { set; get; }
        public string Import { set; get; }
        public string ImportNote { set; get; }
    }
}
