// Decompiled with JetBrains decompiler
// Type: PMHelper.Persistency.DB.SqlServer.Sql
// Assembly: EDI-002, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 87B9A626-26D9-4106-8F28-1106664B5525
// Assembly location: C:\Users\magdaw\Documents\ExtraDach\EDI\EDI-002_OS.dll

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace PMHelper.Persistency.DB.SqlServer
{
  public sealed class Sql
  {
    private string connectionString;

    public Sql(string connectionString) => this.connectionString = connectionString;

    public object ExecuteScalar(string query, List<SqlParameter> parameters)
    {
      using (SqlConnection connection = new SqlConnection(this.connectionString))
      {
        SqlCommand sqlCommand = new SqlCommand(query, connection);
        if (parameters != null)
          sqlCommand.Parameters.AddRange(parameters.ToArray());
        connection.Open();
        try
        {
          return sqlCommand.ExecuteScalar();
        }
        catch (SqlException ex)
        {
          throw ex;
        }
      }
    }

    public int ExectuteNonQuery(string query, List<SqlParameter> parameters)
    {
      using (SqlConnection connection = new SqlConnection(this.connectionString))
      {
        SqlCommand sqlCommand = new SqlCommand(query, connection);
        if (parameters != null)
          sqlCommand.Parameters.AddRange(parameters.ToArray());
        connection.Open();
        try
        {
          return sqlCommand.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
          throw ex;
        }
      }
    }

    public DataTable TableFromQuery(string query, List<SqlParameter> parameters)
    {
      DataTable dataTable = new DataTable();
      using (SqlConnection connection = new SqlConnection(this.connectionString))
      {
        SqlCommand selectCommand = new SqlCommand(query, connection);
        if (parameters != null)
          selectCommand.Parameters.AddRange(parameters.ToArray());
        connection.Open();
        new SqlDataAdapter(selectCommand).Fill(dataTable);
      }
      return dataTable;
    }
  }
}
