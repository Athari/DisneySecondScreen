namespace ElephantGraveyard.Disney.SecondScreen.Downloader.Shell.Config
{
    internal class Config_Bambi : ConfigBase
    {
        public override string FlashName
        {
            get { return "FOSS_app"; }
        }
        public override string AppId
        {
            get { return "Bambi"; } //?
        }

        public override string AppName
        {
            get { return "Bambi"; }
        }

        public override string ApplicationUrl
        {
            get { return base.ApplicationUrl + "support/"; }
        }
    }
}