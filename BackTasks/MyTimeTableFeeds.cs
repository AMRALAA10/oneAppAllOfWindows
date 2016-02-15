using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Windows.Web.Syndication;



namespace BackTasks
{
    public sealed class MyTimeTableFeeds : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral _deferral = taskInstance.GetDeferral();
            

            //background tasks operations here
            UpdateTile();
            _deferral.Complete();
            
        }

        public static void UpdateTile()
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();
            
            List<string> Feeds = new List<string>()
            {
                "Math", "physics", "programming"
            };
            
            foreach (var item in Feeds)
            {
                XmlDocument tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150Text01);
                tileXml.GetElementsByTagName("text")[0].InnerText = item;
                updater.Update(new TileNotification(tileXml));
            }


        }


        public async static void Register()
        {
            var backgroundAccessStatue = await BackgroundExecutionManager.RequestAccessAsync();
            if (backgroundAccessStatue == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity
                || backgroundAccessStatue == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
            {
                foreach (var Task in BackgroundTaskRegistration.AllTasks)
                {
                    if (Task.Value.Name == "MyTimeTableFeeds")
                        Task.Value.Unregister(true);
                }

                BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
                taskBuilder.Name = "MyTimeTableFeeds";
                taskBuilder.TaskEntryPoint = "BackTasks.MyTimeTableFeeds";
                taskBuilder.SetTrigger(new TimeTrigger(15, false));

                taskBuilder.Register();

            }
        }

    }
}
