namespace ElephantGraveyard.Disney.SecondScreen.Downloader.Shell.Config
{
    internal class Config_LadyAndTheTramp : ConfigBase
    {
        public override string FlashName
        {
            get { return "Pound"; }
        }
        public override string AppId
        {
            get { return "Lady_And_The_Tramp"; } //?
        }

        public override string AppName
        {
            get { return "Lady and the Tramp"; }
        }

        public override string ApplicationUrl
        {
            get { return base.ApplicationUrl + "LadyAndTheTramp/"; }
        }
    }
}