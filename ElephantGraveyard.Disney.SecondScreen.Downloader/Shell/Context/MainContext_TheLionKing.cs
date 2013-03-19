using System.Collections.Generic;
using ElephantGraveyard.Disney.SecondScreen.Downloader.Shell.Config;

namespace ElephantGraveyard.Disney.SecondScreen.Downloader.Shell.Context
{
    internal class MainContext_TheLionKing : MainContext
    {
        public MainContext_TheLionKing ()
        {
            Config = new Config_TheLionKing();
        }

        public override int CountIcons
        {
            get { return 7; }
        }

        public override IEnumerable<string> TypeIcons
        {
            get { return new[] { "activity", "flipbook", "video" }; }
        }
    }
}