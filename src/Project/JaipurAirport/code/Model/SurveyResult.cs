using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Sitecore.JaipurAirport.Website.Model
{
    [Serializable]
    public class SurveyResult
    {
        public string CANumber
        {
            get;
            set;
        }

        public string Email
        {
            get;
            set;
        }

        public string MobileNo
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public List<SubSurveyResult> SubSurveyResult
        {
            get;
            set;
        }

        public SurveyResult()
        {
            this.SubSurveyResult = new List<SubSurveyResult>();
        }
    }
}