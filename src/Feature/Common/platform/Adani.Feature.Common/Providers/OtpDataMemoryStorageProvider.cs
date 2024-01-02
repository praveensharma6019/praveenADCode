using Adani.Feature.Common.Data;
using Adani.Feature.Common.Models;
using System.Runtime.Caching;

namespace Adani.Feature.Common.Providers
{
    public class OtpDataMemoryStorageProvider : IOtpDataStorageProvider
    {
        private static MemoryCache _memoryCache;

        public OtpDataMemoryStorageProvider()
        {
            if (_memoryCache == null)
                _memoryCache = new MemoryCache("OtpMemoryStorage");
        }

        public bool Save(OtpDataModel data)
        {
            try
            {
                var cacheItemPolicy = new CacheItemPolicy
                {
                    AbsoluteExpiration = data.ExpireAt,
                };

                _memoryCache.Remove(data.ID);
                return _memoryCache.Add(data.ID, data, cacheItemPolicy);
            }
            catch
            {
                return false;
            }
        }

        public OtpDataModel Get(string key)
        {
            try
            {
                return (OtpDataModel)_memoryCache.Get(key);
            }
            catch
            {
                return null;
            }
        }
        public bool Delete(string key)
        {
            
            return true;
        }
    }
}
