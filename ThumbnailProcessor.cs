using Colossal.PSI.Common;
using Colossal.PSI.Environment;
using Game;
using Game.Modding;
using Game.UI.Menu;
using System.IO;
using System.Linq;

namespace IntersectableSubwaySystem
{
    public partial class ThumbnailProcessor(Mod mod) : GameSystemBase
    {
        public Mod _mod = mod;
        public static Setting Setting = Mod.Setting;
        public static NotificationUISystem notificationUISystem = Mod.notificationUISystem;
        public static ModManager modManager = Mod.modManager;
        public static string notificationName = "starq-intersectable-subway-system-thumbnail-process";

        protected override void OnCreate()
        {
            base.OnCreate();
            if (!Setting.ProcessedThumbnails)
            {
                ProcessThumbnail();
            }
        }

        protected override void OnUpdate()
        {
        }

        public static void ProcessThumbnail()
        {
            string ailCustomFolder = EnvPath.kUserDataPath + "/ModsData/AssetIconLibrary/CustomThumbnails";
            bool ailFound = false;

            var thumbNotification = notificationUISystem.AddOrUpdateNotification(notificationName,
                title: $"Processing Thumbnails ({Mod.Name})",
            text: "Starting system...",
                progressState: ProgressState.Indeterminate,
                progress: 0);

            foreach (var modInfo in modManager)
            {
                string modName = modInfo.asset.name;
                if (modName.StartsWith("AssetIconLibrary"))
                {
                    ailFound = true;
                    break;

                }
            }

            if (ailFound)
            {
                thumbNotification.text = "Asset Icon Library found...";
                thumbNotification.progressState = ProgressState.Progressing;
                thumbNotification.progress = (2 / 4 * 100);
                if (!CheckExistingFiles(ailCustomFolder, thumbNotification))
                {
                    FindAndCopyFiles(ailCustomFolder, thumbNotification);
                }
            }
            else
            {
                thumbNotification.text = "Asset Icon Library not found, cancelling...";
                thumbNotification.progressState = ProgressState.Complete;
                thumbNotification.progress = (4 / 4 * 100);
                notificationUISystem.RemoveNotification(notificationName, delay: 10f);
            }
        }

        protected static bool CheckExistingFiles(string ailCustomFolder, NotificationUISystem.NotificationInfo thumbNotification = null)
        {
            string[] files =
            {
                "StarQ IntersectableSubway 1W 1T.svg",
                "StarQ IntersectableSubway 1W 2T.svg",
                "StarQ IntersectableSubway 2W 1T.svg",
                "StarQ IntersectableSubway 2W 2T.svg",
                "StarQ IntersectableSubway Menu.svg",
                "StarQ IntersectableSubway SubwayStationElevated01.png",
                "StarQ IntersectableSubway SubwayStationElevated02.png",
            };

            Directory.CreateDirectory(ailCustomFolder);
            string[] existingFiles = Directory.GetFiles(ailCustomFolder, "*.*")
                             .Where(f => f.Contains("StarQ IntersectableSubway"))
                             .ToArray(); ;
            int count = 0;

            if (existingFiles.Length > 0)
            {
                foreach (var file in files)
                {
                    if (existingFiles.Contains(ailCustomFolder + "\\" + file))
                    {
                        count++;
                    }
                }
            }

            if (thumbNotification != null && count == files.Length)
            {
                thumbNotification.text = "Thumbnails already exists; cancelling...";
                thumbNotification.progressState = ProgressState.Complete;
                thumbNotification.progress = (4 / 4 * 100);
                Setting.ProcessedThumbnails = true;
                notificationUISystem.RemoveNotification(notificationName, delay: 10f);
            }
            return count == files.Length;
        }

        protected static void FindAndCopyFiles(string ailCustomFolder, NotificationUISystem.NotificationInfo thumbNotification)
        {
            string assetPath = null;
            foreach (var modInfo in modManager)
            {
                if (modInfo.asset.path.Contains($"/{nameof(IntersectableSubwaySystem)}.dll"))
                {
                    assetPath = modInfo.asset.path.Replace($"/{nameof(IntersectableSubwaySystem)}.dll", "");
                }
                else if (modInfo.asset.path.Contains($"\\{nameof(IntersectableSubwaySystem)}.dll"))
                {
                    assetPath = modInfo.asset.path.Replace($"\\{nameof(IntersectableSubwaySystem)}.dll", "");
                }
            }

            if (assetPath != null)
            {
                string thumbdirectory = assetPath + "\\thumbs";
                string[] files = Directory.GetFiles(thumbdirectory, "*.*");
                int count = 0;
                foreach (var file in files)
                {
                    count++;
                    File.Copy(file, ailCustomFolder + "\\" + file.Substring(file.LastIndexOf("\\")), true);
                    thumbNotification.text = $"Configuring thumbnails...";
                    thumbNotification.progressState = ProgressState.Progressing;
                    thumbNotification.progress = 50 + (25 * count/file.Length);
                }
            }
            if (CheckExistingFiles(ailCustomFolder))
            {
                thumbNotification.text = "Thumbnail processing completed...";
                thumbNotification.progressState = ProgressState.Complete;
                thumbNotification.progress = (4 / 4 * 100);
                Setting.ProcessedThumbnails = true;
                notificationUISystem.RemoveNotification(notificationName, delay: 10f);
            }
            else
            {
                thumbNotification.text = "Thumbnail processing failed...";
                thumbNotification.progressState = ProgressState.Failed;
                thumbNotification.progress = (4 / 4 * 100);
                Setting.ProcessedThumbnails = false;
                notificationUISystem.RemoveNotification(notificationName, delay: 10f);
            }
        }
    }
}
