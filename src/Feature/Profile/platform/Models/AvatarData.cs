using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Avatar.Models
{
    public class AvatarData
    {
        [JsonProperty(PropertyName = "avatarID")]
        public string avatarId { get; set; }

        [JsonProperty(PropertyName = "avatarInclude")]
        public bool isAvatarInclude { get; set; }

        [JsonProperty(PropertyName = "avatarImage")]
        public string avatarImagePath { get; set; }
    }
}