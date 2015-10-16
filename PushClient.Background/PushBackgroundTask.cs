using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Networking.PushNotifications;
using Windows.UI.Notifications;

namespace PushClient.Background
{
    public sealed class PushBackgroundTask : IBackgroundTask
    {
        //private string serviceUrl = "http://localhost:48997/api/notifications/";

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            RawNotification raw = taskInstance.TriggerDetails as RawNotification;
            if (raw != null)
            {
                BackgroundTaskDeferral deferral = taskInstance.GetDeferral();

                //var notificationId = raw.Content;
                //HttpClient client = new HttpClient();
                //var notificationString = await client.GetStringAsync(new Uri(serviceUrl + notificationId));

                ShowToast(raw.Content);

                deferral.Complete();
            }
        }

        private void ShowToast(string notificationString)
        {
            ToastTemplateType toastTemplate = ToastTemplateType.ToastText01;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(notificationString));
            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
