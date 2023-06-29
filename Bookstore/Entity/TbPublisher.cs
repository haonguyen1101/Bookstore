using System;
using System.Collections.Generic;

namespace Bookstore.Entity
{
    public partial class TbPublisher
    {
        public TbPublisher()
        {
            TbBooks = new HashSet<TbBook>();
        }

        public int PublisherId { get; set; }
        public string PublisherName { get; set; } = null!;
        public bool Status { get; set; }

        public virtual ICollection<TbBook> TbBooks { get; set; }
    }
}
