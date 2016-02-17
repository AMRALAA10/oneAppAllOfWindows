using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Networking.PushNotifications;
using Windows.ApplicationModel.Background;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

namespace BackTasks
{
    public sealed class NotifyUser : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            var _defrral = taskInstance.GetDeferral();

            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText03);
            XmlNodeList stringElements = toastXml.GetElementsByTagName("text");

            stringElements[0].AppendChild(toastXml.CreateTextNode("AMR ALAA"));
            stringElements[1].AppendChild(toastXml.CreateTextNode("I MADE IT"));

            ToastNotification toast = new ToastNotification(toastXml);

            ToastNotificationManager.CreateToastNotifier().Show(toast);

            _defrral.Complete();
        }

        public async static void Register()
        {
            var backgroundAccessStatue = await BackgroundExecutionManager.RequestAccessAsync();
            if (backgroundAccessStatue == BackgroundAccessStatus.AllowedMayUseActiveRealTimeConnectivity ||
                backgroundAccessStatue == BackgroundAccessStatus.AllowedWithAlwaysOnRealTimeConnectivity)
            {
                foreach (var Task in BackgroundTaskRegistration.AllTasks)
                {
                    if (Task.Value.Name == "NotifyUser")
                        Task.Value.Unregister(true);
                }

                BackgroundTaskBuilder taskBuilder = new BackgroundTaskBuilder();
                taskBuilder.Name = "NotifyUser";
                taskBuilder.TaskEntryPoint = "BackTasks.NotifyUser";
                taskBuilder.SetTrigger(new TimeTrigger(15, false));
                taskBuilder.Register();
                
            }
        }
    }
}
