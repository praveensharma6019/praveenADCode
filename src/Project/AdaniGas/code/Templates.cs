using Sitecore.Data;

namespace Sitecore.AdaniGas.Website
{
    public class Templates
    {
        public struct CostCalculatorstConfiguration
        {
            public static readonly ID ID = new ID("{139C04C8-395B-468A-B690-F7E85A34C866}");

            public struct DomesticPNGCostCalculator
            {
                public static readonly ID EquivalentMMBTUFactor1 = new ID("{048656FE-8179-4060-9BAC-22A9BFDAB4B8}");
                public static readonly ID EquivalentMMBTUFactor2 = new ID("{4DF359F5-B409-462D-B57C-3154C0051A40}");
                public static readonly ID EquivalentMMBTUFactor3 = new ID("{ECCFF359-CF3B-44C8-963F-D70D22965AEB}");
            }

            public struct CNGCostCalculator
            {
                public static readonly ID ExpectedCNGMileage = new ID("{D82C0342-07A4-4FC3-AA2B-4FCE4A773896}");
            }

            public struct Datasource
            {
                public static readonly ID DomesticPNGCC_CylinderSizes = new ID("{C61E1D19-CAB6-4E8D-919A-DF3EA379B343}");
                public static readonly ID DomesticPNGCC_LocationWiseCurrentPNGGasRate = new ID("{DF84FF64-A402-4BF7-8F4F-581171B9C930}");
                public static readonly ID CNGCostCalculator_LocationWiseRateOfGasPerKG = new ID("{4EFF8EF4-7196-495B-9901-D4229E038EDB}");

                public static readonly ID CNGCostCalculator_CurrentFuelRate_Petrol = new ID("{A0AF805F-A1D6-487F-91DD-A1B5EC1C95FC}");
                public static readonly ID CNGCostCalculator_CurrentFuelRate_Diesel = new ID("{2CBB82B5-AA96-46DF-A3CA-E35E6A6E443F}");
                public static readonly ID CNGCC_FuelTypes = new ID("{3D64E2F1-C369-4137-822E-AB67B0B6241A}");
            }
        }
    }
}

