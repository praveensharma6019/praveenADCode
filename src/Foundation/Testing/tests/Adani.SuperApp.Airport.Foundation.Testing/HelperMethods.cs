using System;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using NSubstitute;
using Sitecore.Collections;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.StringExtensions;

namespace Adani.SuperApp.Airport.Foundation.Testing
{
    public static class HelperMethods
    {
        public static Item CreateItem(Database database = null, ID templateId = null, ID itemId = null)
        {
            var db = database ?? Substitute.For<Database>();
            var targetItemId = itemId ?? ID.NewID;
            var targetTemplateId = templateId ?? ID.NewID;

            var def = Substitute.For<ItemDefinition>(targetItemId, String.Empty, targetTemplateId, ID.Null);
            var data = Substitute.For<ItemData>(def, Language.Current, Sitecore.Data.Version.First, new FieldList());
            var item = Substitute.For<Item>(targetItemId, data, db);
            var fieldCollection = Substitute.For<FieldCollection>(item);

            db.GetItem(item.ID).Returns(item);
            db.GetItem(item.ID.ToString()).Returns(item);
            item.Language.Returns(Language.Current);
            item.Version.Returns(Sitecore.Data.Version.First);
            item.TemplateID.Returns(targetTemplateId);
            item.Fields.Returns(fieldCollection);

            return item;
        }

        public static void SetItemField(Item item, string fieldName, string fieldValue)
        {
            var field = Substitute.For<Field>(ID.NewID, item);
            field.Value = fieldValue;
            item[fieldName].Returns(fieldValue);
            field.Database.Returns(item.Database);
            item.Fields[fieldName].Returns(field);
        }

        public static void SetItemField(Item item, ID fieldID, string fieldValue)
        {
            var field = Substitute.For<Field>(fieldID, item);
            field.Value = fieldValue;
            item[fieldID].Returns(fieldValue);
            field.Database.Returns(item.Database);
            item.Fields[fieldID].Returns(field);
        }

        public static void SetCheckBoxField(Item item, string fieldName, bool fieldValue)
        {
            var stringFieldValue = fieldValue ? "1" : "0";
            var field = Substitute.For<Field>(ID.NewID, item);
            field.Value = stringFieldValue;
            item[fieldName].Returns(stringFieldValue);
            item.Fields[fieldName].Returns(field);
        }
    }
}
