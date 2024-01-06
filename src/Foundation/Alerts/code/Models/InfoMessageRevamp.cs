using System;

namespace Sitecore.Foundation.Alerts.Models
{
    [Serializable]
    public class InfoMessageRevamp
    {
        public InfoMessageRevamp()
        {
        }

        //public InfoMessageRevamp(string heading, string message, bool isformsubmission) : this(heading, message, MessageTypeRevamp.Info, isformsubmission)
        //{
        //}

        public InfoMessageRevamp(string heading, string message, MessageTypeRevamp messageType, bool isformsubmission)
        {
            if (string.IsNullOrEmpty(heading))
            {
                if (messageType == MessageTypeRevamp.Success)
                {
                    this.Heading = "Success";
                }
                else if (messageType == MessageTypeRevamp.Info)
                {
                    this.Heading = "Information";
                }
                else if (messageType == MessageTypeRevamp.Error)
                {
                    this.Heading = "Error";
                }
                else if (messageType == MessageTypeRevamp.Warning)
                {
                    this.Heading = "Warning";
                }
            }
            else
            {
                this.Heading = heading;
            }
            this.Message = message;
            this.Type = messageType;
            this.isFormSubmission = isformsubmission;
        }

        public string Heading { get; set; }
        public string Message { get; set; }
        public bool isFormSubmission { get; set; }

        public MessageTypeRevamp Type { get; set; }

        public enum MessageTypeRevamp
        {
            Info,
            Success,
            Warning,
            Error
        }

        public static InfoMessageRevamp Error(string heading, string message, bool isformsubmission) => new InfoMessageRevamp(heading, message, MessageTypeRevamp.Error, isformsubmission);
        public static InfoMessageRevamp Warning(string heading, string message, bool isformsubmission) => new InfoMessageRevamp(heading, message, MessageTypeRevamp.Warning, isformsubmission);
        public static InfoMessageRevamp Success(string heading, string message, bool isformsubmission) => new InfoMessageRevamp(heading, message, MessageTypeRevamp.Success, isformsubmission);
        public static InfoMessageRevamp Info(string heading, string message, bool isformsubmission) => new InfoMessageRevamp(heading, message, MessageTypeRevamp.Info, isformsubmission);
    }
}