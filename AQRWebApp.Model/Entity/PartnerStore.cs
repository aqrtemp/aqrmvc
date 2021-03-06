﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQRWebApp.Model.Entity
{
    public class PartnerStore: BaseEntity
    {
        public PartnerStore()
        {
            Line = 0;
        }

        public virtual Partner Partner { set; get; }
        public string StoreId {set;get;}
        public string StoreName {set;get;}
        public string AddressLine1 {set;get;}
        public string AddressLine2 {set;get;}
        public string City {set;get;}
        public string State {set;get;}
        public string Postcode {set;get;}
        public string Country {set;get;}
        public string Comment1 {set;get;}
        public string Comment2 {set;get;}
        public string Comment3 {set;get;}
        public string Email {set;get;}
        public string PhoneNo {set;get;}
        public string FaxNo {set;get;}
        public string DCI_MyerStore {set;get;}
        public string ReceiverID2 {set;get;}
        public int Line {set;get;}
        public string EntryName {set;get;}
        public string Import {set;get;}
        public string ImportNote {set;get;}
        public string Udf1 {set;get;}
        public string Udf2 {set;get;}
        public string Udf3 {set;get;}
        public string Udf4 {set;get;}
        public string Udf5 {set;get;}
        public double? Udf6 {set;get;}
        public double? Udf7 { set; get; }
        public double? Udf8 { set; get; }
        public double? Udf9 { set; get; }
        public double? Udf10 { set; get; }
    }
}
