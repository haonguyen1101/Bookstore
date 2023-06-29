using System;
using System.Collections.Generic;

namespace Bookstore.Entity
{
    public partial class TbBookRating
    {
        public int RateId { get; set; }
        public int? Rate { get; set; }
        public int? BookId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual TbBook? Book { get; set; }
        public virtual TbCustomer? Customer { get; set; }
    }
}
