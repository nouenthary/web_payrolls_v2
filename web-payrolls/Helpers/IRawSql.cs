using System.Data;

namespace web_payrolls.Helpers
{
  public interface RawSql
  {
    DataTable GetDataSource(string sql);

    void ExecuteQuery(string sql);
  }
}
