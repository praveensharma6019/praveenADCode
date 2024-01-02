using Sitecore.Data;

namespace Project.AdaniInternationalSchool.Website.Templates
{
    public static class CareerFormTemplate
    {
        public static class Fields
        {
            public static readonly ID title = new ID("{BA6C5457-1D23-4827-B6C8-4F6C50A8D768}");
            public static readonly ID subTitle = new ID("{C8182207-6344-46C7-BFDD-0ADCB80ECD1B}");
            public static readonly ID heading = new ID("{F8DE225A-8948-43AF-AE9B-EEAFEE40756F}");
            public static readonly ID formFields = new ID("{B2E1C64F-4B3F-4374-B72E-C516096B8B15}");
            public static readonly ID checkboxField = new ID("{1A24D529-514F-4422-BBF2-92B517C04835}");
            public static readonly ID reCaptchaField = new ID("{6C3DCE38-8DF9-4CFC-B06B-A0C388C7D59A}");
            public static readonly ID submitButton = new ID("{62621DE7-0EE3-4E4F-A139-9FF5D6DB207F}");
            public static readonly ID formGTMData = new ID("{57A0E14B-90C9-4FEC-8F7F-F3265435F488}");
            public static readonly ID thankYouData = new ID("{27C06C25-DD3A-46E9-B3B3-A1CEEE81DA01}");


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

                public static readonly ID MaxFileSize = new ID("{EF7B8D9E-2C05-4EA3-B0A6-8A0B620B7C29}");
                public static readonly ID FileSizeUnit = new ID("{76A13EA6-D06D-4CFD-AB5B-8252DD716E17}");
                public static readonly ID MaxFileCount = new ID("{AF4F2BA2-1284-40E4-8572-5ADADAECC393}");
                public static readonly ID AllowedContentTypes = new ID("{41FE1C8B-64AD-423B-BA86-471389588968}");


                public static readonly ID CheckboxDefaultValue = new ID("{3C05A626-A3B1-4CBC-BF67-198537A13E20}");
                public static readonly ID ValueProviderParameters = new ID("{F6696439-2DAC-4183-8F3D-1979188D8336}");

            }
            public class ErrorMessagesSection
            {

                public static readonly ID requiredFieldErrorMessageName = new ID("{EC7DC10D-4C2E-4B6C-9AD7-E08844A8AA39}");
                public static readonly ID maxLengthErrorMessageName = new ID("{E7B36372-77E0-46E3-A699-815BB345F1D2}");
                public static readonly ID minLengthErrorMessageName = new ID("{03ED478F-1E0F-4956-9CB7-D0602CAD4089}");
                public static readonly ID regexErrorMessageName = new ID("{30C4D93E-DDE8-46C8-BCD9-38CC16A3B603}");
                public static readonly ID requiredFieldErrorMessageMobileNumber = new ID("{5FB5D897-BD79-48B1-A504-30FB8CAD4005}");
                public static readonly ID maxLengthErrorMessageMobileNumber = new ID("{FEAADEEF-B755-4E84-B575-B910799F14FD}");
                public static readonly ID minLengthErrorMessageMobileNumber = new ID("{A39A9B15-3C37-4540-9089-6714DB0C5AEB}");
                public static readonly ID regexErrorMessageMobileNumber = new ID("{DA22DEF5-97F0-4A64-B4F1-F1E5A2936452}");
                public static readonly ID requiredFieldErrorMessageEmail = new ID("{9E4BF7FF-BA52-4D4B-A7F0-99544B9996CB}");
                public static readonly ID maxLengthErrorMessageEmail = new ID("{A7AA3F5B-FD8C-4769-A7C6-8E0B64D1442A}");
                public static readonly ID minLengthErrorMessageEmail = new ID("{52C5C35E-0684-4BF7-8216-3B9CDB57B534}");
                public static readonly ID regexErrorMessageEmail = new ID("{9E50818D-6690-4DCE-B279-407E930FE064}");
                public static readonly ID requiredFieldErrorMessageInterestedPosition = new ID("{6477D4B2-A28F-479B-9157-BA772D442D95}");
                public static readonly ID requiredFieldErrorMessageCurrentOrganisation = new ID("{10A23A8F-B1D6-422B-B0A2-253D61AB8C9A}");
                public static readonly ID maxLengthErrorMessageCurrentOrganisation = new ID("{4CB3F0E0-C7DE-42AC-B069-D978E0E1AFCD}");
                public static readonly ID minLengthErrorMessageCurrentOrganisation = new ID("{D5D8AC4B-40AD-41EC-97C9-03B588A10EA6}");
                public static readonly ID regexErrorMessageCurrentOrganisation = new ID("{75683A3A-E02B-4EFF-8D2E-B3A665465779}");
                public static readonly ID requiredFieldErrorMessageTotalExperience = new ID("{614F5FEE-5FDD-4A90-A48E-0334B8002588}");
                public static readonly ID maxFileSizeErrorMessageAttachResume = new ID("{80CA1601-3F9E-42B4-9B90-948EC1B04C0F}");
                public static readonly ID minFileSizeErrorMessageAttachResume = new ID("{CCC0323B-BE53-439F-AC48-EE41CAF6B274}");
                public static readonly ID requiredFieldErrorMessageAttachResume = new ID("{FD79AB2E-E9D2-4BA4-AD27-3C83FDF9089A}");
                public static readonly ID regexErrorMessageAttachResume = new ID("{F3579D2C-55F2-4789-821A-5CF6B28BEEC3}");
                public static readonly ID requiredFieldErrorMessageTermsAndConditions = new ID("{DDA1E009-9260-4084-BA5C-9FBE1DFBE6CC}");
                public static readonly ID requiredFieldErrorMessagereCaptcha = new ID("{5915973A-6B61-40B5-B7EF-70AEAF1FE445}");
                public static readonly ID checkboxFieldUrl = new ID("{4861DC0F-0DF6-4052-8404-68FF41F010E2}");
                public static readonly ID checkboxFieldtarget = new ID("{308E9ADD-A1FF-4836-9467-523012DDFA35}");

            }

            public class TemplatesSection
            {
                public static readonly ID Position = new ID("{9121D435-48B8-4649-9D13-03D680474FAD}");
                public static readonly ID Resume = new ID("{17203DAA-0DED-4160-A23C-EC1114AB4FEF}");
                public static readonly ID Agreement = new ID("{2F07293C-077F-456C-B715-FDB791ACB367}");
                public static readonly ID Submit = new ID("{94A46D66-B1B8-405D-AAE4-7B5A9CD61C5E}");
                public static readonly ID Name = new ID("{0908030B-4564-42EA-A6FA-C7A5A2D921A8}");

                public static readonly ID NameField = new ID("{4299957F-C7F6-4F59-B834-318B0356984F}");
                public static readonly ID PositionField = new ID("{383343A4-1C28-4878-8E26-6F6F4DE272ED}");
                public static readonly ID MobileNumber = new ID("{326268C7-787F-4AEB-84A1-B86B22591A67}");
                public static readonly ID Email = new ID("{695D46B6-3357-4CCC-BA7E-F3A6BD495A7F}");
                public static readonly ID Organization = new ID("{A9ACD93B-52F9-444D-A384-91C9DC9EE102}");
                public static readonly ID Experience = new ID("{AFCD6CAD-3A77-4EBC-B62B-419036AC60B8}");
                public static readonly ID ResumeField = new ID("{ABA7283E-89E1-4515-A759-B9B076261CC1}");
                public static readonly ID AgreementField = new ID("{59361CC6-264E-4A55-8770-4E1FC65731EA}");
                public static readonly ID SubmitField = new ID("{03B7A2E4-8078-497C-AE89-B7E0E0359026}");


                public static readonly ID errMsgfieldItem = new ID("{1918E7B7-360F-4541-9B29-F422D5E561E5}");
                public static readonly ID thankYouDatafieldItem = new ID("{30EF7359-71ED-4F27-9E2B-A27719E63604}");
                public static readonly ID reCaptchaFieldfieldItem = new ID("{61C9780D-12C7-4D71-96F0-2A6CEC768568}");
                public static readonly ID formGTMDatafieldItem = new ID("{F9765888-023E-4705-B408-62C0E8FF4FC2}");
                public static readonly ID progressDatafieldItem = new ID("{B005A4C6-2D2A-4283-A8DB-C07283E3D461}");
                public static readonly ID formFailDatafieldItem = new ID("{A8DECC5D-39C6-4A79-91E5-3BE323CA95A5}");
            }

            public class FormGTMDataSection
            {
                public static readonly ID gtmEvent = new ID("{CF699354-2A07-4E98-BFFE-579454F88D06}");
                public static readonly ID gtmCategory = new ID("{64DE163C-C82C-492E-866C-C7338085CE76}");
                public static readonly ID gtmSubCategory = new ID("{E58CAD70-07D8-4191-876C-2E0BC2BBCF70}");
                public static readonly ID gtmEventSub = new ID("{B8FEA7CD-60FE-40E9-8FAD-A2284FED0C39}");
                public static readonly ID pageType = new ID("{565577DA-A5AF-42E1-A063-14D8A9453EF8}");
            }
            public class ReCaptchaFieldSection
            {
                public static readonly ID title = new ID("{BA6C5457-1D23-4827-B6C8-4F6C50A8D768}");
                public static readonly ID subTitle = new ID("{C8182207-6344-46C7-BFDD-0ADCB80ECD1B}");
                public static readonly ID heading = new ID("{F8DE225A-8948-43AF-AE9B-EEAFEE40756F}");
                public static readonly ID errorMessages = new ID("{15B2D475-5393-4421-9B40-F2F43E1EB2F8}");
                public static readonly ID required = new ID("{282D17A8-99FA-4294-857E-D9655469FB53}");
            }

            public class ThankYouDataSection
            {
                public static readonly ID heading = new ID("{F8DE225A-8948-43AF-AE9B-EEAFEE40756F}");
                public static readonly ID description = new ID("{9A7A3637-6481-469F-9D20-247CBB1460C3}");
            }
        }
    }
}
