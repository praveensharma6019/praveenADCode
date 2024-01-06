using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Sitecore.Feature.Media.Repositories
{
    public interface IMediaRepository
    {
        IEnumerable<Item> Get(Item contextItem);
        IEnumerable<Item> GetLatest(Item contextItem, int count);
    }
}