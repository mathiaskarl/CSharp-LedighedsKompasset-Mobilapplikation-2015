using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using LedighedsApp.Model.DataModel;

namespace LedighedsApp.Model.Handler
{
    class ToastService
    {
        public ToastService()
        {
            //Empty constructor included for brewity
        }

        public static void SetToastForNotification(Notification notification)
        {
            string text = notification.Text; //TODO: Hent teksten fra databasen via textId

            var toastTemplate = ToastTemplateType.ToastText01;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");

            toastTextElements[0].AppendChild(toastXml.CreateTextNode(text));
            var scheduledToast = new ScheduledToastNotification(toastXml, notification.TriggerTime)
            {
                Id = notification.Id.ToString()
            };

            ToastNotificationManager.CreateToastNotifier().AddToSchedule(scheduledToast);
        }

        public static void RemoveToastFromSchedule(Notification notification)
        {
            string toastId = notification.Id.ToString();
            
            ScheduledToastNotification toastToBeRemoved = ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications().FirstOrDefault(x => x.Id == toastId);

            if (toastToBeRemoved != null)
            {
                ToastNotificationManager.CreateToastNotifier().RemoveFromSchedule(toastToBeRemoved);
            }
        }

        //Debug methods
        public static List<ScheduledToastNotification> GetAllScheduledToasts()
        {
            return ToastNotificationManager.CreateToastNotifier().GetScheduledToastNotifications().ToList();
        }
    }
}
