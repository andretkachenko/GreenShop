using System;
using System.Data.SqlClient;

namespace GreenShop.Catalog.Api.Config.Interfaces
{
    public interface ISqlContext : IDisposable
    {
        SqlConnection Connection { get; }
    }
}
