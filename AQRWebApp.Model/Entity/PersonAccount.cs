using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQRWebApp.Model.Entity
{
    public class PersonAccount: BaseEntity
    {
        public int UserId { set; get; }
        public virtual Person Person { set; get; }
    }
}
