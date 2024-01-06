using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Runtime.CompilerServices;

namespace Sitecore.MangaloreAirport.Website.Model
{
    [Serializable]
    public class Options
    {
        public bool Checked
        {
            get;
            set;
        }

        public static string FirstNameCaption
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/Accounts/Register/FirstName", "First Name");
            }
        }

        public string Option
        {
            get;
            set;
        }

        public static string Required
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
            }
        }

        public string Response
        {
            get;
            set;
        }

        public Options()
        {
        }
    }
}