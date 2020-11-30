using System;
namespace lab3.Manager
{
    public class Archive
    {
        public string Cryptextension { get; set; }
        public string ArchivizeCrypt(string tpath)
        {
            var stpath = tpath.Remove(tpath.Length - Cryptextension.Length) + ".gz";
            using (FileStream sourceStream = new FileStream(tpath, FileMode.Open))
            {
                using (FileStream targetStream = File.Create(stpath))
                {
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream);
                    }
                }
            }

            return stpath;
        }
        public string DearchivizeCrypt(string newpath)
        {
            string newtargetpath = newpath.Remove(newpath.Length - 3) + Cryptextension;
            using (FileStream sourceStream = new FileStream(newpath, FileMode.OpenOrCreate))
            {
                using (FileStream targetStream = File.Create(newtargetpath))
                {
                    using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(targetStream);
                    }
                }

                File.Delete(newpath);
            }

            return newtargetpath;
        }
    }
}
