using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;

namespace DirectoryAnalysis.Helpstaff
{
    public class FilesContainer
    {
        private Dictionary<string, byte[]> container;

        public FilesContainer() 
        {
            this.container = new Dictionary<string, byte[]>();
        }

        public byte[] GetValueByKey(string key)
        {
            byte[] file = null;
            container.TryGetValue(key, out file);
            return this.GetBytesCopy(file);
        }

        public bool Remove(string key)
        {
            if (container.Remove(key)) return true;
            return false;
        }

        public bool Add(string key, byte[] value) 
        {
            if (container.TryAdd(key, this.GetBytesCopy(value))) return true;
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

        private byte[] GetBytesCopy(byte[] copyFrom) 
        {
            byte[] copy = new byte[copyFrom.Length];
            Array.Copy(copyFrom, copy, copyFrom.Length);
            return copy;
        }
    }
}