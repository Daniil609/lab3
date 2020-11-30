
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using ClassLibrary2;
namespace lab3.Service
{
    public class logger
    {
        FileSystemWatcher watcher;

        EtlXmlJsonOption options;

        ParsOptions pmanager;

        bool enabled = true;
        public logger()
        {
            if (File.Exists(@"C:\Users\source\repos\fuck\fuck\bin\Debug\netcoreapp3.1\application.json"))
            {
                pmanager = new ParsOptions(@"C:\Users\source\repos\fuck\fuck\bin\Debug\netcoreapp3.1\application.json");
            }
            else
            {
                pmanager = new ParsOptions(
                    @"C:\Users\source\repos\fuck\fuck\bin\Debug\netcoreapp3.1\config.xml");
            }

            options = pmanager.GetModel<EtlXmlJsonOption>();
            if (options == null) return;

            var path = options.pathes.Source1;
            watcher = new FileSystemWatcher(path);
            watcher.Created += Watcher_Created;
            watcher.Filter = "*" + options.cryptOptions.Extension;
        }
        public bool IsEmp()
        {
            if (options == null) return true;
            else return false;
        }
        public void Start()
        {
            if (!IsEmp())
            {
                watcher.EnableRaisingEvents = true;
                while (enabled)
                {
                    Thread.Sleep(5000);
                }
            }
        }

        public void Stop()
        {

            watcher.EnableRaisingEvents = false;
            enabled = false;
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            var filePath = e.FullPath;
            RecordEntry(filePath);
        }


        public void RecordEntry(string filePath)
        {


            options.Do(filePath);


        }
    }
}


