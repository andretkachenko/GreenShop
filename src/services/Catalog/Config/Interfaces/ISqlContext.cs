using System.Data.SqlClient;

namespace GreenShop.Catalog.Config.Interfaces
{
    public interface ISqlContext
    {
        SqlConnection Context { get; }
    }
}
