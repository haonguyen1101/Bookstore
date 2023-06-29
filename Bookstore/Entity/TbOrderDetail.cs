using System;
using System.Collections.Generic;

namespace Bookstore.Entity
{
    public partial class TbOrderDetail
    {
        public int OrderDetailId { get; set; }
        public int? OrderId { get; set; }
        public int? BookId { get; set; }
        public int? OrderNumber { get; set; }
        public int? Amount { get; set; }
        public int? Discount { get; set; }
        public int? TotalMoney { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Price { get; set; }
        public int? PromotionPrice { get; set; }

        public virtual TbBook? Book { get; set; }
        public virtual TbOrder? Order { get; set; }
    }
}
