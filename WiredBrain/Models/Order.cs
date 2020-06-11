﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WiredBrain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public string Size { get; set; }
    }
}