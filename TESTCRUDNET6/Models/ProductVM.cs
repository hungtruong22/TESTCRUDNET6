﻿using TESTCRUDNET6.Data;

namespace TESTCRUDNET6.Models
{
    public class ProductVM
    {
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public int CategoryId { get; set; }
    }
}
