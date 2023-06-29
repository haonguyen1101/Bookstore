using System;
using System.Collections.Generic;

namespace Bookstore.Entity
{
    public partial class TbPost
    {
        public int PostId { get; set; }
        public string? Title { get; set; }
        public string? Scontents { get; set; }
        public string? Contents { get; set; }
        public string? Thumb { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Tags { get; set; }
        public bool IsHot { get; set; }
        public bool IsNewfeed { get; set; }
        public int? Views { get; set; }
        public bool Active { get; set; }
    }
}
