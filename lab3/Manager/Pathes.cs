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
    public class Pathes
    {
        public string Source1 { get; set; }
        public string Target1 { get; set; }

        public string ArchFolder { get; set; }
    }
}
