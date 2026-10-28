// Decompiled with JetBrains decompiler
// Type: EDI_002.Dane
// Assembly: EDI-002, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 87B9A626-26D9-4106-8F28-1106664B5525
// Assembly location: C:\Users\magdaw\Documents\ExtraDach\EDI\EDI-002_OS.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace EDI_002
{
  public class Dane
  {
    private string cs;
    private SQL sql = new SQL();

    public Dane(string baza, string server) => this.cs = "Persist Security Info=False;Integrated Security=False;UID=cdnxl;password=hslxla2mssql;database=" + baza + ";server=" + server + ";Connect Timeout=30";

    public string KodTowaruUKontrahenta(int twrGIDNumer, int kntGIDNumer) => this.sql.ExecuteScalar("select \r\n            TwK_Kod from CDN.TwrKarty\r\n            join CDN.TwrKody on TwK_TwrNumer=Twr_GIDNumer\r\n            join CDN.TwrKodyKnt on TKK_TwKId=TwK_Id\r\n            where \r\n            Twr_GIDNumer=@numer\r\n            and TKK_KntNumer=@kntNumer", new ArrayList()
    {
      (object) new SqlParameter("@numer", (object) twrGIDNumer),
      (object) new SqlParameter("@kntNumer", (object) kntGIDNumer)
    })?.ToString();

        public string NumerZZ(int GIDNumer, int GIDTyp) => this.sql.ExecuteScalar("SELECT  Replace(cdn.NumerDokumentu(Zan_GIDTyp, ZaN_GIDTyp, ZaN_ZamTyp, Zan_ZamNumer, ZaN_ZamRok, ZaN_ZamSeria, ZaN_ZamMiesiac), 'Zam', 'ZZ') from cdn.ZamNag " +
                                           " where ZaN_GIDTyp=@typ and Zan_GIDNumer=@numer", new ArrayList()
    {
      (object) new SqlParameter("@numer", (object) GIDNumer),
      (object) new SqlParameter("@typ", (object) GIDTyp)
    })?.ToString();

        public List<Towar> TowaryZDokumentuZam(int dokumentGIDNumer)
    {
      DataTable table = this.sql.DataSetFromQuery("select\r\nE.ZaE_TwrNumer\r\n,(E.ZaE_Ilosc) ZaE_Ilosc\r\n,(\r\nselect top 1 ZeO_Opis from cdn.ZaEOpisy where ZeO_ZamTyp=960 and ZeO_ZamNumer in ((select ZZL_ZSGidNumer from cdn.ZamZamLinki \r\nwhere ZZL_ZZGidNumer=ZaE_GIDNumer and ZZL_ZZGidLp=ZaE_GIDLp)) \r\nand ZeO_ZamLp in (select ZZL_ZSGidLp from cdn.ZamZamLinki \r\nwhere ZZL_ZZGidNumer=ZaE_GIDNumer and ZZL_ZZGidLp=ZaE_GIDLp)\r\n)\r\n ElOpis --opis elementu ZS\r\n,isnull(atr_wartosc,1) [divider]  --podzielnik\r\nfrom cdn.ZamNag\r\nleft join CDN.ZamElem E on E.ZaE_GIDTyp=ZaN_GIDTyp and ZaN_GIDNumer=E.ZaE_GIDNumer\r\nleft join cdn.Atrybuty on Atr_ObiTyp=ZaE_TwrTyp and Atr_ObiNumer=E.ZaE_TwrNumer and Atr_AtkId=(select AtK_ID from cdn.AtrybutyKlasy where AtK_Nazwa='Dzielenie do eksportu CSV')  --atrybut podzielnik\r\nwhere \r\nZaN_ZamTyp = 1152 and \r\nZaN_GIDNumer=@numer", new ArrayList()
      {
        (object) new SqlParameter("@numer", (object) dokumentGIDNumer)
      }).Tables[0];
      List<Towar> towarList = new List<Towar>();
      foreach (DataRow row in (InternalDataCollectionBase) table.Rows)
        towarList.Add(new Towar()
        {
          GIDNumer = Convert.ToInt32(row["ZaE_TwrNumer"].ToString()),
          Ilosc = Convert.ToDecimal(row["ZaE_Ilosc"].ToString()),
          Opis = row["ElOpis"] != DBNull.Value ? row["ElOpis"].ToString() : string.Empty,
          Dzielnik = row["divider"] != DBNull.Value ? Convert.ToDecimal(row["divider"].ToString()) : 1M
        });
      return towarList;
    }

    public string SciezkaEksportu(int kntGIDTyp, int kntGIDNumer) => this.sql.ExecuteScalar("select Atr_Wartosc from CDN.Atrybuty \r\n              join CDN.AtrybutyKlasy on AtK_ID= Atr_AtkId\r\n              where AtK_Nazwa='EDI-Eksport' and  Atr_ObiTyp=@Typ and Atr_ObiNumer=@Numer", new ArrayList()
    {
      (object) new SqlParameter("@Typ", (object) kntGIDTyp),
      (object) new SqlParameter("@Numer", (object) kntGIDNumer)
    })?.ToString();
  }
}
