﻿using ECommerceWebApplication.Data.Cart;

namespace ECommerceWebApplication.Data.ViewModels
{
    public class ShoppingCartVM
    {
        public ShoppingCart? ShoppingCart { get; set; }
        public double ShoppingCartTotal { get; set; }
    }
}