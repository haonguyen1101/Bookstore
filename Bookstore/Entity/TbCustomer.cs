using System;
using System.Collections.Generic;

namespace Bookstore.Entity
{
    public partial class TbCustomer
    {
        public TbCustomer()
        {
            TbBookComments = new HashSet<TbBookComment>();
            TbBookRatings = new HashSet<TbBookRating>();
            TbOrders = new HashSet<TbOrder>();
        }

        public int CustomerId { get; set; }
        public string? FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? LocationId { get; set; }
        public int? District { get; set; }
        public int? Ward { get; set; }
        public DateTime? CreateDate { get; set; }
        public string? Password { get; set; }
        public string? Salt { get; set; }
        public bool Active { get; set; }

        public virtual TbLocation? Location { get; set; }
        public virtual ICollection<TbBookComment> TbBookComments { get; set; }
        public virtual ICollection<TbBookRating> TbBookRatings { get; set; }
        public virtual ICollection<TbOrder> TbOrders { get; set; }
    }
}
