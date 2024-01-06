using Adani.Feature.Common.Data;
using Adani.Feature.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Adani.Feature.Common.Providers
{
    class OtpDataFileStorageProvider : IOtpDataStorageProvider
    {
        private IList<OtpData> _OtpData = new List<OtpData>();
        private string filePath = HttpContext.Current.Server.MapPath("/App_Data/OtpData/OtpData.txt");

        public OtpDataFileStorageProvider()
        {
            if (!File.Exists(filePath))
            {
                var filestream = File.Create(filePath);
                filestream.Close();
            }
            else
            {
                _OtpData = File.ReadAllLines(filePath).Select(x => JsonConvert.DeserializeObject<OtpData>(x)).ToList();
            }
        }

        public bool Save(OtpDataModel data)
        {
            try
            {
                var id = StringToGUID(data.ID);
                var otpData = _OtpData.FirstOrDefault(x => x.ID == id);
                if (otpData != null)
                {
                    otpData.OTP = data.OTP;
                    otpData.ExpireAt = data.ExpireAt.DateTime;
                }
                else
                {
                    otpData = new OtpData()
                    {
                        ID = id,
                        Key = data.ID,
                        OTP = data.OTP,
                        ExpireAt = data.ExpireAt.DateTime
                    };
                    _OtpData.Add(otpData);
                }

                File.WriteAllLines(filePath, _OtpData.Select(x => JsonConvert.SerializeObject(x)));
                return true;
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
                var id = StringToGUID(key);

                var otpData = _OtpData.FirstOrDefault(x => x.ID == id);
                if (otpData != null)
                {
                    return new OtpDataModel
                    {
                        ID = otpData.ID.ToString(),
                        OTP = otpData.OTP,
                        ExpireAt = otpData.ExpireAt
                    };
                }
            }
            catch
            {
            }

            return null;
        }

        Guid StringToGUID(string value)
        {
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(value));
            return new Guid(data);
        }

        public bool Delete(string key)
        {
            var id = StringToGUID(key);
            var otpData = _OtpData.FirstOrDefault(x => x.ID == id);
            _OtpData.Remove(otpData);
            File.WriteAllLines(filePath, _OtpData.Select(x => JsonConvert.SerializeObject(x)));
            return true;
        }
    }
}
