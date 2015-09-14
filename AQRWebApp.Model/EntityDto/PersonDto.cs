using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQRWebApp.Model.EntityDto
{
    public class PersonDto: BaseEntityDto
    {
        public string Firstname { set; get; }
        public string Lastname { set; get; }
        public string Gender { set; get; }
        public string Birthday { set; get; }
        public int? GenderId { set; get; }
        public string GenderName { set; get; }
        public string PhoneNumber { set; get; }
        public string Email { set; get; }
        public int? DepartmentId { set; get; }
        public string DepartmentName { set; get; }
        public int? ChucDanhId { set; get; }
        public string ChucDanhName { set; get; }

        public string RecruitedDate { set; get; }
        public string NgayNghi { set; get; }

        public string Note { set; get; }
    }
}
