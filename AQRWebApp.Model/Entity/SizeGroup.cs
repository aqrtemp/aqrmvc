using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQRWebApp.Model.Entity
{
    public class SizeGroup: BaseEntity
    {
        public virtual IList<Size> Sizes { set; get; }
    }
}
