using System;
using System.IO;
using System.Threading.Tasks;

namespace DirectoryAnalysis
{
    public class EnterPoint
    {
        static void Main(string[] args)
        {
            string path = Path.Join(System.Environment.CurrentDirectory, "Resources", "Websites", "SomeSite1");
            DirectoryInfo directory = new DirectoryInfo(path);
            DirectoryAnalyser da = new DirectoryAnalyser(directory);

            Console.WriteLine($"Now some job is doing - dynamic analysing files in {path}");
            Console.WriteLine("Press q to quit the sample.");

            new Task(da.StartSomeJob).Start();
            while (Console.Read() != 'q');
        }
    }
}