using System;
using System.Collections.Generic;

namespace Bookstore.Entity
{
    public partial class TbTransactStatus
    {
        public TbTransactStatus()
        {
            TbOrders = new HashSet<TbOrder>();
        }

        public int TransactStatusId { get; set; }
        public string? Desription { get; set; }

        public virtual ICollection<TbOrder> TbOrders { get; set; }
    }
}
