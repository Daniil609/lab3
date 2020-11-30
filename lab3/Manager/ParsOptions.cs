using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace lab3.Manager
{
    public class ParsOptions
    {
        private string path;
        public ParsOptions(string name)
        {
            path = name;
        }
        public T GetModel<T>()
        {
            var i = path.Length - 1;
            while (path[i] != '.') --i;
            var ext = path.Substring(i);
            if (ext == ".json")
            {
                IParser parser = new jsonParser(path, typeof(T));
                return parser.GetOptions<T>();
            }
            else
            {
                IParser parser = new XmlParser(path, typeof(T));
                return parser.GetOptions<T>();
            }
        }
    }
}
