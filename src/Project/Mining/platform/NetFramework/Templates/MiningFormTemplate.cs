using Sitecore.Data;
using System;

namespace Project.Mining.Website.Templates
{
    public class MiningFormTemplate
    {
        public class SubscribeUsFormTemplate
        {
            public static readonly ID NewsLetter = new ID("{BFD9EFBD-FBF1-4ABF-9A4F-B9DA6DA65545}");
            public static readonly ID Text = new ID("{3D5E26B3-FF3C-4675-ADDD-8A924E142B5D}");
            public static readonly ID Email = new ID("{1FF4FC21-A9BB-4638-82FD-711423EA2F20}");
            public static readonly ID GoogleV3Captcha = new ID("{5AF95120-DA67-4EBB-9C4D-2AF5FDF80996}");
        }
        public class EnquiryFormTemplate
        {
            public static readonly ID LookingForDropDownList = new ID("{2B117BB2-074B-4D4E-A089-3296E9F3C3E1}");
        }

        public class RequestACallFormTemplate
        {
            public static readonly ID RequestACallHeading = new ID("{5DD76368-4632-4833-AF54-DA575C0052C6}");
            public static readonly ID Name = new ID("{524F2B96-7D61-4DD4-BCCE-4020588BFD48}");
            public static readonly ID Email = new ID("{83F090FB-AA85-46C8-AF17-390A664BAB0F}");
            public static readonly ID Mobile = new ID("{6DF9E672-C4EF-4BF7-B85F-A96BC60EA306}");
            public static readonly ID SelectSolutionType = new ID("{E03E53F3-88EE-469A-BB33-F4A0EFC63FB4}");
            public static readonly ID Message = new ID("{0A4188AB-D6FB-4B3E-90E8-77BD7D59184E}");
            public static readonly ID GoogleV3Captcha = new ID("{60D519C6-D318-4725-A5FE-8D45E4B3AAD8}");
            public static readonly ID SubmitButton = new ID("{B0121BDA-AB00-45E9-9274-34C75C830532}");
            public static readonly ID SolutionTypeDropDownList = new ID("{8D61E1C5-AB8B-4A67-935E-2EED123390CB}");
        }
        public class ContactFormTemplate
        {
            public static readonly ID Helptext = new ID("{AB8C9801-C436-4407-8182-8F5E662A2409}");
            public static readonly ID Feedbacktext = new ID("{ABBFECD4-8D2D-4C66-A383-7C1F55D6F1FA}");
            public static readonly ID FirstName = new ID("{13CD047D-05D9-40A2-9837-FFADFE4DC3F5}");
            public static readonly ID LastName = new ID("{BC1F1432-C104-4B5B-9E26-61BF9878BFAB}");
            public static readonly ID Email = new ID("{3579E7DA-ECEF-43EC-85B8-52718DEDA4CC}");
            public static readonly ID Mobile = new ID("{19B96F3C-F650-4D35-B629-856CD46C9E1A}");
            public static readonly ID SelectHelpType = new ID("{B57D654C-8594-4606-B6A4-F68B495E0D10}");
            public static readonly ID Message = new ID("{ED8A4A2B-28A7-488C-98B5-16577D964A55}");
            public static readonly ID GoogleV3Captcha = new ID("{2F0188B7-BE97-406E-B487-E32F64F69C3F}");
            public static readonly ID SubmitButton = new ID("{9A873830-9CCF-4E17-B355-24B8FCFABB67}");
            public static readonly ID HelpTypeDropDownList = new ID("{9F305F62-878E-495A-B825-D81AE52CA87C}");

        }

        public class MiningFormErrorMessages
        {
            public static readonly ID RequiredFieldErrorMessageName = new ID("{03797F3F-0C7B-49B6-9E2D-2BD5B2DA9EC6}");
            public static readonly ID MaxLengthErrorMessageName = new ID("{FE1E1336-B13D-48C6-8BF6-DC1F246002DA}");
            public static readonly ID MinLengthErrorMessageName = new ID("{5178DD9C-D397-4210-9576-0532C7AA5B8D}");
            public static readonly ID RegexErrorMessageName = new ID("{8167C9E9-1695-4416-8846-655A28055612}");

            public static readonly ID RequiredFieldErrorMessageEmail = new ID("{32201A98-CE3D-473A-BD69-0C938D665561}");
            public static readonly ID MaxLengthErrorMessageEmail = new ID("{83BE5396-0DCB-4A36-8919-4A442DC691C3}");
            public static readonly ID MinLengthErrorMessageEmail = new ID("{9B26148C-D8EF-44B1-8555-014B759CDA73}");
            public static readonly ID RegexErrorMessageEmail = new ID("{597830C2-643D-47B0-A165-7951CA0528CD}");

            public static readonly ID RequiredFieldErrorMessageMobile = new ID("{10D16E75-329A-4229-B431-463C98AE34BC}");
            public static readonly ID MaxLengthErrorMessageMobile = new ID("{86F8712D-5C70-40FE-B652-BFD5D5CF3853}");
            public static readonly ID MinLengthErrorMessageMobile = new ID("{5AA66D66-113C-4D51-9B08-E9016A7BD9C4}");
            public static readonly ID RegexErrorMessageMobile = new ID("{A0D97D73-932F-4E3A-8B17-4D83F5A42D75}");

            public static readonly ID RequiredFieldErrorMessageSelectSolutionType = new ID("{9139EA45-B65E-4AF4-8804-19765A4F571E}");

            public static readonly ID RequiredFieldErrorMessageMessage = new ID("{1A13FC44-960B-4543-8928-6FC5FCE6EF11}");
            public static readonly ID MaxLengthErrorMessageMessage = new ID("{B182785C-C473-4599-AF83-5DEF4BD43F4D}");
            public static readonly ID MinLengthErrorMessageMessage = new ID("{85E05BDD-4A8B-407E-8F15-994C9C866515}");
            public static readonly ID RegexErrorMessageMessage = new ID("{8EE86CA3-45B9-4AF0-92F6-883D780C93F8}");

            public static readonly ID SubscribeUsErrorMessagesItem = new ID("{944CACCA-D1F5-47D9-909F-CF34DD642ACC}");
            public static readonly ID RequestACallFormErrorMessagesItem = new ID("{31312AAB-9F31-489D-A693-B84675BFEFA1}");
            public static readonly ID ThankyouMessagesItem = new ID("{B4992C7D-8260-46AC-8937-53CDA8A0A2F5}");
            public static readonly ID ProgressMessagesItem = new ID("{D1F99AC9-7EF4-4D64-BA4B-433CBA8F7E0F}");
            public static readonly ID FormSubmissionFailedMessagesItem = new ID("{80ADADCA-051D-484F-9FB2-EE9FC5C49307}");
            public static readonly ID RecaptchaErrorMessagesItem = new ID("{96276DF1-A037-43F8-9E19-52BD452D876A}");

            public static readonly ID RecaptchaErrorMessageFieldId = new ID("{5B502D6E-3AB8-4B38-8D25-22B61968C7E7}");
            public static readonly ID IsCaptchaRequiredFieldId = new ID("{C078BFC5-2EBC-4A71-AAE2-27020E2CF1C9}");

        }
        public class MiningContactFormErrorMessages
        {
            public static readonly ID RequiredFieldErrorMessageFirstName = new ID("{5C5671B7-BA96-4E94-9DEF-1B6E5500A088}");
            public static readonly ID RequiredFieldErrorMessageLastName = new ID("{BD579EBC-EFC2-4AAA-83D2-57DBE6A1A4AD}");
            public static readonly ID MaxLengthErrorMessage = new ID("{3C4FE49E-3D88-4C66-9D96-047619FCEC1A}");
            public static readonly ID MinLengthErrorMessage = new ID("{10CB86DA-CC95-416D-BDAA-CDBB738F9A1F}");
            public static readonly ID RegexErrorMessage = new ID("{8137A196-5C23-4860-87CB-D10EBCCF44C1}");
            public static readonly ID RequiredFieldErrorMessageEmail = new ID("{79F60A2A-9E7E-4A51-BF26-7ED454E047B1}");
            public static readonly ID RequiredFieldErrorMessageMobile = new ID("{1AA4EA9E-AA98-49E5-A170-6970355E12FD}");
            public static readonly ID RequiredFieldErrorSelectHelpDropdown = new ID("{9465F4B9-B834-4BAB-A9C1-AD8DD04EFF5A}");
            public static readonly ID RequiredFieldErrorMessageField = new ID("{91BCC56C-B47C-4068-9D59-83C9D0F8506F}");


            public static readonly ID SubscribeUsErrorMessagesItem = new ID("{944CACCA-D1F5-47D9-909F-CF34DD642ACC}");
            public static readonly ID ContactUsFormErrorMessagesItem = new ID("{28818317-2DE3-43BE-AE09-7EF3B9FCBBE8}");
            public static readonly ID ThankyouModelMessagesItem = new ID("{857EF585-FD9E-4E84-9BC3-44AA0362E6BF}");
            public static readonly ID ProgressMessagesItem = new ID("{D1F99AC9-7EF4-4D64-BA4B-433CBA8F7E0F}");
            public static readonly ID FormSubmissionFailedMessagesItem = new ID("{80ADADCA-051D-484F-9FB2-EE9FC5C49307}");
            public static readonly ID RecaptchaErrorMessagesItem = new ID("{96276DF1-A037-43F8-9E19-52BD452D876A}");

            public static readonly ID RecaptchaErrorMessageFieldId = new ID("{D4B09C8E-ADA5-4B3F-BE6E-BE7787D4C7C0}");
            public static readonly ID IsCaptchaRequiredFieldId = new ID("{2F0188B7-BE97-406E-B487-E32F64F69C3F}");

        }
        public class TextTemplate
        {
            public static readonly ID Text = new ID("{9666782B-21BB-40CE-B38F-8F6C53FA5070}");
            public static readonly ID HtmlTag = new ID("{C6CAA979-C3AC-4FFC-861E-2961F5FC3C48}");
        }

        public class FormFieldsSection
        {
            public static readonly ID PlaceholderText = new ID("{A6972F08-CBB5-4830-A8C9-8A0AE34650D1}");
            public static readonly ID minRequiredLength = new ID("{9F7DA002-7B86-436B-8DDA-7EFE5837B88E}");
            public static readonly ID maxAllowedLength = new ID("{71061C01-A495-4BF3-A52C-D378346BFAE2}");
            public static readonly ID DefaultValue = new ID("{B775BCA9-2605-4D60-B367-A0C354BE2504}");
            public static readonly ID Required = new ID("{8A43AF61-0812-4B37-A343-96CDE3F12BB1}");
            public static readonly ID Title = new ID("{71FFD7B2-8B09-4F7B-8A66-1E4CEF653E8D}");
            public static readonly ID DefaultSelection = new ID("{21ED172D-97E2-4AC4-8EAF-0A8860B891F4}");
            public static readonly ID fieldOptionslabel = new ID("{3A07C171-9BCA-464D-8670-C5703C6D3F11}");
            public static readonly ID ValueProviderParameters = new ID("{F6696439-2DAC-4183-8F3D-1979188D8336}");
        }
        public class Form
        {
            public static readonly Guid RequestACall = new Guid("{628EF1CD-BE8E-4A7F-B846-A5C2D443980C}");
            public static readonly ID RequestACallMailBody = new ID("{3CBC7C1C-0953-43E3-AB4B-72219C3051D1}");

            public static readonly Guid Career = new Guid("{D95B43FA-53A5-4C8B-AEAC-D130BBA65C65}");
            public static readonly ID CareerMailBody = new ID("{088940D6-A17A-469C-8377-FB1848E2B6C7}");

            public static readonly Guid Contact = new Guid("{52387A2D-C29E-4741-A144-4AB0F03580DE}");
            public static readonly ID ContactMailBody = new ID("{870F5809-430D-4016-9C0A-E2A6AA9F8BE8}");

            public static readonly Guid Enquiry = new Guid("{F95305F9-C9D4-4669-889F-3C1385C7D2E4}");
            public static readonly ID EnquiryMailBody = new ID("{F49BA101-801C-4C5B-9AD4-0E3980C1FF5F}");

            public static readonly Guid Subscribe = new Guid("{5294DC01-664F-4B78-9E20-651F9D691572}");
            public static readonly ID SubscribeMailBody = new ID("{333B9025-33F2-4582-BD81-3286B8C2C872}");

            public static readonly Guid Brochure = new Guid("{C6C74C99-3B1D-4B3A-8916-9E3176D3FE62}");
            public static readonly ID BrochureMailBody = new ID("{93255120-B4B3-4F8C-8E84-44A3DC91225E}");
        }
    }
}