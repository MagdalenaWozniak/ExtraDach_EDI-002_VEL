// Decompiled with JetBrains decompiler
// Type: PMHelper.Persistency.DB.SqlServer.Parameter
// Assembly: EDI-002, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 87B9A626-26D9-4106-8F28-1106664B5525
// Assembly location: C:\Users\magdaw\Documents\ExtraDach\EDI\EDI-002_OS.dll

using System.Data;
using System.Data.SqlClient;

namespace PMHelper.Persistency.DB.SqlServer
{
  public class Parameter
  {
    public static SqlParameter Create(string name, SqlDbType type, object value)
    {
      SqlParameter sqlParameter = new SqlParameter(name, type);
      sqlParameter.Value = value;
      return sqlParameter;
    }
  }
}
