
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
    public class ETLXmlJSON
    {
        public Pathes pathes { get; set; }
        public Archive archivizeOptions { get; set; }
        public Crypt cryptOptions { get; set; }
        public CopyOptions copyOptions { get; set; }
        public void Do(string filePath)
        {
            var cryptstr = cryptOptions.Decryptf(filePath);
            var archstr = archivizeOptions.ArchivizeCrypt(cryptstr);
            var newstr = copyOptions.Copystr(archstr, pathes.Target1);
            var newcrypt = archivizeOptions.DearchivizeCrypt(newstr);
            var getstr = cryptOptions.Encryptf(newcrypt);

            string folder = Path.Combine(pathes.Target1, pathes.ArchFolder);
            var stpath = getstr.Remove(getstr.Length - cryptOptions.Extension.Length) + ".gz";
            var i = stpath.Length - 1;
            while (copyOptions.w != stpath[i]) i--;
            var name = stpath.Substring(i);
            var newpath = Path.Combine(folder, name);



            using (FileStream sourceStream = new FileStream(getstr, FileMode.Open))
            {
                using (FileStream targetStream = File.Create(newpath))
                {
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream);
                    }
                }
            }
        }

    }
    
}
