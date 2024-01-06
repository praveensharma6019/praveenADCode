// Decompiled with JetBrains decompiler
// Type: Sitecore.BKRPL.Website.Models.AdaniRoadGallery
// Assembly: Sitecore.BKRPL.Website, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: A008C160-EE4E-4764-B38F-F5A8B3AD9B31
// Assembly location: C:\Users\PraveenSharma\To Decompile\Sitecore.BKRPL.Website.dll

using System;
using System.Collections.Generic;

namespace Sitecore.BKRPL.Website.Models
{
    [Serializable]
    public class AdaniRoadGallery
    {
        public string MonthYearName { get; set; }

        public List<RoadImages> AdaniRoadImages { get; set; }

        public AdaniRoadGallery() => this.AdaniRoadImages = new List<RoadImages>();
    }
}
