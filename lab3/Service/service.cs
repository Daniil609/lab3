
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
    public class service : : ServiceBase
    {
        Logger logger;
        public service()
        {
        InitializeComponent();
        this.CanStop = true;
        this.CanPauseAndContinue = true;
        this.AutoLog = true;
    
        }
    protected override void OnStart(string[] args)
    {
        logger = new Logger();
        try
        {
            Thread loggerThread = new Thread(new ThreadStart(logger.Start));
            loggerThread.Start();
        }
        catch (Exception)
        {
            using (StreamWriter sw = new StreamWriter(@"D:\csharp\log.txt", true, System.Text.Encoding.Default))
            {
                sw.WriteLine("Logger started with error,emergency mode is on");
            }
        }
    }

    protected override void OnStop()
    {
        logger.Stop();
        Thread.Sleep(1000);
    }
}
