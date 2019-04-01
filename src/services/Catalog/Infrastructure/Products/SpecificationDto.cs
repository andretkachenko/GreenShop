﻿using System.Collections.Generic;

namespace GreenShop.Catalog.Infrastructure.Products
{
    public class SpecificationDto
    {
        public string Name { get; set; }
        public int MaxSelectionAvailable { get; set; }
        public IEnumerable<string> Options { get; set; }
    }
}
