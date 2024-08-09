using Colossal;
using Colossal.IO.AssetDatabase;
using Game.Modding;
using Game.Settings;
using System.Collections.Generic;

namespace IntersectableSubwaySystem
{
    [FileLocation($"ModsSettings\\StarQ\\{nameof(IntersectableSubwaySystem)}")]
    [SettingsUIGroupOrder(ActionGroup, InfoGroup)]
    [SettingsUIShowGroupName(InfoGroup)]
    public class Setting : ModSetting
    {
        public const string Tab = "Main";
        public const string ActionGroup = "Actions";
        public const string InfoGroup = "Info";

        public Setting(IMod mod) : base(mod)
        {
            SetDefaults();
        }

        [SettingsUIHidden]
        public bool ProcessedThumbnails { get; set; }


        [SettingsUIButton]
        [SettingsUISection(Tab, ActionGroup)]
        public bool RedoThumbnail
        {
            set { ThumbnailProcessor.ProcessThumbnail(); }
        }

        [SettingsUISection(Tab, InfoGroup)]
        public string NameText => Mod.Name;

        [SettingsUISection(Tab, InfoGroup)]
        public string VersionText => Mod.Version;

        [SettingsUISection(Tab, InfoGroup)]
        public string AuthorText => Mod.Author;

        public override void SetDefaults()
        {
            ProcessedThumbnails = false;
        }
    }

    public class LocaleEN(Setting setting) : IDictionarySource
    {
        public IEnumerable<KeyValuePair<string, string>> ReadEntries(IList<IDictionaryEntryError> errors, Dictionary<string, int> indexCounts)
        {
            return new Dictionary<string, string>
            {
                { setting.GetSettingsLocaleID(), Mod.Name },
                { setting.GetOptionTabLocaleID(Setting.Tab), Setting.Tab },
                { setting.GetOptionGroupLocaleID(Setting.ActionGroup), Setting.ActionGroup },
                { setting.GetOptionGroupLocaleID(Setting.InfoGroup), Setting.InfoGroup },

                { setting.GetOptionLabelLocaleID(nameof(Setting.RedoThumbnail)), "Reset Thumbnail Cache [powered by Asset Icon Library]" },
                { setting.GetOptionDescLocaleID(nameof(Setting.RedoThumbnail)), $"This button will recreate the thumbnails if necessary. The thumbnails are loaded by Asset Icon Library. Make sure you're already subscribed to it to have the thumbnails." },

                { setting.GetOptionLabelLocaleID(nameof(Setting.NameText)), "Mod Name" },
                { setting.GetOptionDescLocaleID(nameof(Setting.NameText)), "" },
                { setting.GetOptionLabelLocaleID(nameof(Setting.VersionText)), "Mod Version" },
                { setting.GetOptionDescLocaleID(nameof(Setting.VersionText)), "" },
                { setting.GetOptionLabelLocaleID(nameof(Setting.AuthorText)), "Author" },
                { setting.GetOptionDescLocaleID(nameof(Setting.AuthorText)), "" },

            };
        }

        public void Unload()
        {

        }
    }
}
