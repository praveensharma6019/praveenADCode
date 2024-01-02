using Sitecore.Data;

namespace Project.AmbujaCement.Website.Templates.Forms
{
    public class AmbujaFormsTemplate
    {
        public class GetInTouchFormTemplate
        {
            public static readonly ID FirstName = new ID("{4683C374-6A29-48BD-BCAD-92F2C6BC7BE9}");
            public static readonly ID LastName = new ID("{74488B9A-6BB4-49E1-BD72-D01C904205CC}");
            public static readonly ID Mobile = new ID("{B317FA4D-5D7F-441E-BE36-A3DA2ED28046}");
            public static readonly ID Email = new ID("{4C267183-4DCD-4DC3-B5F5-10F6AEF103D2}");
            public static readonly ID LookingForDropDown = new ID("{8711F999-8365-449E-974C-136D12FA56DE}");
            public static readonly ID QueryTypeDropDown = new ID("{29CF7598-805B-4C28-A2B3-B986E0288CA2}");
            public static readonly ID StatesDropdown = new ID("{E43211C8-B5AC-4C3D-A2A1-05515BDF7045}");
            public static readonly ID DistrictDropdown = new ID("{71392F2F-C0A0-4F73-862F-7D09353A2E38}");
            public static readonly ID TermsAndConditionsCheckbox = new ID("{D12C8204-9AC2-4DA0-8A01-52C12F4AEA97}");
            public static readonly ID SubmitButton = new ID("{1FD977FC-D2C6-45F8-9CD2-71897A2A2155}");
        }

        public class GetInTouchFormDetailsTemplate
        {
            public static readonly ID GetInTouchFormDetailsItemId = new ID("{ED1158C0-F335-48AB-AAC2-FC10A5D89F3E}");

            public static readonly ID SuccessMessageFieldId = new ID("{81D8736F-E70D-4C56-BAE8-AD5E3AF85CBB}");
            public static readonly ID ProgressMessageFieldId = new ID("{56012AA7-5275-4788-A5A2-E9C58584513E}");
            public static readonly ID ErrorMessageFieldId = new ID("{E86AA9F8-41DD-43A5-AEA4-0EF4B901527E}");
        }

        public class GetInTouchOtpDetailsTemplate
        {
            public static readonly ID GetInTouchOtpDetailsItemId = new ID("{D98B0D70-8533-4C98-80A6-5473344E1D3D}");

            public static readonly ID HeadingFieldId = new ID("{609FD1D6-6A8F-4F1A-B239-04E0BCCA7D49}");
            public static readonly ID WehavesentviaSMStoLabelFieldId = new ID("{F8000D20-9BE3-41C5-B3B5-F7C8E32B2E10}");
            public static readonly ID EditButtonLabelFieldId = new ID("{8B787009-1181-44BB-8D91-19F24D60064A}");
            public static readonly ID SubmitButtonTextFieldId = new ID("{AD6E6A4A-CD0C-4FA8-A804-95BCC475B1B9}");
            public static readonly ID ResendButtonLabelFieldId = new ID("{EF83760D-0D5C-4234-B003-D2B1662BC10B}");
            public static readonly ID NotReceivedOtpLabelFieldId = new ID("{44924124-E18D-4CEA-BB1E-7360C4AE28DC}");
            public static readonly ID YouWillReceiveOtpLabelFieldId = new ID("{299740C5-1678-4746-94AE-171EB69D1583}");
            public static readonly ID SecondLabelFieldId = new ID("{A2E41EDA-28E7-458E-9981-A1F941058AC9}");
            public static readonly ID EditMobileNOTitleFieldId = new ID("{21FF2742-AA9C-4EED-8ED8-5079B18C7A20}");
            public static readonly ID ResendOtpTitleFieldId = new ID("{62692142-B96C-4890-89ED-4AB25C858306}");
            public static readonly ID TimerFieldId = new ID("{B50E743C-0F0C-4CDF-9013-7EBF6ED82059}");
        }

        public class AmbujaFormDetailsTemplate
        {
            public static readonly ID AmbujaFormDetailsItemID = new ID("{C5D8FEA7-7160-4F60-99BC-3CED3B5DB186}");
            public static readonly ID LookingForOptionsFolderItemID = new ID("{4EDEE60B-FDBC-465C-B8B7-0537E3B28B1B}");
            public static readonly ID QueryTypeFolderItemID = new ID("{DDA83C00-C857-4B09-B840-540DA4EA682F}");
            public static readonly ID StatesFolderItemID = new ID("{44C6C91C-8E73-4260-B0AD-2A5DBA68552E}");
            public static readonly ID DistrictFolderItemID = new ID("{30D05FAA-DF78-4462-A369-CDA764475B00}");


            public static readonly ID DropdownOptionLabelFieldID = new ID("{AE0A74D8-FB60-421C-AA89-89ED8F28BB0C}");
            public static readonly ID DropdownOptionValueFieldID = new ID("{B12F2393-ABCC-418B-AB24-CD01F26BC889}");
            public static readonly ID DropdownOptionParentIdFieldID = new ID("{D3D6706C-B0F5-4CB8-AAB8-FD3499274F95}");

            public static readonly ID HeadingFieldID = new ID("{B2089CC8-51B8-49E6-8FED-16252D099FD9}");
            public static readonly ID DescriptionFieldID = new ID("{87E82310-2B75-4831-A1BD-D96360DC138F}");
            public static readonly ID SubmitButtonTextFieldID = new ID("{B7A8D03F-5D0F-432E-8039-359BABBDDDC6}");
            public static readonly ID CancelButtonTextFieldID = new ID("{29A3CA33-215C-4512-AFD5-FA06FAB4ECBE}");

            public static readonly ID RequiredFieldErrorMessageFieldID = new ID("{7C1F3909-1982-4BCF-BCA4-0617F4094B60}");
            public static readonly ID MaxLengthErrorMessageFieldID = new ID("{6BBCF789-85B9-476C-9890-3D4762585002}");
            public static readonly ID MinLengthErrorMessageFieldID = new ID("{FFE8FC5B-8856-4EF9-86FF-C0ABE1FA6F1B}");
            public static readonly ID RegexErrorMessageFieldID = new ID("{F3CCE8BA-421C-4BB8-9237-F90F08913010}");

            public static readonly ID RequiredNameFieldErrorMessageFieldID = new ID("{6A1152AC-E53F-4466-82A4-A43E0A72476B}");

            public static readonly ID RequiredMobileFieldErrorMessageFieldID = new ID("{3B176C71-AB13-4EA2-96F5-8DECC0C9E6C1}");

            public static readonly ID RequiredEmailFieldErrorMessageFieldID = new ID("{8CF2BE1A-4BD1-417B-B319-561320F3C320}");

            public static readonly ID RequiredLookingForFieldErrorMessageFieldID = new ID("{BDF9DD08-A9E2-4109-A4B7-70457B133DCA}");

            public static readonly ID RequiredQueryTypeFieldErrorMessageFieldID = new ID("{526D8EA0-7D9B-41FB-99D1-828AEBC0EDB0}");

            public static readonly ID RequiredStateFieldErrorMessageFieldID = new ID("{5E1FDF7A-A9C8-4B64-A978-81B0B671E1DF}");

            public static readonly ID RequiredDistrictFieldErrorMessageFieldID = new ID("{5D0129F0-EAA9-4506-AC30-D20128D044BD}");

            public static readonly ID RequiredCheckboxFieldErrorMessageFieldID = new ID("{2470E83C-4092-474B-92F5-36FC3B2442C9}");
        }

        public class FormFieldsSection
        {
            public static readonly ID PlaceholderText = new ID("{A6972F08-CBB5-4830-A8C9-8A0AE34650D1}");
            public static readonly ID minRequiredLength = new ID("{9F7DA002-7B86-436B-8DDA-7EFE5837B88E}");
            public static readonly ID maxAllowedLength = new ID("{71061C01-A495-4BF3-A52C-D378346BFAE2}");
            public static readonly ID DefaultValue = new ID("{B775BCA9-2605-4D60-B367-A0C354BE2504}");
            public static readonly ID Required = new ID("{8A43AF61-0812-4B37-A343-96CDE3F12BB1}");
            public static readonly ID CheckboxDefaultValue = new ID("{3C05A626-A3B1-4CBC-BF67-198537A13E20}");
            public static readonly ID Title = new ID("{71FFD7B2-8B09-4F7B-8A66-1E4CEF653E8D}");
            public static readonly ID DefaultSelection = new ID("{21ED172D-97E2-4AC4-8EAF-0A8860B891F4}");
            public static readonly ID fieldOptionslabel = new ID("{3A07C171-9BCA-464D-8670-C5703C6D3F11}");
            public static readonly ID ValueProviderParameters = new ID("{F6696439-2DAC-4183-8F3D-1979188D8336}");
        }

    }
}