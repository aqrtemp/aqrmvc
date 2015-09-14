using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQRWebApp.Model.Entity
{
    public class ShipmentId: BaseEntity
    {
        public string ShipId {set;get;}
        public string ShipType {set;get;}
        public string ShipDoc { set; get; }
    }
}
