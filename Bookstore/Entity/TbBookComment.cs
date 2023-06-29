using System;
using System.Collections.Generic;

namespace Bookstore.Entity
{
    public partial class TbBookComment
    {
        public int CommentId { get; set; }
        public string Detail { get; set; } = null!;
        public bool Status { get; set; }
        public int BookId { get; set; }
        public int CustomerId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual TbBook Book { get; set; } = null!;
        public virtual TbCustomer Customer { get; set; } = null!;
    }
}
