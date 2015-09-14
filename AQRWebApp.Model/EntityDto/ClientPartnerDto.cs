using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQRWebApp.Model.EntityDto
{
    public class ClientPartnerDto:BaseEntityDto
    {
        public string ClientId { set; get; }
        public string ClientName { set; get; }
        public string PartnerId { set; get; }
        public string PartnerName { set; get; }
        public string CarrierId { set; get; }
        public string PartnerEDINo { set; get; }
        public string EDIDocCode { set; get; }
        public string EDIDocType { set; get; }
        public string Directory { set; get; }
        public int Line { set; get; }
        public string EntryName { set; get; }
        public string Import { set; get; }
        public string ImportNote { set; get; }
    }
}
