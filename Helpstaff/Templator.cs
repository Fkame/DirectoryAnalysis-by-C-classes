using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace DirectoryAnalysis.Helpstaff
{
    public static class Templator
    {
        public static void CreateTemplateStructureIfNotExists(DirectoryInfo directoryPath)
        {
            string[] ways = { 
                Path.Join(directoryPath.FullName),
                Path.Join(directoryPath.FullName, "js"),
                Path.Join(directoryPath.FullName, "css"),
                Path.Join(directoryPath.FullName, "about"),
                Path.Join(directoryPath.FullName, "about", "js"),
                Path.Join(directoryPath.FullName, "about", "css"),
                Path.Join(directoryPath.FullName, "posts"),
                Path.Join(directoryPath.FullName, "posts", "js"),
                Path.Join(directoryPath.FullName, "posts", "css"),
                Path.Join(directoryPath.FullName, "posts", "subposts"),
                Path.Join(directoryPath.FullName, "posts", "subposts", "js"),
                Path.Join(directoryPath.FullName, "posts", "subposts", "css")
            };

            string[] files = { "index.html", "code.js", "style.css" };
            foreach (string way in ways)
            {
                DirectoryInfo path = new DirectoryInfo(way);
                if (!path.Exists) path.Create();

                if (path.Name != "js" & path.Name != "css") 
                {
                    string pathToFile = Path.Join(path.FullName, files[0]);
                    if (!File.Exists(pathToFile)) File.Create(pathToFile).Close();
                } 

                if (path.Name == "js") 
                {
                    string pathToFile = Path.Join(path.FullName, files[1]);
                    if (!File.Exists(pathToFile)) File.Create(pathToFile).Close();
                }  

                if (path.Name == "css") 
                {
                    string pathToFile = Path.Join(path.FullName, files[2]);
                    if (!File.Exists(pathToFile)) File.Create(pathToFile).Close();
                }               
            }
            
            Console.WriteLine("Structure created!");
        }

        public static void PrintAllFilesInDirectory(DirectoryInfo directoryPath, bool includeSubdirectories)
        {
            if (includeSubdirectories)
            {
                Console.WriteLine("All files in directory and subdirectories:");
                Console.WriteLine(string.Join('\n', Directory.GetFiles(directoryPath.FullName, "*", SearchOption.AllDirectories)));
            }
            else
            {
                Console.WriteLine("All files in directory:");
                Console.WriteLine(string.Join('\n', Directory.GetFiles(directoryPath.FullName, "*", SearchOption.TopDirectoryOnly)));
            }
        }

        public static void PrintInfoAboutDirectory(DirectoryInfo directory)
        {
            Console.WriteLine("***** Информация о каталоге *****\n");
            Console.WriteLine("Полный путь: {0}\nНазвание папки: {1}\nРодительский каталог: {2}\n" + 
                        "Время создания: {3}\nАтрибуты: {4}\nКорневой каталог: {5}",
                        directory.FullName, directory.Name, directory.Parent, 
                        directory.CreationTime, directory.Attributes, directory.Root
            );
        }
    }
}