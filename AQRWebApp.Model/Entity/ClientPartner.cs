using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQRWebApp.Model.Entity
{
    public class ClientPartner: BaseEntity
    {
        public ClientPartner()
        {
            Line = 0;
        }
        public virtual Client Client {set;get;}
        public virtual Partner Partner {set;get;}
        public string CarrierId {set;get;}
        public string PartnerEDINo {set;get;}
        public string EDIDocCode {set;get;}
        public string EDIDocType {set;get;}
        public string Directory {set;get;}
        public int Line {set;get;}
        public string EntryName {set;get;}
        public string Import {set;get;}
        public string ImportNote { set; get; }
    }
}
