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
    public class Crypt
    {
        public int key { get; set; }
        public string Extension { get; set; }
        public string Cryptext { get; set; }

    }
    public string Decryptf(string path)
    {
        var text = File.ReadAllText(path);
        byte[] buf = Encoding.Unicode.GetBytes(text);
        for (var i = 0; i < buf.Length; i++)
        {
            buf[i] = (byte)(buf[i] ^ key);
        }

        text = Encoding.Unicode.GetString(buf);
        var tpath = path.Remove(path.Length - Extension.Length) + Cryptext;
        File.WriteAllText(tpath, text);
        return tpath;
    }
    public string Encryptf(string newtargetpath)
    {
        var text = File.ReadAllText(newtargetpath);

        byte[] buf = Encoding.Unicode.GetBytes(text);
        for (var i = 0; i < buf.Length; i++)
        {
            buf[i] = (byte)(buf[i] ^ key);
        }

        text = Encoding.Unicode.GetString(buf);
        var delfile = newtargetpath;
        newtargetpath = newtargetpath.Remove(newtargetpath.Length - Cryptext.Length) + Extension;
        File.WriteAllText(newtargetpath, text);
        File.Delete(delfile);

        return newtargetpath;
    }
}
