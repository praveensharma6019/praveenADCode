using Project.AdaniInternationalSchool.Website.Templates;
using Sitecore.Data.Items;
using System;
using System.Reflection;

namespace Project.AdaniInternationalSchool.Website.Helpers
{
    public static class AcademicServiceHelper<T>
    {
        public static T InitModelWithCommonFields(Item item)
        {
            T model = (T)Activator.CreateInstance(typeof(T));
            Type objType = model.GetType();

            PropertyInfo propType = objType.GetProperty("Type");
            propType.SetValue(model, item.TemplateName.ToLower());

            PropertyInfo propHeading = objType.GetProperty("Heading");
            propHeading.SetValue(model, Utils.GetValue(item, BaseTemplates.HeadingTemplate.HeadingFieldId, item.Name));

            PropertyInfo propDescription = objType.GetProperty("Description");
            propDescription.SetValue(model, Utils.GetValue(item, BaseTemplates.DescriptionTemplate.DescriptionFieldId));

            return model;
        }

        public static T InitModelWithCommonImgFields(Item item)
        {
            T model = (T)Activator.CreateInstance(typeof(T));
            Type objType = model.GetType();

            PropertyInfo propImageSource = objType.GetProperty("ImageSource");
            propImageSource.SetValue(model, Utils.GetImageURLByFieldId(item, BaseTemplates.ImageSourceTemplate.ImageSourceFieldId));

            PropertyInfo propImageSourceMobile = objType.GetProperty("ImageSourceMobile");
            propImageSourceMobile.SetValue(model, Utils.GetImageURLByFieldId(item, BaseTemplates.ImageSourceTemplate.ImageSourceMobileFieldId));

            PropertyInfo propImageSourceTablet = objType.GetProperty("ImageSourceTablet");
            propImageSourceTablet.SetValue(model, Utils.GetImageURLByFieldId(item, BaseTemplates.ImageSourceTemplate.ImageSourceTabletFieldId));

            PropertyInfo propImageAlt = objType.GetProperty("ImageAlt");
            propImageAlt.SetValue(model, Utils.GetValue(item, BaseTemplates.ImageSourceTemplate.ImageAltFieldId));

            return model;
        }
    }
}