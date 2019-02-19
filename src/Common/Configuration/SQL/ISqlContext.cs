using System.Data.SqlClient;

namespace Common.Configuration.SQL
{
    public interface ISqlContext
    {
        SqlConnection Context { get; }
    }
}
