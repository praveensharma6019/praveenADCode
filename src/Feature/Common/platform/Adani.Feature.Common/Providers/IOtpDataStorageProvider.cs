using Adani.Feature.Common.Models;

namespace Adani.Feature.Common.Providers
{
    public interface IOtpDataStorageProvider
    {
        bool Save(OtpDataModel data);
        OtpDataModel Get(string key);
        bool Delete(string key);
    }
}
