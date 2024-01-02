using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adani.SuperApp.Airport.Foundation.DataAPI.Platform.Services
{
    public interface IAPIResponse
    {
        string GetAPIResponse(string method, string URL, Dictionary<string, string> headers, Dictionary<string, string> parameters, object body);
        Task<string> GetAPIResponseNew(string method, string URL, Dictionary<string, string> headers, Dictionary<string, string> parameters, object body);
        Task<string> GetDelayedAPIResponse(string method, string URL, Dictionary<string, string> headers, Dictionary<string, string> parameters, object body);
    }
}
