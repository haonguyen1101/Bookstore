using System;
using System.Collections.Generic;

namespace Bookstore.Entity
{
    public partial class TbRole
    {
        public TbRole()
        {
            TbAdministrations = new HashSet<TbAdministration>();
        }

        public int RoleId { get; set; }
        public string? RoleName { get; set; }

        public virtual ICollection<TbAdministration> TbAdministrations { get; set; }
    }
}
