﻿using System;
using System.Collections.Generic;

namespace ABKS_project.Models
{
    public partial class Stock
    {
        public int StockId { get; set; }
        public int? ProductId { get; set; }
        public int? Quantity { get; set; }
    }
}
