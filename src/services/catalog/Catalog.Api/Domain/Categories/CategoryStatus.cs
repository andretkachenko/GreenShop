using System.ComponentModel.DataAnnotations.Schema;

namespace GreenShop.Catalog.Api.Domain.Categories
{
    [Table("CategoryStatuses")]
    public class CategoryStatus
    {
        #region Properties
        public char Code { get; protected set; }
        public string Name { get; protected set; }
        #endregion

        #region Codes
        public const char Active = 'A';
        public const char Inactive = 'I';
        public const char Archived = 'R';
        #endregion
    }
}
