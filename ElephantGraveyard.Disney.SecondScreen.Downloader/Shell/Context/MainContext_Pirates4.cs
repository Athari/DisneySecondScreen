using System.Collections.Generic;
using ElephantGraveyard.Disney.SecondScreen.Downloader.Library.Events;
using ElephantGraveyard.Disney.SecondScreen.Downloader.Shell.Config;

namespace ElephantGraveyard.Disney.SecondScreen.Downloader.Shell.Context
{
    internal class MainContext_Pirates4 : MainContext
    {
        public MainContext_Pirates4 ()
        {
            Config = new Config_Pirates4();
        }

        public override int CountIcons
        {
            get { return 7; }
        }

        public override IEnumerable<string> TypeIcons
        {
            get { return new[] { "activity", "flipbook", "video" }; }
        }

        public override IEnumerable<EventTypes> AvailableEventTypes
        {
            get
            {
                return new[] {
                    EventTypes.DeletedScene, EventTypes.Flipbook, EventTypes.Gallery, EventTypes.InkAndPaint,
                    EventTypes.SceneScrambler, EventTypes.Trivia, EventTypes.Video,
                };
            }
        }
    }
}