using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Model
{
    public class Terminal
    {
        public string TerminalName { get; set; }

        public List<TerminalItem> ContactList { get; set; }

        public string ImmigrationTitle { get; set; }

        public List<TerminalItem> Immigration { get; set; }

        public string MinistryCivilTitle { get; set; }

        public List<TerminalItem> MinistryCivil { get; set; }

    }

    public class TerminalItem
    {
        public string TerminalContactName { get; set; }

        public string DepartureContactNo { get; set; }

        public string ArrivalContactNo { get; set; }

    }
}