using System;

namespace ElephantGraveyard.Disney.SecondScreen.Downloader.Shell.Config
{
    internal class Config_TheLionKing : ConfigBase
    {
        public override string AppId
        {
            get { return "The_Lion_King"; }
        }

        public override string AppName
        {
            get { return "The Lion King"; }
        }

        public override string ApplicationUrl
        {
            get { return base.ApplicationUrl + "TheLionKing/"; }
        }
    }
}