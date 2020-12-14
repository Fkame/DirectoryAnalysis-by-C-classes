using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

using DirectoryAnalysis.Helpstaff;

namespace DirectoryAnalysis.Tests
{
    public class TestFilesContainer : ITestCode
    {

        private DirectoryInfo directoryPath;

        public TestFilesContainer (DirectoryInfo directoryPath)
        {
            this.directoryPath = directoryPath;
        }

        public void StartTest()
        {
            Console.WriteLine("\n--- TestFilesContainer ---");

            // Получим список путей ко всем файлам в указанной дирректории
            string[] filesPaths = Directory.GetFiles(directoryPath.FullName, "*", SearchOption.AllDirectories);

            Console.WriteLine("Get all inner files");
            
            // Получим из путей файлы и запишем их в буффер
            List<byte[]> filesInBytes = new List<byte[]>();
            for (int i = 0; i < filesPaths.Length; i++)
            {
                byte[] file = File.ReadAllBytes(filesPaths[i]);
                if (file == null)
                {
                    Console.WriteLine("!!! Null file by path " + filesPaths[i]);
                }

                filesInBytes.Add(file);
            }

            Console.WriteLine("Readed bytes of all files by paths");

            // Поместим всё содержимое в контейнер
            FilesContainer container = new FilesContainer();
            for (int i = 0; i < filesPaths.Length; i++)
            {
                if(!container.Add(filesPaths[i], filesInBytes[i]))
                {
                    Console.WriteLine("!!! Cannot add to dictionary file by path " + filesPaths[i]);
                }
            }

            Console.WriteLine("Files and paths addead to Dictionaty");

            // Провекрим, не произошло ли изменение ключей при добавлении
            string[] containerKeys = container.GetKeys();
            if (containerKeys.Length != filesPaths.Length)
            {
                Console.WriteLine("Keys were modified in adding - Length is not the same");
                return;
            }
            
            for (int i = 0; i < containerKeys.Length; i++)
            {
                if (containerKeys[i].Equals(filesPaths[i])) continue;
                Console.WriteLine($"Keys were modified in adding - [{i}] --> {filesPaths[i]} != {containerKeys[i]}");
                return;
            }

            Console.WriteLine("Keys were not modified in adding");

            // Проверим, не произошло ли изменение файлов при добавлении
            List<byte[]> containerValues = new List<byte[]>();
            foreach (string key in containerKeys)
            {
                containerValues.Add(container.GetValueByKey(key));
            }

            if (containerValues.Count != filesInBytes.Count) 
            {
                Console.WriteLine("Values were modified in adding - Length is not the same");
                return;
            }

            for (int i = 0; i < containerValues.Count; i++)
            {
                byte[] value1 = containerValues[i];
                byte[] value2 = filesInBytes[i];
                if (value1.Length != value2.Length)
                {
                    Console.WriteLine("Values were modified in adding - Length of file is not the same " + 
                                    $"--> {i} by path {containerKeys[i]}; Len in container = {value1.Length}, len original = {value2.Length}");
                    return; 
                }
                else 
                {
                    for (int j = 0; j < value1.Length; j++)
                    {
                        if (value1[j] == value2[j]) continue;
                        Console.WriteLine("Values were modified in adding - bytes are different");
                        return; 
                    }
                }
            }
            
            Console.WriteLine($"Values were not modified in adding");

            // Проверим на добавление дубликатов
            if(!container.Add(filesPaths[0], filesInBytes[0]))
            {
                Console.WriteLine("Dublicate key test is success");
            }
            else
            {
                Console.WriteLine("Dublicate key test is failder");
            }

            // Удаление из контейнера
            if (!container.Remove(filesPaths[0]))
            {
                Console.WriteLine("Deletion broked - do not see key");
                return;
            }
            if (container.Remove(filesPaths[0]))
            {
                Console.WriteLine("Deletion broked - see deleted key");
                return;
            }

            Console.WriteLine("Deletion is OK");
        }
    }
}