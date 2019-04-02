using System;
using System.Data.SqlClient;

namespace GreenShop.Catalog.Config.Interfaces
{
    public interface ISqlContext : IDisposable
    {
        SqlConnection Connection { get; }
    }
}
