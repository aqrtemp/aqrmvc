using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQRWebApp.Model.Entity
{
    public class Client: BaseEntity
    {
        public Client()
        {
            IsActive = true;
            Line = 0;
        }
        public bool IsActive {set;get;}
        public string ClientId {set;get;}
        public string ClientName {set;get;}
        public string ShortName {set;get;}
        public string AddressLine1 {set;get;}
        public string AddressLine2 {set;get;}
        public string City {set;get;}
        public string State {set;get;}
        public string Postcode {set;get;}
        public string EDINo {set;get;}
        public string Mfrid {set;get;}
        public int? NextPack {set;get;}
        public int? EndPack {set;get;}
        public int? NextASN {set;get;}
        public string SupplierNo {set;get;}
        public string TaxNo {set;get;}
        public string ABN {set;get;}
        public string Phone {set;get;}
        public string ClientContactEmail {set;get;}
        public string AQRAMEmail {set;get;}
        public string FreightforwarderEmail {set;get;}
        public int Line {set;get;}
        public string EntryName {set;get;}
        public string Import {set;get;}
        public string ImportNote { set; get; }
    }
}
