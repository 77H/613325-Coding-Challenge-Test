using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WorkLogParser;

// using (var reader = new StreamReader("path\\to\\file"))
// using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
// {
//     csv.Read();
//     csv.ReadHeader();
//     while (csv.Read())
//     {
//         var records = csv.GetRecord<Foo>();
//     }
// }
// var config = new CsvConfiguration(CultureInfo.InvariantCulture)
// {
//     PrepareHeaderForMatch = args =>args.Header.ToLower(),
// };
// public class Foo{
//     public int Id {get; set;}
//     public string Name {get; set;}
// }

// public class FooMap : ClassMap<Foo>
// {
//     public FooMap()
//     {
//         Map(m => m.Id).Name("id");
//         Map(m => m.Name).Name("name");
//     }
// }

namespace WorkLogParser
{
    public class WorkLogEntry
    {
        public int StaffID {get; set;}
        public string Name {get; set;}
        public DateTime Date {get; set;}
        public TimeSpan ShiftStart{get; set;}
        public TimeSpan ShiftEnd{get; set;}
    }
}

class Program
{
    static void Main(string[] args)
    {
        var path = "/Users/th/Desktop/613325 – Applications Developer Coding Challenge/test/ConsoleApp1/work_log.csv";

        var entries = new List <WorkLogEntry>();

        try
        {
            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HeaderValidated = null,
                MissingFieldFound = null
            });
            entries = csv.GetRecords<WorkLogEntry>().ToList();


            var grouped = entries.GroupBy(e => new {e.StaffID, e.Name}).Select(g => new
            {
                g.Key.StaffID,
                g.Key.Name,
                totalhours = g.Sum(e =>(e.ShiftEnd - e.ShiftStart).TotalHours)
            });

            foreach (var staff in grouped){
                Console.WriteLine($"Staff: {staff.Name} (ID: {staff.StaffID}) - Total Hours {staff.totalhours:F1}");
            }

        }
        catch (Exception e)
        {
            Console.WriteLine($"Error Occured: {e.Message}");
        }
    }
}
