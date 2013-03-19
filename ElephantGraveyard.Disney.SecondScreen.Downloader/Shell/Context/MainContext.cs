using System;
using System.Collections.Generic;
using Alba.Plist;
using ElephantGraveyard.Disney.SecondScreen.Downloader.Library.Events;
using ElephantGraveyard.Disney.SecondScreen.Downloader.Shell.Config;
using System.Linq;
using PlistConfig = System.Collections.Generic.IDictionary<string, object>;

// TODO Search for ".mxcsi", ".plist", ".png" etc. (regex?)
// ReSharper disable LoopCanBeConvertedToQuery
namespace ElephantGraveyard.Disney.SecondScreen.Downloader.Shell.Context
{
    internal abstract class MainContext
    {
        private readonly Lazy<PlistConfig> _eventListConfig;

        public ConfigBase Config { get; protected set; }

        protected MainContext ()
        {
            _eventListConfig = new Lazy<PlistConfig>(() => (PlistConfig)Plist.ReadFile(Constants.DataBaseDir + EventListConfigFile));
        }

        //
        // Views
        //

        // Original: com\mx_production\second_screen\shell\view\components\StartupView.as
        public virtual IEnumerable<string> GetStartupFiles ()
        {
            string dir = Config.StartupImageBase;
            yield return dir + "Default-Landscape.jpg";
            yield return dir + "second_splash_landscape.jpg";
            dir = Config.SecondScreenAssetBase;
            yield return dir + "intro_bk.jpg";
        }

        //
        // Models
        //

        // Original: com\mx_production\second_screen\shell\model\MainModel.as
        // Contains control paths.
        public virtual IEnumerable<string> GetMainModelInterfaceFiles ()
        {
            string dir = Config.SecondScreenAssetBase;
            yield return dir + "second_screen_fullsegment.mxcsi";
            yield return dir + "splash_segment.mxcsi";
            yield return dir + "help_segment.mxcsi";
            yield return dir + "infotext_segment.mxcsi";
            yield return dir + "preview_options_menu_segment.mxcsi";
            yield return dir + "options_segment.mxcsi";
            yield return dir + "start_preview_segment.mxcsi";
            yield return dir + "content_not_available_segment.mxcsi";
        }

        // Original: com\mx_production\second_screen\shell\model\UIInformationModel.as
        // Contains fonts, colors, sizes and other UI settings.
        // Original: com\mx_production\cub_second_screen\shell\model\EventListModel.as
        // Contains icons assignment for timeline.
        public virtual IEnumerable<string> GetUiInformationModelResourceFiles ()
        {
            string dir = Config.ResourcesAssetBase;
            yield return dir + "UIInformation.plist";
        }

        // Original: com\mx_production\second_screen\shell\model\EventListModel.as
        // Contains events parsing.
        public virtual IEnumerable<string> GetEventListModelResourceFiles ()
        {
            yield return EventListConfigFile;
        }

        public virtual IEnumerable<string> GetEventListModelIconFiles ()
        {
            string dir = Config.SecondScreenAssetBase;
            for (int i = 2; i <= CountIcons; i++)
                yield return dir + string.Format("{0}_icon.png", i);
            foreach (string typeIcon in TypeIcons)
                yield return dir + string.Format("{0}_icon.png", typeIcon);
        }

        public virtual string EventListConfigFile
        {
            get { return Config.ResourcesAssetBase + "EventList.plist"; }
        }

        public PlistConfig EventListConfig
        {
            get { return _eventListConfig.Value; }
        }

        public abstract int CountIcons { get; }

        public abstract IEnumerable<string> TypeIcons { get; }

        // Original: com\mx_production\second_screen\shell\model\TimelineModel.as
        // Contains control paths for templates and groups.
        public virtual IEnumerable<string> GetTimelineModelInterfaceFiles ()
        {
            string dir = Config.SecondScreenAssetBase;
            yield return dir + Config.TimelineInterfaceFile;
        }

        // Original: com\mx_production\second_screen\shell\model\SyncModel.as
        // Contains control paths.
        public virtual IEnumerable<string> GetSyncModelInterfaceFiles ()
        {
            string dir = Config.SecondScreenAssetBase;
            yield return dir + "locked_start_segment.mxcsi";
            yield return dir + "preview_sync_segment.mxcsi";
            yield return dir + "non_bd_live_sync_segment.mxcsi";
            yield return dir + "bd_sync_est_segment.mxcsi";
            yield return dir + "sync_choice_segment.mxcsi";
            yield return dir + "bd_live_searching_segment.mxcsi";
            yield return dir + "prev_sync_bdlive_err_segment.mxcsi";
            yield return dir + "preview_listening_segment.mxcsi";
            yield return dir + "prev_sync_audio_sync_err_segment.mxcsi";
            yield return dir + "counter_segment.mxcsi";
            yield return dir + "aud_sync_err_segment.mxcsi";
            yield return dir + "audio_sync_status_segment.mxcsi";
            yield return dir + "start_manual_sync_segment.mxcsi";
            yield return dir + "resync_menu_segment.mxcsi";
            yield return dir + "man_resync_pu_segment.mxcsi";
        }

        // Original: com\mx_production\second_screen\shell\model\IndexModel.as
        // Contains control paths.
        public virtual IEnumerable<string> GetIndexModelInterfaceFiles ()
        {
            string dir = Config.SecondScreenAssetBase;
            yield return dir + "index_segment.mxcsi";
            yield return dir + "chapter_index_segment.mxcsi";
            yield return dir + "activities_index_segment.mxcsi";
            yield return dir + "ticket_index_segment.mxcsi";
        }

        // Original: com\mx_production\second_screen\shell\model\CaptionModel.as
        // Contains captions parsing.
        // idToFile = caption + ".png"
        public virtual IEnumerable<string> GetCaptionModelResourceFiles ()
        {
            string dir = Config.ResourcesAssetBase;
            yield return dir + "captions.plist";
        }

        // Original: com\mx_production\second_screen\shell\model\IndexModel.as
        public virtual IEnumerable<string> GetAlertModelInterfaceFiles ()
        {
            string dir = Config.SecondScreenAssetBase;
            yield return dir + "unlock_success_segment.mxcsi";
        }

        // Original: com\mx_production\second_screen\shell\model\SocialModel.as
        // Contains control paths. Some controls are hidden after loading.
        public virtual IEnumerable<string> GetSocialModelInterfaceFiles ()
        {
            string dir = Config.SecondScreenAssetBase;
            yield return dir + "social_nudge_pu_segment.mxcsi";
            yield return dir + "social_options_segment.mxcsi";
            yield return dir + "status_updates_comment_segment.mxcsi";
            yield return dir + "ticket_earned_segment.mxcsi";
            yield return dir + "account_option_segment.mxcsi";
            yield return dir + "like_posted_segment.mxcsi";
            yield return dir + "status_updates_posted_segment.mxcsi";
            yield return dir + "ticket_earned_posted_segment.mxcsi";
            yield return dir + "fb_and_tw_post_error_segment.mxcsi";
            yield return dir + "fb_post_error_segment.mxcsi";
            yield return dir + "tw_post_error_segment.mxcsi";
        }

        // Original: com\mx_production\second_screen\shell\model\SocialStringsModel.as
        // Contains social strings parsing (regex: /^"(.*)" = "(.*)"$/).
        public virtual IEnumerable<string> GetSocialStringsModelTextFiles ()
        {
            string dir = Config.SocialBase;
            yield return dir + "social.strings";
        }

        // Original: com\mx_production\second_screen\shell\model\AwardModel.as
        // Contains control paths.
        public virtual IEnumerable<string> GetAwardModelInterfaceFiles ()
        {
            string dir = Config.SecondScreenAssetBase;
            yield return dir + "ticket_segment.mxcsi";
            yield return dir + "ticket_A_segment.mxcsi";
            yield return dir + "ticket_B_segment.mxcsi";
            yield return dir + "ticket_C_segment.mxcsi";
            yield return dir + "ticket_D_segment.mxcsi";
            yield return dir + "ticket_E_segment.mxcsi";
            yield return dir + "ticket_A_earned_msg_segment.mxcsi";
            yield return dir + "ticket_B_earned_msg_segment.mxcsi";
            yield return dir + "ticket_C_earned_msg_segment.mxcsi";
            yield return dir + "ticket_D_earned_msg_segment.mxcsi";
            yield return dir + "ticket_E_earned_msg_segment.mxcsi";
        }

        public virtual IEnumerable<string> GetAwardModelResourceFiles ()
        {
            string dir = Config.SocialBase;
            yield return dir + "awards.plist";
        }

        //
        // Modules
        //

        // Original: com\mx_production\cub_second_screen\shell\view\components\EventModule.as
        // Contains assets config for modules.
        public virtual IEnumerable<string> GetEventModuleFlashFiles ()
        {
            string dir = Config.ModuleBase;
            foreach (EventTypes value in AvailableEventTypes)
                yield return dir + string.Format("{0}.swf", value);
        }

        public virtual IEnumerable<EventTypes> AvailableEventTypes
        {
            get { return Enum.GetValues(typeof(EventTypes)).Cast<EventTypes>(); }
        }

        // Original: com\mx_production\second_screen\modules\video\config\Config.as
        // Contains coords and colors of controls.
        public virtual IEnumerable<string> GetVideoModuleResourceFiles ()
        {
            string dir = Config.InterfaceAssetBase;
            yield return dir + "modules/video/video_segment.xml";
        }
    }
}