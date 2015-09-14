using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQRWebApp.Model.Entity
{
    public class Delivery: BaseEntity
    {
        public Delivery()
        {
            isPending = true;
            isManualImporting = true;
            Line = 0;
        }
        public bool isPending {set;get;}
        public bool isManualImporting {set;get;}
        public System.DateTime DateDelivery {set;get;}
        public virtual Product Product { set; get; }
        public string PONumber {set;get;}
        public string CustomerOrderNo {set;get;}
        public string DeliveryTo {set;get;}
        public string Advertise {set;get;}
        public string Note {set;get;}
        public string Style {set;get;}
        public string LotNo {set;get;}
        public string QtyUnit {set;get;}
        public string Carton {set;get;}
        public string QtyPallet {set;get;}
        public string GTIN {set;get;}
        public string SKU {set;get;}
        public string InnerPack {set;get;}
        public string PackType {set;get;}
        public string DispatchMethod {set;get;}
        public System.DateTime StartDate {set;get;}
        public System.DateTime FinishDate { set; get; }
        public string BookingNo {set;get;}
        public string ManifestNo {set;get;}
        public string CartonDimension {set;get;}
        public string CartonWeight {set;get;}
        public System.DateTime DateReceivedEDI {set;get;}
        public virtual Partner Partner { set; get; }
        public string ASNNo {set;get;}
        public string ASNExport {set;get;}
        public double? UnitPrice {set;get;}
        public double? UnitPriceExGST {set;get;}
        public double? POAmount {set;get;}
        public string POInvoiceNo {set;get;}
        public string AQRInvoiceNo {set;get;}
        public string EntryName {set;get;}
        public double Line {set;get;}
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
        public string PurposeCode {set;get;}
        public string POTypeCode { set; get; }
        public System.DateTime OrderedDate {set;get;}
        public string DeptNo {set;get;}
        public string DeptDescription {set;get;}
        public string OrderStatus {set;get;}
        public string POContractNo {set;get;}
        public System.DateTime RequestedDeliDate {set;get;}
        public string OrderedByStore {set;get;}
        public string EventType {set;get;}
        public string SeqLineNo {set;get;}
        public string ProductQty {set;get;}
        public virtual PartnerStore PartnerStore { set; get; }
        public string PromoPrice {set;get;}
        public string Changed {set;get;}
        public string PackID {set;get;}
        public System.DateTime DateAdded {set;get;}
        public virtual IList<FileInfo> filesInfo { set; get; }
    }
}
