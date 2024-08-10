using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Game;
using Game.Modding;
using Game.SceneFlow;
using Game.UI.Menu;
using Unity.Entities;

namespace IntersectableSubwaySystem
{
    public class Mod : IMod
    {
        public static string Name = "Intersectable Subway System";
        public static string Version = "1.0.1";
        public static string Author = "StarQ";
        //public static ILog log = LogManager.GetLogger($"{nameof(PropVanillaVegetations)}.{nameof(Mod)}").SetShowsErrorsInUI(false);

        public static Setting Setting;
        public static NotificationUISystem notificationUISystem;
        public static ModManager modManager = GameManager.instance.modManager;
        public ThumbnailProcessor thumbnailProcessor;

        public void OnLoad(UpdateSystem updateSystem)
        {
            notificationUISystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<NotificationUISystem>();

            Setting = new Setting(this);
            Setting.RegisterInOptionsUI();
            GameManager.instance.localizationManager.AddSource("en-US", new LocaleEN(Setting));
            AssetDatabase.global.LoadSettings(nameof(IntersectableSubwaySystem), Setting, new Setting(this));

            thumbnailProcessor = new ThumbnailProcessor(this);
            World.DefaultGameObjectInjectionWorld.AddSystemManaged(thumbnailProcessor);
        }

        public void OnDispose()
        {
            if (Setting != null)
            {
                Setting.UnregisterInOptionsUI();
                Setting = null;
            }
        }
    }
}