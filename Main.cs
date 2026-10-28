// Decompiled with JetBrains decompiler
// Type: EDI_002.Main
// Assembly: EDI-002, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 87B9A626-26D9-4106-8F28-1106664B5525
// Assembly location: C:\Users\magdaw\Documents\ExtraDach\EDI\EDI-002_OS.dll

using EDI_002.Persist;
using Hydra;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace EDI_002
{
    [SubscribeProcedure(Procedures.ZaNZamEdycjaZak, "EDI-002 VEL")]
    public class Main : Callback
  {
    private ClaWindow btnEDIEksport;
    private ClaWindow btnECODE;
    private ClaWindow wnd;
    private Dane dane;
    private int dokumentGIDNumer;

    private static string CreateFilename(DateTime dt, string extension) => (dt.Date.ToShortDateString() + "-" + dt.TimeOfDay.ToString().Substring(0, 8) + "." + extension).Replace("-", "").Replace(":", "");

    public override void Cleanup()
    {
    }

    public override void Init()
        {
            AddSubscription(true, 0, Events.JustAfterWindowOpening, new TakeEventDelegate(OnOpenwindow));
        }

        private bool OnOpenwindow(Procedures ProcedureId, int ControlId, Events Event)
        {

            this.dane = new Dane(Runtime.Config.Baza, Runtime.Config.Serwer);
            this.dokumentGIDNumer = (int)ZamNag.ZaN_GIDNumer;
            this.wnd = this.GetWindow();
            this.btnECODE = this.wnd.AllChildren["?KolektorPopupButton"];
            // ISSUE: method pointer
            wnd.OnAfterResizeWindow += new TakeEventDelegate(Wnd_OnBeforeSized);
            this.btnEDIEksport = this.wnd.AllChildren.Add((ControlTypes)9);
            this.btnEDIEksport.Visible = true;
            ((ClaWindowRaw)this.btnEDIEksport).TextRaw = "VEL";
            ((ClaWindowRaw)this.btnEDIEksport).ToolTipRaw = "Eksport towarów do pliku csv";
            // ISSUE: method pointer
            btnEDIEksport.OnAfterAccepted += new TakeEventDelegate(BtnExportZZAccepted_BeforeAccepted);
            return true;
        }

        

    private bool BtnExportZZAccepted_BeforeAccepted(
      Procedures procedureId,
      int controlId,
      Events Event)
    {
      DateTime now = DateTime.Now;
      string filename1 = Main.CreateFilename(now, "csv");
      string filename2 = Main.CreateFilename(now, "txt");
      string str1 = this.dane.SciezkaEksportu((int) ZamNag.ZaN_KntTyp, (int) ZamNag.ZaN_KntNumer);
      string path1;
      string path2;
      if (Directory.Exists(str1))
      {
        path1 = Path.Combine(str1, filename1);
        path2 = Path.Combine(str1, filename2);
      }
      else
      {
        path1 = filename1;
        path2 = filename2;
        int num = (int) MessageBox.Show("Katalog eksportu określony na karcie kontrahenta nie istnieje\nPliki zostanią wyeksportowane do folderu aplikacji ERP XL", "Eksport do pliku");
      }
      ICollection<Towar> towars = (ICollection<Towar>) this.dane.TowaryZDokumentuZam(this.dokumentGIDNumer);
      ICollection<TowarEksport> towarEksports = (ICollection<TowarEksport>) new List<TowarEksport>();
      foreach (Towar towar in (IEnumerable<Towar>) towars)
      {
        Decimal num1 = towar.Dzielnik > 0.00M ? towar.Dzielnik : 1.0M;
        Decimal num2 = Math.Ceiling(towar.Ilosc / num1);
        string str2 = this.dane.KodTowaruUKontrahenta(towar.GIDNumer, (int) ZamNag.ZaN_KntNumer);
                string str3 = this.dane.NumerZZ((int)ZamNag.ZaN_GIDNumer, (int)ZamNag.ZaN_GIDTyp);
        towarEksports.Add(new TowarEksport()
        {
            NumerZZ=str3,
          Kod = str2 != null ? str2.ToString() : string.Empty,
          Ilosc = num2
        });
      }
      StringBuilder stringBuilder = new StringBuilder();
      if (towars.Count == towarEksports.Count)
      {
        using (Stream stream = (Stream) new FileStream(path1, FileMode.Create))
        {
          using (Csv.CsvWriter csvWriter = new Csv.CsvWriter(stream, Encoding.UTF8))
          {
            foreach (TowarEksport towarEksport in (IEnumerable<TowarEksport>) towarEksports)
            {
              Csv.Row row1 = new Csv.Row();
                            row1.Add(towarEksport.NumerZZ);
              row1.Add(towarEksport.Kod);
              row1.Add(towarEksport.Ilosc.ToString());
              Csv.Row row2 = row1;
              csvWriter.WriteRow(row2, ';');
              stringBuilder.Append(towarEksport.NumerZZ+";"+towarEksport.Kod + ";" + towarEksport.Ilosc.ToString()  + ";\r\n");
            }
          }
        }
        using (StreamWriter streamWriter = new StreamWriter(path2))
          streamWriter.Write(stringBuilder.ToString());
        int num = (int) MessageBox.Show("Pliki zostały wyeksportowane", "Eksport do pliku");
      }
      return true;
    }

    private bool Wnd_OnBeforeSized(Procedures procedureId, int controlId, Events Event)
    {
      ClaWindow btnEdiEksport = this.btnEDIEksport;
      Rectangle bounds = this.btnECODE.Bounds;
      int x = bounds.X;
      bounds = this.btnECODE.Bounds;
      int y1 = bounds.Y;
      bounds = this.btnECODE.Bounds;
      int height1 = bounds.Height;
      int y2 = y1 - height1 +40;
      bounds = this.btnECODE.Bounds;
      int width = bounds.Width;
      bounds = this.btnECODE.Bounds;
      int height2 = bounds.Height;
      Rectangle rectangle = new Rectangle(x, y2, width, height2);
      btnEdiEksport.Bounds = rectangle;
      return true;
    }
  }
}
