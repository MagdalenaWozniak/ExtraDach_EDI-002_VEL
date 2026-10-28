// Decompiled with JetBrains decompiler
// Type: PMHelper.Persistency.DB.SqlServer.ConnectionString
// Assembly: EDI-002, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 87B9A626-26D9-4106-8F28-1106664B5525
// Assembly location: C:\Users\magdaw\Documents\ExtraDach\EDI\EDI-002_OS.dll

namespace PMHelper.Persistency.DB.SqlServer
{
  public sealed class ConnectionString
  {
    public string Server { get; set; }

    public string DBName { get; set; }

    public int Timeout { get; set; }

    public string Username { get; set; }

    public string Password { get; set; }

    public string GetString() => string.Format("Persist Security Info=False;Integrated Security=False;UID={0};Password={1};Database={2};Server={3};Connect Timeout={4}", (object) this.Username, (object) this.Password, (object) this.DBName, (object) this.Server, (object) this.Timeout);
  }
}
