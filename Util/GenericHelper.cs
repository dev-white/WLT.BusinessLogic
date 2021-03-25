using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace WLT.BusinessLogic.Util
{
    public class CSV
    {
      public   static string  Get<T>( IEnumerable<T> data  )
        {      
            using (var mem = new MemoryStream())
            using (var writer = new StreamWriter(mem))
            using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
            //    csvWriter.Configuration.Delimiter = "\n";

                //csvWriter.NextRecord();

                csvWriter.WriteRecords<T>(data);                

                writer.Flush();

              return  Encoding.UTF8.GetString(mem.ToArray());
              
            }
        }
    }      
}
