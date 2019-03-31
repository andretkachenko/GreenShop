using GreenShop.MVC.Models.Categories;
using System.Collections.Generic;

namespace GreenShop.MVC.ViewModels.Catalog
{
    public class IndexViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
    }
}
