using Sitecore.Data;

namespace Adani.SuperApp.Airport.Feature.Payment.Platform
{
    public static class Constants
    {
        public const string Card = "CARD";
        public const string NetBanking = "NETBANKING";
        public const string Upi = "UPI";
        public const string Wallet = "WALLET";
        public const string EMI = "EMI";
    }

    public static class Templates
    {
        public static class PaymentOption
        {
            public static readonly ID Id = new ID("{D7F870BC-89D8-4F17-95EC-59ACFD7DA05C}");

            public static class Fields
            {
                public static readonly string ApprovePaymentFieldId = "{51D89A97-ABF4-4DE2-AEBC-CA4FAEDE0F1B}";
                public static readonly string CardOptionFieldId = "{4FB965A2-6F2A-4541-A2AC-DF9236EF8B8E}";
                public static readonly string NetBankingListFieldId = "{E53C07CC-4F62-4254-B13F-0D3A7659B164}";
                public static readonly string paymentHeading = "{4EA92FB6-4513-40BB-9566-EAFB03F3EAB3}";

                public static readonly string paymentText = "{C52DAFFC-1EE5-4FEC-8504-990A1286C6A5}";

                public static readonly string paymentTypeListFieldId = "{A854FC89-15EB-4346-ABF5-731345BCB2A3}";
                public static readonly string PromoCardFieldId = "{315EB29D-2F80-480E-BF18-8B71D2B22ED6}";
                public static readonly string SafeIconBigFieldId = "{E73265CD-5087-4A18-A3BF-07C41891BAA7}";
                public static readonly string SafeIconSmallFieldId = "{68E3245B-AA26-426F-AD90-680BC81D29D2}";
                public static readonly string SecurityCardFieldId = "{39C80268-ED6C-4F64-BDDD-268B2EE83E38}";
                public static readonly string UpiOptionFieldId = "{09CB208E-3374-4A31-AD24-2786D8028CB1}";
                public static readonly string walletListFieldId = "{70917394-7533-4DEF-8A7A-404D0ED8BE26}";
                public static readonly string EMIListFieldId = "{33588480-EB19-48DB-9D8A-3FA7275B3B2E}";

                public static readonly string SafeTextFieldId = "{47D6E16F-9484-4606-B56F-CB091D1F3772}";
                public static readonly string OfferTextFieldId = "{CEE65C4E-A328-4A7B-8366-97B83637D05A}";
                public static readonly string DownTimeText = "DownTimeText";
                public static readonly string FluctuateTimeText = "FluctuateTimeText";
            }
        }

        public class CardOption
        {
            public static readonly string CardIconBigFieldId = "{E3C35C55-E534-42E5-BA48-156175CA1430}";
            public static readonly string CardIconSmallFieldId = "{E7ABD4DE-1097-4A45-9E9A-0A6AA7BAB248}";
            public static readonly string CardImageBigFieldId = "{E791D133-9353-435A-8989-0915410F220A}";
            public static readonly string CardImagesFieldId = "{E30CF3C6-BC69-45DB-B5F7-E8EABC7C011F}";
            public static readonly string CardImageSmallFieldId = "{71D2DA57-C489-4B8D-8518-38375100B5AC}";
            public static readonly string CardNumberLabelFieldId = "{4A98D2C9-B74A-483A-91B4-C943FA153CE1}";
            public static readonly string CvvLabelFieldId = "{FF06F00A-3E36-435F-B5BD-A9FA68BF4524}";
            public static readonly string CVVLengthFieldId = "{B1C08DE1-D74C-420F-9804-7E56D81DEE82}";
            public static readonly string IncorrectCardNumberErrMsgFieldId = "{73446224-472B-40F0-88A5-60151432A35A}";
            public static readonly string IncorrectNameCardErrMsgFieldId = "{8AA54AD4-A3DA-4ECD-9C89-301E47AA24A4}";
            public static readonly string IncorrectValidThruErrMsgFieldId = "{D25F722D-9898-423C-A87F-D1AD0203E807}";
            public static readonly string IsNameMandatoryFieldId = "{4F817ACC-57FA-4A74-A5F0-25AFE41DACEC}";
            public static readonly string NameOnCardLabelFieldId = "{4A2477BF-79C8-4246-97B0-2630256DCA7C}";
            public static readonly string RegexForCardNumberFieldId = "{66E03084-5D13-4051-A67C-D9A9A98D3AF4}";
            public static readonly string RequiredCardNumberErrMsgFieldId = "{06BD02FA-9064-4517-B669-93EC284D6A52}";
            public static readonly string RequiredValidThruErrMsgFieldId = "{08226CAF-8F80-46C4-B0A6-945DFC80F7E3}";
            public static readonly string SecureCardLabelFieldId = "{BB8520A1-2F82-40D1-8FD3-E12CE1DE3998}";
            public static readonly string ValidThruLabelFieldId = "{FE60F1DF-CAFA-4EEE-AA1F-BCE05D695F43}";
            public static readonly string ExpireIconSmallFieldId = "{17CD44DC-B05D-4C0C-9B10-37B86161CF1D}";
            public static readonly string ExpireIconBigFieldId = "{F31AE940-69D3-47FC-BE33-42212A29D614}";
            public static readonly string InfoIconSmallFieldId = "{DB38264E-A230-4DBB-8F89-195966DC004C}";
            public static readonly string InfoIconBigFieldId = "{ED9ACE46-AF06-4D34-807F-5F9AF0EBEAE5}";
            public static readonly string RequiredCvvFieldId = "{5B862DDC-3118-4148-A616-F011663EE840}";
            public static readonly string IncorrectCvvFieldId = "{0779F9EB-3B63-476E-B15F-1423446B9A34}";
            public static readonly string IsActiveFieldId = "{D4D1CCF2-EF72-439A-B18A-8A112A8C6338}";
            public static readonly string IsShowInPageFieldId = "{035394EF-FCA8-4D95-9D5C-B918990FFF8C}";
           
        }

        public class NetBanking
        {
            public static readonly string BankIconLargeFieldId = "{DC99D044-64BC-40A4-BF47-4C0923C4E9A7}";
            public static readonly string BankIconSmallFieldId = "{31BE6D6C-1154-4B70-9C37-6776FAC08ADA}";
            public static readonly string IsShowInPageFieldId = "{10017E07-8A6E-4616-9EE3-B5D2CC1495A5}";
            public static readonly string NetBankingCodeFieldId = "{B87432B4-2D27-4C06-9BCC-E98FCA2396C1}";
            public static readonly string NetBankingIsActiveFieldId = "{4A99126F-1035-441C-BD7B-D7B05C1A5AF1}";
            public static readonly string NetBankingNameFieldId = "{6EF27358-4340-4BF8-950E-9FD889FB5D4B}";
        }

        public class PaymentType
        {
            public static readonly string WalletItem = "Wallet Option List";
            public static readonly string UPIItem = "UPI List";
            public static readonly string NewWalletItem = "NewWallet Option List";
            public static readonly string AndroidUPIItem = "Android UPI List";
            public static readonly string IOSUPIItem = "IOS UPI List";
            public static readonly string NewUPIItem = "NewUPI List";
            public static readonly string EMIItem = "EMI List";
            public static readonly string TypeFilter = "{593A1698-8632-4F9E-A209-D5BF350EF94F}";
            public static readonly string PaymentTypeFieldId = "{C22ABE45-AFAA-44A7-ACC3-A1783A56B804}";
            public static readonly string PaymentTypeIconBigFieldId = "{264E0EFB-1BF0-4DA7-BCE6-7E2A9C4CD906}";
            public static readonly string PaymentTypeIconSmallFieldId = "{3820F4B2-AD85-4E09-809A-3AA6E59C431D}";
            public static readonly string PaymentTypeNameFieldId = "{8797992B-565E-4369-B0DD-39687CCBC1BD}";
        }

        public class Promocards
        {
            public static class Fields
            {
                public static readonly string ButtonTextFieldId = "{FA4BD4E1-1255-42D6-8C34-BDB5E56F28C8}";
                public static readonly string DescriptionFieldId = "{12E90CD7-E7A7-4820-B3B6-F150158356E0}";
                public static readonly string HeadingFieldId = "{7F3EFBF0-F0A5-44BF-AB8C-F67F1CE942D2}";
                public static readonly string ImageLargeFieldId = "{8094D707-79AE-41AC-9668-A360565566CF}";

                public static readonly string ImageSmallFieldId = "{015149BC-47A9-4786-93F5-6464DE8F73CB}";

                public static readonly string NoteFieldId = "{1DFF2DA0-E02D-41D5-AEE3-E43F6569FEF2}";
                public static readonly string BtnLinkFieldId = "{2127B2B5-7049-416F-99FF-91C31E3080EA}";
            }
        }

        public class SecurityCard
        {
            public class Fields
            {
                public static readonly string AskLaterLinkFieldId = "{2D5F5B34-EB2A-4A8D-86BB-5B0BBA84F63E}";
                public static readonly string SecurePayLinkFieldId = "{615F5C14-8F8E-4127-A75B-C26779D8B93A}";
                public static readonly string SecurityCardDescFieldId = "{8F9FB4AC-5846-486B-ACF5-716374E1563F}";
                public static readonly string SecurityCardHeadingFieldId = "{ABD4F7A9-0315-419A-B7B7-A46823F62532}";
                public static readonly string SecurityImageBigFieldId = "{37EFE269-3CF6-41D5-A6AB-FEBDAF747846}";
                public static readonly string SecurityImageSmallFieldId = "{6B6CEC70-3406-4309-8F73-F055C3785C72}";
                public static readonly string SecurityPoint1FieldId = "{FB998AA6-54C0-4A92-959F-6CB11D895786}";

                public static readonly string SecurityPoint2FieldId = "{E8195318-F8FC-491F-800D-14F69A937353}";
            }
        }

        public class UpiOption
        {
            public static readonly string UpiCodeFieldId = "{AD718A43-3B5D-4B3A-8E7D-D74FE9A8DA05}";
            public static readonly string UpiIsActiveFieldId = "{6DF8EF86-1EDD-4DC2-9889-F6EDB9CF9343}";
            public static readonly string UpiLabelFieldId = "{293C1CCE-7470-4DE0-A0CE-B915B270331E}";
            public static readonly string UpiLargeIconFieldId = "{ED34DAFC-90FC-4AF5-8A1B-6B31CBDCAC76}";
            public static readonly string UpiSmallIconFieldId = "{8604A95D-8D07-4FE0-BC43-AE0B112AF687}";
            public static readonly string PackageNameId = " {2729766E-3002-4040-AB68-870E4761375D}";
        }

        public class WalletOption
        {
            public static readonly string walletCodeFieldId = "{81D20DE5-270E-47BC-8F7E-2696CA371333}";
            public static readonly string walletIsActiveFieldId = "{F54D7BBD-990E-425A-A917-7AE3EE72AE5C}";
            public static readonly string WalletLargeIconFieldId = "{D303D5BA-F265-413C-8451-F772C9020814}";
            public static readonly string walletNameFieldId = "{3433F910-6EF9-4780-AEDB-820C83C26590}";
            public static readonly string WalletSmallIconFieldId = "{D927C153-D27E-47B3-BE40-987948C654EC}";
        }
        public class EMIOption
        {
            public static readonly string EMICodeFieldId = "{EF5474E3-7404-4ECE-8B88-F3594D59B159}";
            public static readonly string EMIIsActiveFieldId = "{93FFA95A-E531-4A8A-82A7-FA0150975F8C}";
            public static readonly string EMILargeIconFieldId = "{067C690E-C54B-45B3-B32E-C46379350A8B}";
            public static readonly string EMINameFieldId = "{359A440C-32D3-44B8-9168-CD5D774A9428}";
            public static readonly string EMISmallIconFieldId = "{48F60621-C03D-47F8-B9E7-9C75AB252987}";
            public static readonly string TNCFieldId = "{A1E97447-FBB4-450F-86A1-F91EE4EE924E}";
            public static readonly string TNCDC = "TnCDC";

        }
    }
}