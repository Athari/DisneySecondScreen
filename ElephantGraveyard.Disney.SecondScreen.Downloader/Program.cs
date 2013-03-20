using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using Alba.Framework.Attributes;
using Alba.Framework.IO;
using ElephantGraveyard.Disney.SecondScreen.Downloader.Library.Events;
using ElephantGraveyard.Disney.SecondScreen.Downloader.Library.Parser;
using ElephantGraveyard.Disney.SecondScreen.Downloader.Library.Ui;
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
        private readonly List<string> _failedDownloads = new List<string>();
        private readonly Stream _logStream;
        private readonly TextWriter _logWriter;
        private MainContext _context;
        private int _successfulDownloadsNum;

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
            try {
                string contextType = args.Length > 0 ? args[0] : "TheLionKing";
                switch (contextType) {
                    case "TheLionKing":
                        _context = new MainContext_TheLionKing();
                        break;
                    default:
                        throw new InvalidOperationException("Unsupported context type.");
                }

                Log(new string('=', 80));
                Log("Started on {0}", DateTime.Now);
                Log(new string('=', 80));

                DownloadMainData();
                DownloadEventData();
                DownloadInterfaceAssets();

                if (_failedDownloads.Any()) {
                    Log();
                    Log("Failed downloads:");
                    foreach (string failedDownload in _failedDownloads)
                        Log("  - {0}", failedDownload);
                }

                Log();
                Log("Successfully downloaded {0} file(s)", _successfulDownloadsNum);
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
                .Concat(_context.GetEventListModelIconFiles())
                .Concat(_context.GetEventListModelResourceFiles())
                .Concat(_context.GetEventModuleFlashFiles())
                .Concat(_context.GetIndexModelInterfaceFiles())
                .Concat(_context.GetInkAndPaintModelInterfaceFiles())
                .Concat(_context.GetMainModelInterfaceFiles())
                .Concat(_context.GetSceneScramblerModelInterfaceFiles())
                .Concat(_context.GetSocialModelInterfaceFiles())
                .Concat(_context.GetSocialStringsModelTextFiles())
                .Concat(_context.GetStartupFiles())
                .Concat(_context.GetSyncModelInterfaceFiles())
                .Concat(_context.GetTimelineModelInterfaceFiles())
                .Concat(_context.GetUiInformationModelResourceFiles())
                .Concat(_context.GetVideoModuleResourceFiles());
            Download(files);
            Log();
            Log("* Downloading main data - DONE");
        }

        private void DownloadEventData ()
        {
            Log();
            Log("* Downloading event interfaces");
            Log();
            string dirSS = _context.Config.SecondScreenAssetBase;
            string dirVid = _context.Config.VideoBase;
            var files = new List<string>();
            dynamic eventListConfig = _context.EventListConfig;
            foreach (dynamic evt in eventListConfig["events"]) {
                files.Add(dirSS + (string)evt["interfaceFile"] + ".mxcsi");
                files.Add(dirSS + (string)evt["thumb"]);
                var eventId = (string)evt["eventID"];
                switch ((EventTypes)Enum.Parse(typeof(EventTypes), (string)evt["type"])) {
                    case EventTypes.Gallery:
                        break;
                    case EventTypes.Trivia:
                        break;
                    case EventTypes.Generic:
                        break;
                    case EventTypes.Video:
                        files.Add(dirVid + eventId + ".mov");
                        files.Add(dirSS + eventId + "_video_segment.mxcsi");
                        //files.Add(dirSS + eventId + "_video_start_segment.mxcsi"); // HasSeparateStartSegment is always false
                        break;
                    case EventTypes.DeletedScene:
                        files.Add(dirSS + eventId + "_deleted_scene_segment.mxcsi");
                        files.Add(dirSS + eventId + "_deleted_scene_storyboard_segment.mxcsi");
                        break;
                    case EventTypes.Flipbook:
                        files.Add(dirSS + eventId + "_flipbook.mxcsi");
                        break;
                    case EventTypes.SceneScrambler:
                        files.Add(dirSS + eventId + "_scenescrambler_segment.mxcsi");
                        break;
                    case EventTypes.WhatsDifferent:
                        break;
                    case EventTypes.InkAndPaint:
                        files.Add(dirSS + eventId + "_inkpaint_start_segment.mxcsi");
                        files.Add(dirSS + eventId + "_inkpaint_segment.mxcsi");
                        files.Add(dirSS + eventId + "_ink_segment.mxcsi");
                        files.Add(dirSS + eventId + "_paint_segment.mxcsi");
                        break;
                    case EventTypes.FlipbookType2:
                        break;
                    case EventTypes.Application:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                // flipbook:
                //   *_FB_*.swf (?) - FlipbookView
                // gallery:
                //   galleryInfoButtons:
                //     *..._info_segment
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
            var files = new HashSet<string>();
            foreach (string uifile in Directory.EnumerateFiles(Constants.DataBaseDir, "*.mxcsi", SearchOption.AllDirectories))
                DownloadAssets(files, MxCsiParser.parse(uifile));
            Download(files);
            Log();
            Log("* Downloading interface assets - DONE");
        }

        private void DownloadAssets (ICollection<string> files, MXUIView view)
        {
            view.buttons.ForEach(o => DownloadAssets(files, o));
            view.layers.ForEach(o => DownloadAssets(files, o));
            view.sliders.ForEach(o => DownloadAssets(files, o));
            view.subviews.ForEach(o => DownloadAssets(files, o));
            var gallery = view as MXUIGallery;
            if (gallery != null) {
                // TODO
            }
            var flipbook = view as MXUIFlipbook;
            if (flipbook != null) {
                // TODO
            }
            var video = view as MXUIVideo;
            if (video != null) {
                // TODO
            }
        }

        private void DownloadAssets (ICollection<string> files, MXUIButton button)
        {
            button.imageStates.Values.ForEach(o => DownloadAssets(files, o));
        }

        private void DownloadAssets (ICollection<string> files, MXUILayer layer)
        {
            DownloadAssets(files, layer.contents);
        }

        private void DownloadAssets (ICollection<string> files, MXUISlider slider)
        {
            slider.thumbStates.Values.ForEach(o => DownloadAssets(files, o));
            slider.trackStates.Values.ForEach(o => DownloadAssets(files, o));
        }

        private void DownloadAssets (ICollection<string> files, MXUIImage image)
        {
            string dir = _context.Config.SecondScreenAssetBase;
            files.Add(dir + image.file);
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
                    _successfulDownloadsNum++;
                    Thread.Sleep(DownloadSuccessDelay);
                }
                catch (WebException e) {
                    Log("  Download failed: {0}", e.GetFullMessage());
                    _failedDownloads.Add(file);
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