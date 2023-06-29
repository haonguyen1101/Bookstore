using System;
using System.Collections.Generic;

namespace Bookstore.Entity
{
    public partial class TbBook
    {
        public TbBook()
        {
            TbBookComments = new HashSet<TbBookComment>();
            TbBookRatings = new HashSet<TbBookRating>();
            TbOrderDetails = new HashSet<TbOrderDetail>();
        }

        public int BookId { get; set; }
        public string? BookName { get; set; }
        public string? Author { get; set; }
        public string? Desription { get; set; }
        public bool Status { get; set; }
        public bool IsHot { get; set; }
        public string? Image { get; set; }
        public string? ListImage { get; set; }
        public int? Quantity { get; set; }
        public int? Price { get; set; }
        public int? PromotionPrice { get; set; }
        public string? Language { get; set; }
        public string? Translator { get; set; }
        public int? Page { get; set; }
        public int? PublishYear { get; set; }
        public int? TypeId { get; set; }
        public int? PublisherId { get; set; }

        public virtual TbPublisher? Publisher { get; set; }
        public virtual TbBookType? Type { get; set; }
        public virtual ICollection<TbBookComment> TbBookComments { get; set; }
        public virtual ICollection<TbBookRating> TbBookRatings { get; set; }
        public virtual ICollection<TbOrderDetail> TbOrderDetails { get; set; }
    }
}
