// Decompiled with JetBrains decompiler
// Type: SQL
// Assembly: EDI-002, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 87B9A626-26D9-4106-8F28-1106664B5525
// Assembly location: C:\Users\magdaw\Documents\ExtraDach\EDI\EDI-002_OS.dll

using Hydra;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;

public class SQL
{
  public object ExecuteScalar(string query, ArrayList parametry)
  {
    SqlConnection connection = new SqlConnection("Persist Security Info=False;Integrated Security=False;UID=cdnxl;password=hslxla2mssql;database=" + Runtime.Config.Baza + ";server=" + Runtime.Config.Serwer + ";Connect Timeout=30");
    object obj;
    try
    {
      connection.Open();
      SqlCommand sqlCommand = new SqlCommand(query, connection);
      if (parametry != null)
        sqlCommand.Parameters.AddRange((Array) parametry.ToArray());
      obj = sqlCommand.ExecuteScalar();
      connection.Close();
    }
    catch (Exception ex)
    {
      if (connection.State == ConnectionState.Open)
        connection.Close();
      throw ex;
    }
    return obj;
  }

  public int ExecuteNonQuery(string query, ArrayList parametry)
  {
    SqlConnection connection = new SqlConnection("Persist Security Info=False;Integrated Security=False;UID=cdnxl;password=hslxla2mssql;database=" + Runtime.Config.Baza + ";server=" + Runtime.Config.Serwer + ";Connect Timeout=30");
    int num;
    try
    {
      connection.Open();
      SqlCommand sqlCommand = new SqlCommand(query, connection);
      if (parametry != null)
        sqlCommand.Parameters.AddRange((Array) parametry.ToArray());
      num = sqlCommand.ExecuteNonQuery();
      connection.Close();
    }
    catch (Exception ex)
    {
      if (connection.State == ConnectionState.Open)
        connection.Close();
      throw ex;
    }
    return num;
  }

  public DataSet DataSetFromQuery(string query, ArrayList parametry)
  {
    DataSet dataSet = new DataSet();
    SqlConnection connection = new SqlConnection("Persist Security Info=False;Integrated Security=False;UID=cdnxl;password=hslxla2mssql;database=" + Runtime.Config.Baza + ";server=" + Runtime.Config.Serwer + ";Connect Timeout=30");
    connection.Open();
    SqlCommand selectCommand = new SqlCommand(query, connection);
    if (parametry != null)
      selectCommand.Parameters.AddRange((Array) parametry.ToArray());
    new SqlDataAdapter(selectCommand).Fill(dataSet);
    connection.Close();
    return dataSet;
  }
}
