using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using Alba.Framework.Attributes;
using Alba.Framework.IO;
using ElephantGraveyard.Disney.SecondScreen.Downloader.Shell.Context;
using System.Linq;
using Alba.Framework.Sys;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable MethodOverloadWithOptionalParameter
namespace ElephantGraveyard.Disney.SecondScreen.Downloader
{
    internal class Program
    {
        private const string DataBaseDir = "data/";
        private const int DownloadSuccessDelay = 200;
        private const int DownloadErrorDelay = 1000;

        private readonly WebClient _web = new WebClient();
        private readonly Stream _logStream;
        private readonly TextWriter _logWriter;
        private MainContext _context;

        public Program ()
        {
            _logStream = new TeeOutputStream(File.Open("Log.txt", FileMode.Append), Console.OpenStandardOutput());
            _logWriter = new StreamWriter(_logStream) { AutoFlush = true };
        }

        private static void Main (string[] args)
        {
            new Program().MainInternal(args);
        }

        private void MainInternal (string[] args)
        {
            _context = new MainContext_TheLionKing();

            Log(new string('=', 80));
            Log("Started on {0}", DateTime.Now);
            Log(new string('=', 80));
            try {
                DownloadMainFiles();
                DownloadInterfaceAssets();
                Log();
                Log("DONE!");
            }
            catch (Exception e) {
                Log("Oops I did it again! {0}", e);
            }
            finally {
                _logWriter.Flush();
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }

        private void DownloadMainFiles ()
        {
            Log();
            Log("I. Downloading main files");
            Log();
            var files = _context.GetAlertModelInterfaceFiles()
                .Concat(_context.GetAwardModelInterfaceFiles())
                .Concat(_context.GetAwardModelResourceFiles())
                .Concat(_context.GetCaptionModelResourceFiles())
                .Concat(_context.GetEventListModelIconFiles())
                .Concat(_context.GetEventListModelResourceFiles())
                .Concat(_context.GetIndexModelInterfaceFiles())
                .Concat(_context.GetMainModelInterfaceFiles())
                .Concat(_context.GetSocialModelInterfaceFiles())
                .Concat(_context.GetSocialStringsModelTextFiles())
                .Concat(_context.GetSyncModelInterfaceFiles())
                .Concat(_context.GetTimelineModelInterfaceFiles())
                .Concat(_context.GetUiInformationModelResourceFiles());
            Download(files);
            Log();
            Log("I. Downloading main files - DONE");
        }

        private void DownloadInterfaceAssets ()
        {
            Log();
            Log("II. Downloading interface assets");
            Log();
            Log();
            Log("II. Downloading interface assets - DONE");
        }

        private void Download (IEnumerable<string> files)
        {
            var filesList = files.ToList();
            for (int i = 0; i < filesList.Count; i++) {
                Log("File {0} of {1}", i + 1, filesList.Count);
                Download(filesList[i]);
            }
        }

        private void Download (string file)
        {
            string url = _context.Config.ApplicationUrl + file;
            string path = DataBaseDir + file;

            if (File.Exists(path)) {
                Log("  Skipping {0}", url);
            }
            else {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                Log("  Downloading {0}\n           to {1}", url, path);
                try {
                    _web.DownloadFile(url, path);
                    Thread.Sleep(DownloadSuccessDelay);
                }
                catch (WebException e) {
                    Log("  Download failed: {0}", e.GetFullMessage());
                    Thread.Sleep(DownloadErrorDelay);
                }
            }
        }

        [StringFormatMethod ("format")]
        private void Log (string format, params object[] arg)
        {
            _logWriter.WriteLine(format, arg);
        }

        private void Log (string value = "")
        {
            _logWriter.WriteLine(value);
        }
    }
}