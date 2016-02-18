using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;
using Windows.UI.StartScreen;
using Windows.Data.Xml.Dom;
using Windows.ApplicationModel.Background;

namespace BackTasks
{
    public sealed class UpdateSeconderyTiles : IBackgroundTask
    {
        private void Update()
        {
            XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Text04);
            XmlNodeList tileTextAttributes = tileXml.GetElementsByTagName("text");

            tileTextAttributes[0].InnerText = "thissssss is awesooooooooooooooooooooooooome";

            XmlDocument squreTileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Text04);
            XmlNodeList squareTileTextAttributes = squreTileXml.GetElementsByTagName("text");
            squareTileTextAttributes[0].AppendChild(squreTileXml.CreateTextNode("this is alsooo wesoooooome"));

            IXmlNode node = tileXml.ImportNode(squreTileXml.GetElementsByTagName("binding").Item(0), true);
            tileXml.GetElementsByTagName("visual").Item(0).AppendChild(node);

            TileNotification tileNotification = new TileNotification(tileXml);
            TileUpdater secTileUpdater = TileUpdateManager.CreateTileUpdaterForSecondaryTile("Amr");

            secTileUpdater.Update(tileNotification);

        }

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var _deferral = taskInstance.GetDeferral();

            Update();

            _deferral.Complete();
        }

        public async static void Register()
        {
            var backgroundAccessStatue = await BackgroundExecutionManager.RequestAccessAsync();
            if (backgroundAccessStatue == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity ||
                backgroundAccessStatue == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
            {
                foreach (var Task in BackgroundTaskRegistration.AllTasks)
                {
                    if (Task.Value.Name == "UpdateSeconderyTiles")
                        Task.Value.Unregister(true);
                }

                BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
                taskBuilder.Name = "UpdateSeconderyTiles";
                taskBuilder.TaskEntryPoint = "BackTasks.UpdateSeconderyTiles";
                taskBuilder.SetTrigger(new TimeTrigger(15, false));
                taskBuilder.Register();

            }
        }


    }
}
