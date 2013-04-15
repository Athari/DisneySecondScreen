namespace ElephantGraveyard.Disney.SecondScreen.Downloader.Shell.Config
{
    internal class Config_Cinderella : ConfigBase
    {
        public override string FlashName
        {
            get { return "Cinderella"; }
        }

        public override string AppId
        {
            get { return "Cinderella"; } //?
        }

        public override string AppName
        {
            get { return "Cinderella"; }
        }

        public override string ApplicationUrl
        {
            get { return base.ApplicationUrl + "Cinderella/"; }
        }
    }
}