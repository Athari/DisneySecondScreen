namespace ElephantGraveyard.Disney.SecondScreen.Downloader.Shell.Config
{
    internal class Config_JohnCarter : ConfigBase
    {
        public override string FlashName
        {
            get { return "JohnCarter"; }
        }

        public override string AppId
        {
            get { return "JohnCarter"; } //?
        }

        public override string AppName
        {
            get { return "John Carter"; }
        }

        public override string ApplicationUrl
        {
            get { return base.ApplicationUrl + "JohnCarter/"; }
        }
    }
}