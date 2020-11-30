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
    public class CopyOptions
    {
        public char w { get; set; }

        public string Copystr(string stpath, string Targetpath)
        {

            var i = stpath.Length - 1;
            while (w != stpath[i]) i--;
            var name = stpath.Substring(i);
            var newpath = Path.Combine(Targetpath, name);
            FileInfo fileInf = new FileInfo(stpath);
            if (fileInf.Exists)
            {
                fileInf.CopyTo(newpath, true);
            }

            return newpath;
        }
    }
}
