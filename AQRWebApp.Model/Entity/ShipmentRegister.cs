using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQRWebApp.Model.Entity
{
    public class ShipmentRegister: BaseEntity
    {
        public ShipmentRegister()
        {
            QtyUnit = 0;
            Carton = 0;
            Pallet = 0;
        }
        public virtual Client Client {set;get;}
        public System.DateTime DateReceived {set;get;}
        public virtual ShipmentId ShipmentId {set;get;}
        public int QtyUnit {set;get;}
        public int Carton {set;get;}
        public int Pallet {set;get;}
        public string ETA {set;get;}
        public string ShipLocation {set;get;}
        public string ChargeRef {set;get;}
        public string ShipArrivalConfirm {set;get;}
        public string ShipArrivalConfirm_SendMail {set;get;}
        public string ShipEmptyConfirm {set;get;}
        public string ShipEmptyConfirm_SendMail {set;get;}
        public string ShipmentOut {set;get;}
        public string ShipmentOut_SendMail {set;get;}
        public string EmailNotifyAQR {set;get;}
        public string EmailNotifyForwarder {set;get;}
        public string EmailNotifyVendor {set;get;}
        public string EntryName {set;get;}
        public string LoadType {set;get;}
        public string Note { set; get; }
    }
}
