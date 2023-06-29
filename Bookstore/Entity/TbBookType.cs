using System;
using System.Collections.Generic;

namespace Bookstore.Entity
{
    public partial class TbBookType
    {
        public TbBookType()
        {
            TbBooks = new HashSet<TbBook>();
        }

        public int TypeId { get; set; }
        public string? TypeName { get; set; }
        public bool Status { get; set; }
        public int? Sort { get; set; }
        public string? Thumb { get; set; }
        public int? ParentId { get; set; }

        public virtual ICollection<TbBook> TbBooks { get; set; }
    }
}
