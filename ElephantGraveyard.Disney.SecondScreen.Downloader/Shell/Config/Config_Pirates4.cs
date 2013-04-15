namespace ElephantGraveyard.Disney.SecondScreen.Downloader.Shell.Config
{
    internal class Config_Pirates4 : ConfigBase
    {
        public override string FlashName
        {
            get { return "Rummy"; }
        }

        public override string AppId
        {
            get { return "Rummy"; } //?
        }

        public override string AppName
        {
            get { return "Pirates of the Caribbean: On Stranger Tides"; }
        }

        public override string ApplicationUrl
        {
            get { return base.ApplicationUrl + "PiratesOfTheCaribbeanOnStrangerTides/"; }
        }
    }
}