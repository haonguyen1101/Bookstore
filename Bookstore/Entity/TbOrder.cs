using System;
using System.Collections.Generic;

namespace Bookstore.Entity
{
    public partial class TbOrder
    {
        public TbOrder()
        {
            TbOrderDetails = new HashSet<TbOrderDetail>();
        }

        public int OrderId { get; set; }
        public int? CustomerId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public int TransactStatusId { get; set; }
        public bool Deleted { get; set; }
        public bool Paid { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? TotalMoney { get; set; }
        public string? Note { get; set; }
        public string? Address { get; set; }
        public int? LocationId { get; set; }
        public int? District { get; set; }
        public int? Ward { get; set; }

        public virtual TbCustomer? Customer { get; set; }
        public virtual TbTransactStatus TransactStatus { get; set; } = null!;
        public virtual ICollection<TbOrderDetail> TbOrderDetails { get; set; }
    }
}
