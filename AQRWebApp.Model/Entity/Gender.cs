using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AQRWebApp.Model.Entity
{
    public class Gender : BaseEntity
    {
        public virtual IList<Person> Person { set; get; }
    }
}
