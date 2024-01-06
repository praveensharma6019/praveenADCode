using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.InspireBkc.Website;
using Sitecore.InspireBkc.Website.Models;

public class InspireBkcRepository
{
	private InspireBkcDataContextDataContext dc = new InspireBkcDataContextDataContext();

	public void DeleteOldOtp(string mobilenumber = null)
	{
		if (!string.IsNullOrEmpty(mobilenumber))
		{
			IQueryable<InspireBkcOtpHistory> removeexistotp = dc.InspireBkcOtpHistories.Where((InspireBkcOtpHistory x) => x.MobileNumber == mobilenumber);
			dc.InspireBkcOtpHistories.DeleteAllOnSubmit(removeexistotp);
			dc.SubmitChanges();
		}
	}

    public bool CanSendOTP(List<DateTime> lastOtpsSentOn, int boundr1InMinutes, int otpAllowedInBundary1, int boundr2InMinutes)
    {
        if (lastOtpsSentOn.Count < otpAllowedInBundary1)
        {
            return true;
        }
        DateTime firstOtpSentInBoundry = lastOtpsSentOn.OrderByDescending(d => d).Take(otpAllowedInBundary1).Last();
        DateTime lastOtpSentInBoundry = lastOtpsSentOn.OrderByDescending(d => d).First();

        if ((lastOtpSentInBoundry - firstOtpSentInBoundry).TotalMinutes <= boundr1InMinutes)
        {
            if (firstOtpSentInBoundry.AddMinutes(boundr2InMinutes) >= DateTime.Now)
            {
                return false;
            }
        }
        return true;
    }

    public string StoreGeneratedOtp(Inspire model)
	{
		string sRandomOTP = GenerateRandomOTP(6);
        var otpDateTimeList = dc.InspireBkcOtpHistories.Where(o => o.MobileNumber == model.mobile && o.ExpiryTime.HasValue).OrderByDescending(o => o.ExpiryTime).Select(x => x.ExpiryTime.Value).ToList();
        var canSendOtpResult = this.CanSendOTP(otpDateTimeList, 30,3,30);
        if(!canSendOtpResult)
        {
            return "411";
        }
        InspireBkcOtpHistory entity = new InspireBkcOtpHistory
		{
			MobileNumber = model.mobile,
			otp = sRandomOTP,
			status = false,
            RemainingAttempt = 3,
            ExpiryTime = DateTime.Now.AddMinutes(5),
        };
		dc.InspireBkcOtpHistories.InsertOnSubmit(entity);
		dc.SubmitChanges();
		return sRandomOTP;
	}

	public string GetOTP(string mobilenumber = null)
	{
		if (!string.IsNullOrEmpty(mobilenumber))
		{
            InspireBkcOtpHistory data = dc.InspireBkcOtpHistories.Where(x => x.MobileNumber == mobilenumber && x.ExpiryTime > DateTime.Now && x.RemainingAttempt > 0).OrderByDescending(x => x.Id).FirstOrDefault();
            if (data != null)
            {
                data.RemainingAttempt--;
                dc.SubmitChanges();
                return data.otp;
            }
        }
		return string.Empty;
	}

	private string GenerateRandomOTP(int iOTPLength)
	{
		string[] saAllowedCharacters = new string[10] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
		string sOTP = string.Empty;
		string sTempChars = string.Empty;
		Random rand = new Random();
		for (int i = 0; i < iOTPLength; i++)
		{
			int p = rand.Next(0, saAllowedCharacters.Length);
			sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];
			sOTP += sTempChars;
		}
		return sOTP;
	}
}
