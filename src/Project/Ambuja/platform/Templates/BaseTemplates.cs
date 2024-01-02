using Sitecore.Data;

namespace Project.AmbujaCement.Website.Templates
{
    public class BaseTemplates
    {
        public class ImageTemplate
        {
            public static readonly ID ImageFieldId = new ID("{6829185C-292D-4537-8F74-9CDEE2E0D79D}");
        }
        public class LogoSourceTemplate
        {
            public static readonly ID headerLogo = new ID("{A4D3AAF8-3B26-4562-AA76-A894F4BBA413}");
            public static readonly ID Logo = new ID("{605F0417-ADE4-4025-8700-67B4493692FF}");
            public static readonly ID LogoSrcHamburger = new ID("{4A19DC6E-F9ED-4F85-9F93-BF52AE0EE236}");
            public static readonly ID MobileLogo = new ID("{9BD0ABE0-A201-4256-9BA8-4A2508BC7741}");
            public static readonly ID LogoAlt = new ID("{516952AA-9843-48D0-A33A-BC360A0813FF}");
        }
        public class ImageSourceTemplate
        {
            public static readonly ID ImageSourceFieldId = new ID("{B27BA48D-45C4-488D-A433-830EA7241AFC}");
            public static readonly ID ImageSourceMobileFieldId = new ID("{AF599194-B424-4961-AB26-A043F98479FE}");
            public static readonly ID ImageSourceTabletFieldId = new ID("{4266333C-0491-4187-A1CB-D152CAC7B2B9}");
            public static readonly ID ImageAltFieldId = new ID("{412B576D-9369-453E-B100-3DE9A4C1E598}");
            public static readonly ID ImageTitleFieldId = new ID("{2AF85C78-3BCF-41AD-B152-53324DC0A744}");
        }

        public class FooterImageSourceTemplate
        {
            public static readonly ID ImageSourceFieldId = new ID("{21E8E999-45DF-4739-9625-CB21E9B7495C}");
            public static readonly ID ImageSourceMobileFieldId = new ID("{2016699D-DF17-47B0-96A7-38C3EA64F70E}");
            public static readonly ID ImageSourceTabletFieldId = new ID("{D2F68ABF-76C6-4E8C-BEC5-BEB468E3371C}");
            public static readonly ID ImageAltFieldId = new ID("{FB714145-72C6-4775-AFC8-06C645272455}");
        }

        public class VideoSourceTemplate
        {
            public static readonly ID VideoSourceFieldId = new ID("{6D406477-F4B7-4365-85CC-71C32C69E3A0}");
            public static readonly ID VideoSourceMobileFieldId = new ID("{17210717-1AA1-48A8-81F1-2F914220FAE9}");
            public static readonly ID VideoSourceTabletFieldId = new ID("{49C20006-4A99-4C76-AA78-C3C9B93FDE13}");
        }

        public class HeadingDetailsTemplate
        {
            public static readonly ID Heading = new ID("{AEF7AA36-A693-4A85-9A79-74E90ABF961C}");
            public static readonly ID Description = new ID("{EA82288C-5154-4095-A205-0E89A6F2AD55}");
        }

        public class DefaultVideoSourceTemplate
        {
            public static readonly ID DefaultVideoSourceFieldId = new ID("{B1766510-0EF8-4CE1-BF17-AD392176D670}");
            public static readonly ID DefaultVideoSourceMobileFieldId = new ID("{3E223EB3-6171-4993-83F5-3CB23B3D4E93}");
            public static readonly ID DefaultVideoSourceTabletFieldId = new ID("{A5ED6DF4-F4A6-489D-ACAC-D0F6821AF4C5}");
        }

        public class VideoSourceOggTemplate
        {
            public static readonly ID VideoSourceOggFieldId = new ID("{48B3B46E-D265-4A80-AC3A-8FB1CA1D90FB}");
            public static readonly ID VideoSourceOggMobileFieldId = new ID("{DFADAFF6-D354-49A3-B0C1-0703A4C431BE}");
            public static readonly ID VideoSourceOggTabletFieldId = new ID("{3ED22A99-235E-4DC0-A2D2-FAC9F840F788}");
        }

        public class DefaultVideoSourceOggTemplate
        {
            public static readonly ID DefaultVideoSourceOggFieldId = new ID("{0007B352-168B-4F98-9F55-D5EB23D02EDC}");
            public static readonly ID DefaultVideoSourceOggMobileFieldId = new ID("{E0534326-4C21-47F1-A09A-7F2CCC812F7F}");
            public static readonly ID DefaultVideoSourceOggTabletFieldId = new ID("{243F1A08-8B9B-4402-9A49-36E46E475284}");
        }

        public class CtaTemplate
        {
            public static readonly ID CtaTextFieldId = new ID("{9E02813C-CD00-4F55-88CD-BF75E77D66B1}");
            public static readonly ID CtaLinkFieldId = new ID("{6868FE5B-C7E0-43C4-8515-0DF6234020A5}");
            public static readonly ID CtaLinkTargetFieldId = new ID("{7529CB93-A9EA-46D1-AD5C-F62B45F8A696}");
        }

        public class DescriptionTemplate
        {
            public static readonly ID DescriptionFieldId = new ID("{EA82288C-5154-4095-A205-0E89A6F2AD55}");
            public static readonly ID SubDescriptionFieldId = new ID("{8710E4E8-5371-4A97-8B5A-C883A0C1F26F}");
        }

        public class DealerDetailsTemplate
        {
            public static readonly ID Id = new ID("{41B342CB-E343-495C-917A-0AEB3A432281}");
            public static readonly ID Label = new ID("{20B6D9BB-ECD8-46D0-A72B-1EDC3160D342}");
            public static readonly ID Type = new ID("{6F9D9B45-C1D3-4624-9BB3-380E769543C4}");
        }
        public class CityDealerDetailsTemplate
        {
            public static readonly ID Id = new ID("{FE74E61B-26C4-491E-895F-195F78F07E18}");
            public static readonly ID Label = new ID("{9AF85D37-E667-4D99-AACF-1932F79D1C63}");
            public static readonly ID Type = new ID("{90159981-EFC5-42E2-B1EA-B700A46CCF28}");
            public static readonly ID Area = new ID("{29ECA56B-C8F9-4DCE-8750-0119675FE9A4}");
        }
        public class StateDealerDetailsTemplate
        {
            public static readonly ID Id = new ID("{0FB31A54-BC80-43AF-BF41-D7E0DB34E493}");
            public static readonly ID Label = new ID("{42F5CC5E-F9F4-45C1-A70D-7F1E902BA24C}");
            public static readonly ID Type = new ID("{6FC8D676-3EFE-41CA-93E9-BC788476A224}");
            public static readonly ID City = new ID("{54FEEDF4-ACD6-4471-AF62-61450F0EFA5F}");
        }

        public class VariantTemplate
        {
            public static readonly ID VariantFieldId = new ID("{E23E26B1-6871-41CA-85A9-15FFF9BF5019}");
        }
        public class SectionIdTemplate
        {
            public static readonly ID SectionIdFieldId = new ID("{434485EF-FE09-4440-8EF6-5E093A6723D5}");
        }

        public class HeadingTemplate
        {
            public static readonly ID HeadingFieldId = new ID("{AEF7AA36-A693-4A85-9A79-74E90ABF961C}");
        }

        public class NoDataTemplate
        {
            public static readonly ID NoDataFieldId = new ID("{7979BF1D-E537-48A9-A790-158746ED1A7D}");
        }

        public class TitleTemplate
        {
            public static readonly ID TitleFieldId = new ID("{B7F8C0BE-39EA-41F5-90C4-A88B3C6C6DF8}");
            public static readonly ID SubTitleFieldID = new ID("{C8182207-6344-46C7-BFDD-0ADCB80ECD1B}");
        }
        public class SubHeadingTemplate
        {
            public static readonly ID SubHeadingFieldId = new ID("{D50DB906-81BA-4ECB-B727-57002B9022B0}");
        }
        public class ThemeTemplate
        {
            public static readonly ID ThemeFieldId = new ID("{782B0141-674E-41E2-BA9C-641E26B56957}");
        }

        public class LabelTemplate
        {
            public static readonly ID LabelFieldId = new ID("{907D900F-9002-4D60-A06B-49BCC1318357}");
            public static readonly ID defaultActiveTab = new ID("{3CBB640E-4A6D-4A63-B3F5-6E928112C8CC}");
            public static readonly ID defaultActiveTemplate = new ID("{FD27B064-0E3E-4450-A48F-5F92126FF0EB}");
        }

        public class DateTemplate
        {
            public static readonly ID DateFieldId = new ID("{1BA3F628-461C-4375-8099-B88C05F9CC31}");
        }

        public class LinkTemplate
        {
            public static readonly ID LinkFieldId = new ID("{7BE690ED-01D2-4F6B-8A18-7C2D58AD2AFF}");
            public static readonly ID LinkTextFieldId = new ID("{A411F701-5EFD-4886-87AF-2A846BD18AFA}");
            public static readonly ID LinkTargetFieldId = new ID("{2E795C0F-28F1-4A3C-8C80-6665B4ABA7F7}");
            public static readonly ID IsActive = new ID("{DB733E75-FEA7-41E0-8C80-642B202DC80D}");
        }

        public class RichTextTemplate
        {
            public static readonly ID RichTextFieldID = new ID("{2374DF20-D9AA-49B2-A65A-258D24383A3D}");
        }

        public class ActiveCheckboxTemplate
        {
            public static readonly ID ActiveFieldID = new ID("{9E0A0CFE-FC9E-4AAB-B08D-3C611450A4DD}");
        }

        public class TargetTemplate
        {
            public static readonly ID TargetFieldId = new ID("{2B9A15F6-14D4-4280-8D4D-140C0FFB3399}");
        }

        public class TypeTemplate
        {
            public static readonly ID TypeFieldId = new ID("{D5113037-ACE6-4806-B344-4C79C959341F}");
        }

        public class GTMTemplate
        {

            public static readonly ID GtmEventFieldId = new ID("{C35C757D-CF8C-4162-801A-9315B612FBAC}");
            public static readonly ID GtmCategoryFieldId = new ID("{FCBD53CB-90CB-4943-B71C-D351B98F628D}");
            public static readonly ID GtmSubCategoryFieldId = new ID("{DED14A3D-EE1D-4B1F-85A6-7EF825CFAF6A}");
            public static readonly ID GtmBannerCategoryFieldId = new ID("{6E0872C9-B6E3-4E14-9D50-95EA41684486}");
            public static readonly ID GtmIndexFieldId = new ID("{FD4B45C7-80E2-4957-9502-97665362848E}");
            public static readonly ID GtmLabelFieldId = new ID("{199F7F2A-AF83-4807-87AB-2BE654629755}");
            public static readonly ID PageTypeFieldId = new ID("{EF47B038-9409-41F2-941F-21426387A79B}");
            public static readonly ID VideoDurationFieldId = new ID("{2717320A-756E-4846-859A-70AF3295B638}");
            public static readonly ID GtmVideoStartEventFieldId = new ID("{E7885034-71CF-4500-8ACA-32BBE6AA68F4}");
            public static readonly ID GtmVideoCompletedEventFieldId = new ID("{AF85D214-9F51-4AD5-BFD6-F74C3D9879DB}");
            public static readonly ID GtmVideoProgressEventFieldId = new ID("{AC4338CB-CC46-4FCB-9FA5-3F279C99B0F5}");
            public static readonly ID SeoNameFieldId = new ID("{89C03BE6-2189-4BF6-96CA-6AAA7EE01155}");
            public static readonly ID SeoDescriptionFieldId = new ID("{2C3EDE88-D9B8-49ED-88C1-BD0956482C29}");
            public static readonly ID UploadDateFieldId = new ID("{7E09DD8E-4EF7-4845-ADD5-3FA394A6C1E2}");
            public static readonly ID GtmTitleFieldId = new ID("{05E16BF6-D387-452B-ADE1-F2763EACA635}");
            public static readonly ID GtmSectionTitleFieldId = new ID("{5850FF40-D952-4AE0-9F61-53A08D19BE3B}");
            public static readonly ID GtmVideoactionFieldId = new ID("{24CD61C0-75E8-42F9-B5B3-7A42AF4719F9}");
            public static readonly ID GtmVideodurationFieldId = new ID("{2717320A-756E-4846-859A-70AF3295B638}");
            public static readonly ID GtmVideopercentFieldId = new ID("{AC4338CB-CC46-4FCB-9FA5-3F279C99B0F5}");
            public static readonly ID GtmClicktextFieldId = new ID("{2DAE5866-FD80-4365-A389-1AB9FE3966AE}");

            public static readonly ID GtmDataFieldId = new ID("{677FAC76-3166-48DD-BD3C-2E687F58237A}");
            public static readonly ID GtmVideoStartFieldId = new ID("{255F044A-4C54-4B27-996D-B8B4B763B6E0}");
            public static readonly ID GtmVideoProgressFieldId = new ID("{C098B7FA-BE93-40A5-9101-28B0BD5C13A5}");
            public static readonly ID GtmVideoCompleteFieldId = new ID("{5449B64E-9C7C-43C6-8099-2FE4E743FDD0}");


            public static readonly ID GtmDataWelcomeCardFieldId = new ID("{59B06917-4BD4-453C-83A3-263BD9547AD5}");
            public static readonly ID GtmVideoStartWelcomeCardFieldId = new ID("{0A9C35F2-7153-49E0-BFDB-DB017B6BEA10}");
            public static readonly ID GtmVideoProgressWelcomeCardFieldId = new ID("{02CBC469-5477-44BB-88D2-C76F7CA63A86}");
            public static readonly ID GtmVideoCompleteWelcomeCardFieldId = new ID("{0B1F95CA-610B-4F8B-AAA1-0D0DEBB8F5C3}");

        }

        public class BackLabelTemplate
        {
            public static readonly ID BackLabelFieldId = new ID("{A0799882-B6DF-41F1-BB59-6FA61F5F885A}");
        }

        public class ThumbnailImageTemplate
        {
            public static readonly ID ThumbnailImageSourceFieldId = new ID("{A778BDEF-419E-403B-B143-175F83FCC210}");
            public static readonly ID ThumbnailImageSourceMobileFieldId = new ID("{5CDE4435-1A7A-4DDE-82F6-6F435D5234EC}");
            public static readonly ID ThumbnailImageSourceTabletFieldId = new ID("{5BA13FF0-447F-4D79-A67E-2920443A4199}");
            public static readonly ID ThumbnailImageAltFieldId = new ID("{591A2B66-82E2-4428-8E13-2111855CC681}");
        }

        public class SectionIDTemplate
        {
            public static readonly ID SectionIDFieldId = new ID("{434485EF-FE09-4440-8EF6-5E093A6723D5}");
        }

        public class TextFirstTemplate
        {
            public static readonly ID TextFirstFieldId = new ID("{CC913CEC-BE6F-4D2B-A273-99063D67A052}");
        }
        public class TextTemplate
        {
            public static readonly ID TextFieldId = new ID("{75E37181-41AD-4A4C-BA4B-8C6C40244AA3}");
        }
        public class MapSourceTemplate
        {
            public static readonly ID MapSourceFieldId = new ID("{55E7B346-7646-44F8-B01B-7EE4A9F4EB92}");
        }

        public class MediaTypeTemplate
        {
            public static readonly ID MediaTypeFieldId = new ID("{96205169-A0FC-4DF2-9732-CC576C7D2510}");
        }

        public class CardTypeTemplate
        {
            public static readonly ID CardTypeFieldId = new ID("{762E69E8-C338-461C-AE3C-C98ECB9E8869}");
        }

        public class AutoPlayTemplate
        {
            public static readonly ID AutoPlayFieldId = new ID("{32FAE09F-3555-4413-AF5A-0A142373AA79}");

        }

        public class OverlayRequiredTemplate
        {
            public static readonly ID IsOverlayRequiredFieldId = new ID("{6BC19084-E512-45CD-9014-5E5D0ED2CD8E}");
        }

        public class TargetItemPathTemplate
        {
            public static readonly ID TargetItemPathFieldId = new ID("{60C99B84-1CD1-4B22-8D94-B46F2B28DF04}");
        }
        public class HeaderFieldTemplate
        {
            public static readonly ID HeaderLeftIcon = new ID("{D424172A-A5D6-43BC-B850-92F6FAA8244C}");
            public static readonly ID HeaderRightIcon = new ID("{CA56D9AD-C6F5-4392-BD8E-B57B363A3C39}");
            public static readonly ID HighlightLabel = new ID("{C61345CD-B5B8-405C-AA74-C140ECC21A9F}");
            public static readonly ID ItemRightIcon = new ID("{E739B9FF-AEE8-413F-8549-2BB7410B1E9D}");
            public static readonly ID ItemSubText = new ID("{A2C280F2-12F5-4456-99A0-24182F144399}");
            public static readonly ID IsActive = new ID("{D4815D8D-71D1-443F-9473-D8B82ECA1114}");

        }
        public class ActiveIdTemplate
        {
            public static readonly ID ActiveFieldId = new ID("{476491F9-C231-478D-B408-9B755B3ADFD2}");
        } 
        public class IsAbsoluteTemplate
        {
            public static readonly ID IsAbsoluteFieldId = new ID("{7C725AE1-B780-476F-8FD2-7799197E6F30}");
        }
        public class IsOffcanvasIdTemplate
        {
            public static readonly ID IsOffcanvasFieldId = new ID("{298F9FF9-CB4C-478F-931C-93B85A771DE5}");
        }
        public class IconclassIdTemplate
        {
            public static readonly ID IconFieldId = new ID("{298F9FF9-CB4C-478F-931C-93B85A771DE5}");
        }
        public class IConItemPathTemplate
        {
            public static readonly ID IconImgeFieldId = new ID("{9C303642-98CA-48F0-B2D2-AE700A8EC88E}");
            public static readonly ID IconalttextFieldId = new ID("{60A5A775-07D8-4D78-AA30-5642E2A85473}");
            public static readonly ID IconclassFieldId = new ID("{35886F11-B46D-4EF2-B1A0-0BAAC79CDA0E}");
            public static readonly ID IconnameFieldId = new ID("{4378676E-CB60-4EC1-9F57-27EDABA9B4AD}");
        }
        public static class CommonItemlist
        {
            public static readonly ID ItemLeftIcon = new ID("{0C7B3DDC-39BB-4E3F-86E6-B125EB310A81}");
            public static readonly ID IconID = new ID("{BD9F76AB-DD25-41EE-9DDF-76109FCB0265}");
            public static readonly ID IconnamefieldID = new ID("{4378676E-CB60-4EC1-9F57-27EDABA9B4AD}");

        }
    }
}
