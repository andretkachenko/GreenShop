﻿namespace GreenShop.Web.Mvc.App.Models.Categories
{
    public class Category : ICategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int ParentCategoryId { get; set; }
    }
}
