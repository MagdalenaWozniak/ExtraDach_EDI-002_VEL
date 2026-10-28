// Decompiled with JetBrains decompiler
// Type: EDI_002.Persist.Csv
// Assembly: EDI-002, Version=2.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 87B9A626-26D9-4106-8F28-1106664B5525
// Assembly location: C:\Users\magdaw\Documents\ExtraDach\EDI\EDI-002_OS.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EDI_002.Persist
{
  public class Csv
  {
    public sealed class Row : List<string>
    {
      public string Line { get; set; }
    }

    public sealed class CsvWriter : StreamWriter
    {
      [Obsolete("Backward compatibility")]
      public CsvWriter(Stream stream)
        : base(stream)
      {
      }

      public CsvWriter(Stream stream, Encoding encoding)
        : base(stream, encoding)
      {
      }

      public CsvWriter(string filename)
        : base(filename)
      {
      }

      public void WriteRow(Csv.Row row, char separator)
      {
        StringBuilder stringBuilder = new StringBuilder();
        bool flag = true;
        foreach (string str in (List<string>) row)
        {
          if (!flag)
            stringBuilder.Append(separator);
          if (str.IndexOfAny(new char[2]{ '"', ',' }) != -1)
            stringBuilder.AppendFormat("\"{0}\"", (object) str.Replace("\"", "\"\""));
          else
            stringBuilder.Append(str);
          flag = false;
        }
        row.Line = stringBuilder.ToString();
        this.WriteLine(row.Line);
      }
    }

    public sealed class CsvReader : StreamReader
    {
      public CsvReader(Stream stream)
        : base(stream)
      {
      }

      public CsvReader(string filename)
        : base(filename)
      {
      }

      public bool ReadRow(Csv.Row row, char separator)
      {
        row.Line = this.ReadLine();
        if (string.IsNullOrEmpty(row.Line))
          return false;
        int index1 = 0;
        int index2 = 0;
        while (index1 < row.Line.Length)
        {
          string str;
          if (row.Line[index1] == '"')
          {
            ++index1;
            int startIndex = index1;
            for (; index1 < row.Line.Length; ++index1)
            {
              if (row.Line[index1] == '"')
              {
                ++index1;
                if (index1 >= row.Line.Length || row.Line[index1] != '"')
                {
                  --index1;
                  break;
                }
              }
            }
            str = row.Line.Substring(startIndex, index1 - startIndex).Replace("\"\"", "\"");
          }
          else
          {
            int startIndex = index1;
            while (index1 < row.Line.Length && (int) row.Line[index1] != (int) separator)
              ++index1;
            str = row.Line.Substring(startIndex, index1 - startIndex);
          }
          if (index2 < row.Count)
            row[index2] = str;
          else
            row.Add(str);
          ++index2;
          while (index1 < row.Line.Length && (int) row.Line[index1] != (int) separator)
            ++index1;
          if (index1 < row.Line.Length)
            ++index1;
        }
        while (row.Count > index2)
          row.RemoveAt(index2);
        return row.Count > 0;
      }
    }
  }
}
