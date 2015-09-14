using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQRWebApp.Model.Entity
{
    public class Size: BaseEntity
    {
        public int SizePosition { set; get; }
        public virtual SizeGroup SizeGroup { set; get; }
    }
}
