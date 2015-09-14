using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AQRWebApp.Model.Entity
{
    public class Person: BaseEntity
    {
        [MaxLength(100)]
        public string Lastname { set; get; }
        [MaxLength(50)]
        public string Firstname { set; get; }
        public System.DateTime? Birthday { set; get; }
        public virtual Gender Gender { set; get; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { set; get; }
        [MaxLength(100)]        
        public string Email { set; get; }     
        public string Note { set; get; }

        public virtual Department Department { set; get; }
        public virtual ChucDanh ChucDanh { set; get; }

        public System.DateTime? RecruitedDate { set; get; }
        public System.DateTime? NgayNghi { set; get; }
    }
}
