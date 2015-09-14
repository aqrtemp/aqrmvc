using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQRWebApp.Model.Entity
{
    public class Receive: BaseEntity
    {
        public Receive()
        { 
            IsPending = true;
            QtyUnit = 0;
            Carton = 0;
            InPack = 0;
            QtyPallet = 0;
            CM3 = 0;
            CtnWeight = 0;
            Line = 0;
        }
        public bool IsPending {set;get;}
        public System.DateTime DateReceived {set;get;}
        public virtual ShipmentId ShipmentId {set;get;}
        public virtual Product Product {set;get;}
        public string LotNo {set;get;}
        public string GTIN {set;get;}
        public int QtyUnit {set;get;}
        public int Carton {set;get;}
        public int InPack {set;get;}
        public string RatioDetails {set;get;}
        public int QtyPallet {set;get;}
        public System.DateTime StartDeli {set;get;}
        public System.DateTime FinishDeli {set;get;}
        public virtual Partner Partner {set;get;}
        public string Manufacture {set;get;}
        public string StyleArrivalInfo {set;get;}
        public string StyleLoc {set;get;}
        public string QAStatus {set;get;}
        public string QAInstructionStatus {set;get;}
        public string Meas_LxWxH {set;get;}
        public double CM3 {set;get;}
        public double CtnWeight {set;get;}
        public int Line {set;get;}
        public string EntryName {set;get;}
        public string Import {set;get;}
        public string ImportNote {set;get;}
        public string Udf1 {set;get;}
        public string Udf2 {set;get;}
        public string Udf3 {set;get;}
        public string Udf4 {set;get;}
        public string Udf5 {set;get;}
        public string Udf6 {set;get;}
        public string Udf7 {set;get;}
        public string Udf8 {set;get;}
        public string Udf9 {set;get;}
        public string Udf10 {set;get;}
        public double? Udf11 {set;get;}
        public double? Udf12 {set;get;}
        public double? Udf13 {set;get;}
        public double? Udf14 {set;get;}
        public double? Udf15 {set;get;}
        public double? Udf16 {set;get;}
        public double? Udf17 {set;get;}
        public double? Udf18 {set;get;}
        public double? Udf19 {set;get;}
        public double? Udf20 {set;get;}
        public virtual IList<FileInfo> FilesInfo { set; get; }
    }
}
