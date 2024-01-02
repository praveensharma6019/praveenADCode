using Sitecore.LayoutService.Configuration;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Feature.Carousel.Platform.Services;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.LayoutService
{
    public class NotificationResolver : Sitecore.LayoutService.ItemRendering.ContentsResolvers.RenderingContentsResolver
    {
        private readonly INotificationService _notification;
        public NotificationResolver(INotificationService notification)
        {
            this._notification = notification;
        }
        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _notification.GetNotificationData(rendering);
        }
    }
}