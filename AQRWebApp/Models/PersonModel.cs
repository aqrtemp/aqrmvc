using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AQRWebApp.Models;
using AQRWebApp.Model.Entity;
using AQRWebApp.Model.EntityDto;
using System.ComponentModel.DataAnnotations;

namespace AQRWebApp.Models
{
    public class PersonModel
    {
        public IList<Gender> Genders { set; get; }
        public IList<ChucDanh> ChucDanhs { set; get; }
        public IList<Department> Departments { set; get; }
        public IList<FilterName> ListOfFilterNames { set; get; }
        public int Filter { set; get; }
        public int DepartmentId { set; get; }
        public string ReportDate { set; get; }
    }

    public class FilterName
    {
        public int Id { set; get; }
        public string Name { set; get; }
    }

}