using Common.Models.Comments;
using System.Collections.Generic;

namespace GreenShop.MVC.ViewModels.Catalog
{
    public class CommentsViewModel
    {
        public IEnumerable<Comment> Comments { get; set; }
    }
}
