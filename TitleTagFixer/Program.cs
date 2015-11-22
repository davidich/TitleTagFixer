using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TitleTagFixer
{
    class Program
    {
        static void Main(string[] args)
        {
            var folderPath = args.Length > 0 && Directory.Exists(args[0]) 
                ? args[0] 
                : AppDomain.CurrentDomain.BaseDirectory;


            foreach (var filePath in Directory.GetFiles(folderPath, "*.mp3"))
            {
                var file = TagLib.File.Create(filePath);

                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var fileTitleTag = file.Tag.Title;

                if (fileName.HasDateValue() && !fileTitleTag.HasDateValue())
                {
                    file.Tag.Title += " " + fileName.GetDatePart();
                    file.Save();

                    Console.WriteLine("{0} - updated: '{1}'", fileName, file.Tag.Title);
                }
                else
                {
                    Console.WriteLine("{0} - skipped", fileName);
                }      
            }

            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }
    }

    public static class StringExtentions
    {
        const string DatePattern = @"\d{4}-\d{2}-\d{2}";

        public static bool HasDateValue(this string input)
        {            
            return Regex.IsMatch(input, DatePattern);
        }

        public static string GetDatePart(this string input)
        {
            var a = Regex.Match(input, DatePattern);
            return a.Value;
        }
    }
}
