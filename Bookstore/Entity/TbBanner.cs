using System;
using System.Collections.Generic;

namespace Bookstore.Entity
{
    public partial class TbBanner
    {
        public int BannerId { get; set; }
        public string? SubTitle { get; set; }
        public string? Title { get; set; }
        public string? Thumb { get; set; }
        public string? ListImage { get; set; }
        public string? UrlLink { get; set; }
        public bool Active { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool? IsNewfeed { get; set; }
    }
}
