using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQRWebApp.Model.Entity
{
    public class Product: BaseEntity
    {
        public Product()
        {
            Inactive = false;
        }
        public bool Inactive {set;get;}
        public string GTIN {set;get;}
        public string DoubleGTIN {set;get;}
        public virtual Client Client { set; get; }
        public virtual Style Style {set;get;}
        public virtual Colour Colour { set; get; }
        public string StyleGroup {set;get;}
        public string ColourGroup {set;get;}
        public virtual Size Size {set;get;}
        public string Description {set;get;}
        public virtual Partner Partner { set; get; }
        public string Note {set;get;}
        public string Season {set;get;}
        public string Keycode {set;get;}
        public System.DateTime GTINRecDate {set;get;}
        public string Category {set;get;}
        public string CategoryDescription {set;get;}
        public string Department {set;get;}
        public string SizeRange {set;get;}
        public int SOH {set;get;}
        public int GOHperMTR {set;get;}
        public string SOHLocation {set;get;}
        public int Pack {set;get;}
        public int StockCount {set;get;}
        public string StockCard {set;get;}
        public string CountBy {set;get;}
        public string NoCarton {set;get;}
        public string NbrCarton {set;get;}
        public string NoPallet {set;get;}
        public string NbrPallet {set;get;}
        public string PendingObsolete {set;get;}
        public string ObsoleteStatus {set;get;}
        public string ObsoleteLocation {set;get;}
        public System.DateTime ObsoleteDate {set;get;}
        public string CM3 {set;get;}
        public string Ratio {set;get;}
        public double? UnitPrice {set;get;}
        public string EntryName {set;get;}
        public int? Line {set;get;}
        public string Import {set;get;}
        public string ImportNote {set;get;}
        public string Udf {set;get;}
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
        public double? Udf20 {set; get;}
    }
}
