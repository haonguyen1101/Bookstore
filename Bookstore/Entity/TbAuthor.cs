using System;
using System.Collections.Generic;

namespace Bookstore.Entity
{
    public partial class TbAuthor
    {
        public TbAuthor()
        {
            TbBooks = new HashSet<TbBook>();
        }

        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = null!;
        public DateTime? BirthDate { get; set; }
        public bool Status { get; set; }

        public virtual ICollection<TbBook> TbBooks { get; set; }
    }
}
