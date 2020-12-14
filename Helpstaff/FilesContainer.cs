using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace DirectoryAnalysis.Helpstaff
{
    public class FilesContainer
    {
        private Dictionary<string, FileAsBytes> container;

        public FilesContainer() 
        {
            this.container = new Dictionary<string, FileAsBytes>();
        }

        public byte[] GetValueByKey(string key)
        {
            FileAsBytes valueObj = new FileAsBytes();
            container.TryGetValue(key, out valueObj);
            return valueObj.GetBytesCopy();
        }

        public bool Remove(string key)
        {
            if (container.Remove(key)) return true;
            return false;
        }

        public bool Add(string key, byte[] value) 
        {
            if (container.TryAdd(key, new FileAsBytes(value))) return true;
            return false;
        }

        /// <summary>
        /// Возвращает массив ключей в виде массива строк (потому что ключи в виде строк и хранятся).
        /// </summary>
        /// <returns></returns>
        public string[] GetKeys()
        {
            string[] array = new string[container.Keys.Count];
            int count = 0;
            foreach (string key in container.Keys)
            {
                array[count] = key;
                count++;
            }
            return array;
        }

    }

    /// <summary>
    /// Вспомогательный промежуточный класс, так как в словарь нельзя поместить значение byte[]
    /// </summary>
    public class FileAsBytes
    {
        public byte[] BytesOfFile = null;
        public FileAsBytes(byte[] bytes) 
        {
            BytesOfFile = new byte[bytes.Length];
            Array.Copy(bytes, BytesOfFile, bytes.Length);
        }

        public FileAsBytes() 
        {
            BytesOfFile = null;
        }

        public bool IsByteArrayNull()
        {
            return BytesOfFile == null;
        }

        public byte[] GetBytesCopy() 
        {
            byte[] copy = new byte[BytesOfFile.Length];
            Array.Copy(BytesOfFile, copy, BytesOfFile.Length);
            return copy;
        }
    }
}