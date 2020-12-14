using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

using DirectoryAnalysis.Helpstaff;
using DirectoryAnalysis.Tests;

namespace DirectoryAnalysis
{
    public class DirectoryAnalyser
    {
        public DirectoryInfo DirectoryPath {get; private set; }

        private FilesContainer filebuffer;

        public DirectoryAnalyser(DirectoryInfo directoryPath) 
        {
            this.DirectoryPath = directoryPath;
            filebuffer = new FilesContainer();
        }
       

        static void Main(string[] args)
        {
            string path = Path.Join(System.Environment.CurrentDirectory, "Resources", "Websites", "SomeSite1");
            DirectoryInfo directory = new DirectoryInfo(path);
            DirectoryAnalyser da = new DirectoryAnalyser(directory);

            da.LoadTemplates(directory);
            new TestFilesContainer(directory).StartTest();
            
            da.StartWatchingForChangesInFiles();

            Console.WriteLine("Press q to quit the sample.");
            while (Console.Read() != 'q') ;
        }

        /// <summary>
        /// Метод, запускающий в асинхронном режиме к основному алгоритму ещё и проверку на изменения в файлах сайта.
        /// Также, этот метод в последствии вызывает специальные методы, которые выполняют специальные действия при определённом событии.
        /// </summary>
        /// <returns></returns>
        public async void StartWatchingForChangesInFiles()
        {
            await Task.Run(StartWatching);
        }

        public void LoadTemplates(DirectoryInfo directory)
        {
            Templator.CreateTemplateStructureIfNotExists(directory);
            Console.WriteLine();
            Templator.PrintInfoAboutDirectory(directory);
            Console.WriteLine();
            Templator.PrintAllFilesInDirectory(directory, true);
        }

        private Task StartWatching()
        {
             // Создаётся новый FileSystemWatcher и прозводится его настройка
            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = DirectoryPath.FullName;
                watcher.IncludeSubdirectories = true;

                // Watch for changes in LastAccess and LastWrite times, and
                // the renaming of files or directories.
                watcher.NotifyFilter = NotifyFilters.LastAccess
                                    | NotifyFilters.LastWrite
                                    | NotifyFilters.FileName
                                    | NotifyFilters.DirectoryName;

                // Наблюдение за всеми файлами в дирректории
                watcher.Filter = "*.*";

                // Add event handlers.
                watcher.Changed += OnChanged;
                watcher.Created += OnCreated;
                watcher.Deleted += OnDeleted;
                watcher.Renamed += OnRenamed;

                Console.WriteLine("\nWatching Events creater");

                // Начало наблюдения
                watcher.EnableRaisingEvents = true;
                Console.WriteLine($"\nWatching for directory [{DirectoryPath}] and it subdirectories start!");

                while (true)
                {
                    watcher.WaitForChanged(WatcherChangeTypes.All);
                }
                /*
                // Ожидание, пока пользователь завершит программу
                Console.WriteLine("Press q to quit the sample.");
                while (Console.Read() != 'q') ;
                */
            }
        }

        #region Методы реагирования на изменения с файлами

        /// <summary>
        /// 1. Ищет файл, с которым произошли измнения, если файл найден - переходим к шагу 2, если файл не найден, переходим к шагу 4.
        /// 2. Если файл найден, сверяется содержимое хранимого в буфере образа.
        /// 3. Если содержимое совпадает, алгоритм завершается, если содержимое не совпадает - в хранилице помещается более актуальное
        /// 4. Алгоритм завершается
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnChanged(object source, FileSystemEventArgs e) 
        {

            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine($"{DateTime.Now} File: {e.FullPath} OnChanged -> {e.ChangeType}");
        }

        /// <summary>
        /// 1. Заменить путь к файлу на более актуальный в хранилище. Хранимые данные не трогать.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnRenamed(object source, RenamedEventArgs e) 
        {
            // Specify what is done when a file is renamed.
            Console.WriteLine($"{DateTime.Now} File: {e.OldFullPath} renamed to {e.FullPath} -> {e.ChangeType}");
        }
        
        /// <summary>
        /// 1. Добавить файл с полученным путем в хранилище.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnCreated(object source, FileSystemEventArgs e) 
        {
            Console.WriteLine($"{DateTime.Now} File: {e.FullPath} {e.ChangeType} OnCreated -> {e.ChangeType}");
        }

        /// <summary>
        /// 1. Удалить файл с таким путем из хранилища.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnDeleted(object source, FileSystemEventArgs e) 
        {
            Console.WriteLine($"{DateTime.Now} File: {e.FullPath} {e.ChangeType} OnDeleted -> {e.ChangeType}");   
        }   

        #endregion     
    }
}
