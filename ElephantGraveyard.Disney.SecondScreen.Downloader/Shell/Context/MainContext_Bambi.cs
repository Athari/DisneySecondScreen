using System.Collections.Generic;
using ElephantGraveyard.Disney.SecondScreen.Downloader.Library.Events;
using ElephantGraveyard.Disney.SecondScreen.Downloader.Shell.Config;

namespace ElephantGraveyard.Disney.SecondScreen.Downloader.Shell.Context
{
    internal class MainContext_Bambi : MainContext
    {
        public MainContext_Bambi ()
        {
            Config = new Config_Bambi();
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