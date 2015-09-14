using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AQRWebApp.Model.Entity;
using AQRWebApp.Model.EntityDto;

namespace AQRWebApp.Models
{
    public class ClientPartnerModel
    {
        public IList<ClientDto> Clients { set; get; }
        public IList<PartnerDto> Partners { set; get; }
    }
}