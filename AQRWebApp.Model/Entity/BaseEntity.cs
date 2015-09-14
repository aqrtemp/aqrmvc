using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AQRWebApp.Model.Entity
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            CreatedDate = System.DateTime.Now;
            ModifiedDate = System.DateTime.Now;
            IsDisabled = false;
        }

        [Key]
        public virtual int Id { set; get; }

        [MaxLength(50)]
        public virtual string Code { set; get; }

        [MaxLength(50)]
        public virtual string Name { set; get; }
        public virtual System.DateTime CreatedDate { set; get; }
        public virtual System.DateTime ModifiedDate { set; get; }
        public bool IsDisabled { set; get; }
    }
}
