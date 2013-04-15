using System.Windows;

namespace ElephantGraveyard.Disney.SecondScreen.Downloader.Shell.Config
{
    internal abstract class ConfigBase
    {
        public abstract string FlashName { get; }

        public abstract string AppId { get; }

        public abstract string AppName { get; }

        public virtual string DataBaseDir
        {
            get { return "data-" + FlashName + "/"; }
        }

        public virtual string ApplicationUrl
        {
            get { return "https://disneysecondscreen.go.com/"; }
        }

        public virtual Rect AppSize
        {
            get { return new Rect(0, 0, 1024, 768); }
        }

        public virtual string TimelineInterfaceFile
        {
            get { return "timeline_segment.mxcsi"; }
        }

        public virtual string AssetBase
        {
            get { return "assets/"; }
        }

        public virtual string ModuleBase
        {
            get { return "modules/"; }
        }

        public virtual string StartupImageBase
        {
            get { return AssetBase + "startup/"; }
        }

        public virtual string ResourcesAssetBase
        {
            get { return AssetBase + "resources/"; }
        }

        public virtual string SecondScreenAssetBase
        {
            get { return AssetBase + "second_screen/"; }
        }

        public virtual string InterfaceAssetBase
        {
            get { return AssetBase + "interface/"; }
        }

        public virtual string VideoBase
        {
            get { return AssetBase + "movies/"; }
        }

        public virtual string SocialBase
        {
            get { return AssetBase + "social/"; }
        }

        public virtual string FlipbookBase
        {
            get { return AssetBase + "flipbooks/"; }
        }
    }
}