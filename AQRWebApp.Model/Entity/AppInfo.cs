using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQRWebApp.Model.Entity
{
    public class AppInfo:BaseEntity
    {
        public int currentGTIN {set;get;}
        public IList<string> newFieldOfProducts {set;get;}
        public IList<string> newFieldOfReceives {set;get;}
        public IList<string> newFieldOfDeliveries { set; get; }
    }
}
