/*
 *
 * Copyright (c) 2022 Adani Digital Labs
 * All rights reserved.
 * Adani Digital Labs Confidential Information
 *
 */

using Farmpik.Domain.Common.ConfigurationModel;
using Farmpik.Domain.Dto;
using Farmpik.Domain.Entities;
using Farmpik.Domain.Interfaces.Repositories;
using Farmpik.Infrastructure.Models;
using Farmpik.Infrastructure.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Farmpik.Infrastructure.Repository
{
    public class HelperMethod : IHelperMethod
    {
        private readonly EncryptionsOptions _encryptionsOptions;
        private readonly SmsOptions _smsOptions;

        public HelperMethod(ConfigurationOptions configuration)
        {
            _encryptionsOptions = configuration.EncryptionsOptions;
            _smsOptions = configuration.SmsOptions;
        }

        public string GenerateOtp()
        {
            int min = 100000;
            int max = 999999;
            int elemInRange = max - min + 1;
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            var byteArray = new byte[32];
            provider.GetBytes(byteArray);
            var randomInt = BitConverter.ToInt32(byteArray, 0);
            var mod = Math.Abs(randomInt) % elemInRange;
            return (min + mod).ToString();
        }

        public async Task<bool> SendEmail(string url, string body)
        {
            HttpClient client = new HttpClient();
            var content = new StringContent(body, Encoding.UTF8, ApplicationConstants.Application_JSON);
            HttpResponseMessage response = await client.PostAsync(url, content);
            return response.StatusCode == HttpStatusCode.OK;
        }


        public async Task<bool> SendOtp(Vendor vendor, string otp, string platform)
        {
            var farmpikOTPDataModel = new FarmpikOTPDataModel();
            farmpikOTPDataModel.id = vendor.Id.ToString();
            farmpikOTPDataModel.smsData = new SmsData()
            {
                recipient = vendor.Telephone,
                otp= otp
            };

            //var otpRequest =new{
            //    app = "adanione",
            //    occasion = platform == "ios" ? "login-otp-ios" : "login-otp-android",
            //    category = "farm-pik",
            //    data = new
            //    {
            //        otp,
            //        hashCode = _smsOptions?.SmsApiKey
            //    },
            //    to = new List<string> { $"91{vendor.Telephone}" }
            //};

            HttpClient client =new HttpClient() { BaseAddress = new Uri(_smsOptions?.SmsUrl ?? string.Empty) };
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApplicationConstants.Application_JSON));
            client.DefaultRequestHeaders.Add(ApplicationConstants.API_Key, _smsOptions?.SmsApiKey);
            var stringContent = new StringContent(JsonConvert.SerializeObject(farmpikOTPDataModel), Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("", stringContent);
            return response.StatusCode == HttpStatusCode.OK;
        }
        public string Encrypt<T>(T model)
        {
            string cipherText;
            var keys = Convert.FromBase64String(_encryptionsOptions?.Key ?? string.Empty);
            var IVs = Convert.FromBase64String(_encryptionsOptions?.IV ?? string.Empty);

            using (Aes newAes = Aes.Create())
            {
                newAes.Key = keys;
                newAes.IV = IVs;

                ICryptoTransform encryptor = newAes.CreateEncryptor(keys, IVs);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            string plainText = model.GetType().Name == typeof(string).Name ? model.ToString() : JsonConvert.SerializeObject(model);

                            swEncrypt.Write(plainText);
                        }
                        cipherText = Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }

            return cipherText;
        }

        public T Decrypt<T>(string cipherText)
        {
            string plainText;
            var keys = Convert.FromBase64String(_encryptionsOptions?.Key ?? string.Empty);
            var IVs = Convert.FromBase64String(_encryptionsOptions?.IV ?? string.Empty);

            using (Aes newAes = Aes.Create())
            {
                newAes.Key = keys;
                newAes.IV = IVs;

                ICryptoTransform decryptor = newAes.CreateDecryptor(keys, IVs);
                try
                {
                    using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            StreamReader srDecrypt = new StreamReader(csDecrypt);
                            plainText = srDecrypt.ReadToEnd();
                        }
                    }
                }
                catch
                {
                    plainText = "{}";
                }
            }

            if (typeof(T) != typeof(string) && string.IsNullOrEmpty(plainText))
            {
                plainText = "{}";
            }

            return JsonConvert.DeserializeObject<T>(plainText);
        }
    }
}