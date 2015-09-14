using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQRWebApp.Model.EntityDto
{
    public class BaseEntityDto
    {
        public int Id { set; get; }
        public string Code { set; get; }
        public string Name { set; get; }
        public string CreatedDate { set; get; }
        public string ModifiedDate { set; get; }
        public bool IsDisabled { set; get; }
    }
}
