using GreenShop.Web.Mvc.App.Models.Categories;
using System.Collections.Generic;

namespace GreenShop.Web.Mvc.App.ViewModels.Catalog
{
    public class IndexViewModel
    {
        public IEnumerable<Category> Categories { get; set; }
    }
}
