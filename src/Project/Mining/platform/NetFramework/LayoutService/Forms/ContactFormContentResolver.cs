﻿using Project.Mining.Website.Services.Forms;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;

namespace Project.Mining.Website.LayoutService.Forms
{
    public class ContactFormContentResolver : RenderingContentsResolver
    {
        private readonly IMiningFormsService _miningFormsService;

        public ContactFormContentResolver(IMiningFormsService miningFormsService)
        {
            _miningFormsService = miningFormsService;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            return _miningFormsService.GetContactForm(rendering);
        }
    }
}