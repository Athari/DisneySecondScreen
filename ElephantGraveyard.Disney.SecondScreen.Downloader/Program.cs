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
                DownloadMainData();
                DownloadEventData();
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

        private void DownloadMainData ()
        {
            Log();
            Log("* Downloading main data");
            Log();
            var files = _context.GetAlertModelInterfaceFiles()
                .Concat(_context.GetAwardModelInterfaceFiles())
                .Concat(_context.GetAwardModelResourceFiles())
                .Concat(_context.GetCaptionModelResourceFiles())
                .Concat(_context.GetDeletedSceneModuleInterfaceFiles())
                .Concat(_context.GetEventListModelIconFiles())
                .Concat(_context.GetEventListModelResourceFiles())
                .Concat(_context.GetEventModuleFlashFiles())
                .Concat(_context.GetFlipbookModuleInterfaceFiles())
                .Concat(_context.GetGalleryModuleInterfaceFiles())
                .Concat(_context.GetIndexModelInterfaceFiles())
                .Concat(_context.GetInkAndPaintModuleInterfaceFiles())
                .Concat(_context.GetMainModelInterfaceFiles())
                .Concat(_context.GetSceneScramblerModuleInterfaceFiles())
                .Concat(_context.GetSocialModelInterfaceFiles())
                .Concat(_context.GetSocialStringsModelTextFiles())
                .Concat(_context.GetStartupFiles())
                .Concat(_context.GetSyncModelInterfaceFiles())
                .Concat(_context.GetTimelineModelInterfaceFiles())
                .Concat(_context.GetTriviaModuleInterfaceFiles())
                .Concat(_context.GetUiInformationModelResourceFiles())
                .Concat(_context.GetVideoModuleInterfaceFiles());
            Download(files);
            Log();
            Log("* Downloading main data - DONE");
        }

        private void DownloadEventData ()
        {
            Log();
            Log("* Downloading event interfaces");
            Log();
            string dir = _context.Config.SecondScreenAssetBase;
            var files = new List<string>();
            dynamic eventListConfig = _context.EventListConfig;
            foreach (dynamic evt in eventListConfig["events"]) {
                files.Add(dir + (string)evt["interfaceFile"] + ".mxcsi");
                // deleted:
                //   *_deleted_scene_segment
                //   *_deleted_scene_storyboard_segment
                // flipbook:
                //   *_flipbook
                //   *_FB_*.swf (?) - FlipbookView
                // gallery:
                //   galleryInfoButtons:
                //     *..._info_segment
                // inkandpaint:
                //   *_inkpaint_start_segment
                //   *_inkpaint_segment
                //   *_ink_segment
                //   *_paint_segment
                //   inkpaint_save_fullsegment
                //   inkpaint_saveover_fullsegment
                //   inkpaint_load_fullsegment
                // scenescrambler:
                //   *_scenescrambler_segment
                //   well_done_segment
                // trivia:
                //   -
                // video:
                //   *_video_segment
                //   *_video_start_segment (HasSeparateStartSegment=false?)
                //   *.mov
            }
            Download(files);
            Log();
            Log("* Downloading event interfaces - DONE");
        }

        private void DownloadInterfaceAssets ()
        {
            Log();
            Log("* Downloading interface assets");
            Log();
            /*var processedFiles = new List<string>();
            while (true) {
                var files = Directory.EnumerateFiles(DataBaseDir, "*.mxcsi", SearchOption.AllDirectories).Except(processedFiles).ToList();
                if (!files.Any())
                    break;
                foreach (string file in files)
                    DownloadInterfaceFileAssets(file);
                processedFiles.AddRange(files);
            }*/
            Log();
            Log("* Downloading interface assets - DONE");
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
            string path = Constants.DataBaseDir + file;

            if (File.Exists(path)) {
                Log("  Skipping {0}", file);
            }
            else {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                Log("  Downloading {0}", file);
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