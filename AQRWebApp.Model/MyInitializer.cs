using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace AQRWebApp.Model
{
    public class MyInitializer : DropCreateDatabaseIfModelChanges<AQRWebAppDbContext>
    {
        protected override void Seed(AQRWebAppDbContext context)
        {
            base.Seed(context);
        }
    }
}
