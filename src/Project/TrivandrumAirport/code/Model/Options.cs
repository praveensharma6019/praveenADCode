

// Sitecore.TrivandrumAirport.Website.Model.Options
using System;
using Sitecore.Foundation.Dictionary.Repositories;

[Serializable]
public class Options
{
    public string Option
    {
        get;
        set;
    }

    public bool Checked
    {
        get;
        set;
    }

    public string Response
    {
        get;
        set;
    }

    public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");

    public static string FirstNameCaption => DictionaryPhraseRepository.Current.Get("/Accounts/Register/FirstName", "First Name");
}
