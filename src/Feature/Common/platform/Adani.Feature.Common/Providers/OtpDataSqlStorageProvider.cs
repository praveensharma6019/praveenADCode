using Adani.Feature.Common.Data;
using Adani.Feature.Common.Models;
using Newtonsoft.Json;
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Adani.Feature.Common.Providers
{
    public class OtpDataSqlStorageProvider : IOtpDataStorageProvider
    {
        private readonly OtpDataDbContext _dbContext;

        public OtpDataSqlStorageProvider()
        {
            _dbContext = new OtpDataDbContext();
        }

        public bool Save(OtpDataModel data)
        {
            try
            {
                var id = StringToGUID(data.ID);
                var otpData = _dbContext.OtpDatas.FirstOrDefault(x => x.ID == id);
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
                }

                _dbContext.OtpDatas.AddOrUpdate(otpData);
                return _dbContext.SaveChanges() > 0;
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

                var otpData = _dbContext.OtpDatas.FirstOrDefault(x => x.ID == id);
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
            
            return true;
        }
    }
}
