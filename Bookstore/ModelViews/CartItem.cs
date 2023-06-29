using Microsoft.AspNetCore.Mvc;
using Bookstore.Entity;

namespace Bookstore.ModelViews
{
    public class CartItem
    {
        public TbBook Book { get; set; }
        public int amount { get; set; }
        public double TotalMoney => amount * Book.Price.Value;
    }
}
