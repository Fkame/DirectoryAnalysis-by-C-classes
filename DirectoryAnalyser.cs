using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Text;

using DirectoryAnalysis.Helpstaff;
using DirectoryAnalysis.Tests;

namespace DirectoryAnalysis
{
    public partial class DirectoryAnalyser
    {
        public DirectoryInfo DirectoryPath{ get; private set; }

        public FilesContainer filebuffer {get; private set; }

        public DirectoryAnalyser(DirectoryInfo directoryPath) 
        {
            this.DirectoryPath = directoryPath;
            filebuffer = new FilesContainer();
        }

        public void StartSomeJob()
        {
            // Создание шаблона папок и файлов
            Templator.LoadTemplates(DirectoryPath, false, false);

            // Загрузка файлов с диска в буффер
            this.LoadFilesToBuffer();

            // Просмотр буфера после загрузки туда файлов
            this.PrintFileBuffer(filebuffer, DirectoryPath);
            
            // Запуск отслеживателя изменений в директории и поддиректориях. Осторожно: работает асинхронно!
            this.StartWatchingForChangesInFiles();

            // Тут всякие тесты лежат
            //this.StartTests();
        }
       
        static void Main(string[] args)
        {
            string path = Path.Join(System.Environment.CurrentDirectory, "Resources", "Websites", "SomeSite1");
            DirectoryInfo directory = new DirectoryInfo(path);
            DirectoryAnalyser da = new DirectoryAnalyser(directory);

            Console.WriteLine($"Now some job is doing - dynamic analysing files in {path}");
            Console.WriteLine("Press q to quit the sample.");

            new Task(da.StartSomeJob);
            while (Console.Read() != 'q');

            
            da.StartWatchingForChangesInFiles();

           
            while (Console.Read() != 'q') ;
        }

        private void LoadFilesToBuffer()
        {
            filebuffer.Clear();
            string[] allFiles = Directory.GetFiles(DirectoryPath.FullName, "*", SearchOption.AllDirectories);
            foreach (string path in allFiles)
            {
                byte[] file = File.ReadAllBytes(path);
                filebuffer.Add(path, file);
            }
        }

        private void PrintFileBuffer(FilesContainer filebuffer, DirectoryInfo directoryPath)
        {
            Console.WriteLine("\n--- Writing Dictionaty ---");
            string[] keys = filebuffer.GetKeys();
            foreach (string key in keys)
            {
                StringBuilder shortPath = new StringBuilder(key);
                shortPath.Remove(0, directoryPath.FullName.Length);
                Console.WriteLine($"[{shortPath}] -> somefile...(null={filebuffer.GetValueByKey(key)==null}");
            }
            Console.WriteLine("--- End Writing Dictionary ---\n");
        }

        private void StartTests()
        {
            new TestFilesContainer(DirectoryPath).StartTest();
        }
    }
}
