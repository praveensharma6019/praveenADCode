using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using Adani.SuperApp.Airport.Feature.DutyFreeProductImport.Platform.Models;
using Adani.SuperApp.Airport.Foundation.DataAPI.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Airport.Feature.DutyFreeProductImport.Platform.Command
{
    public class ProductImport
    {
        LogRepository _logRepository = new LogRepository();

        Helper _helper = new Helper();

        public enum Restricted {[EnumMember(Value = "Liquor")] MaterialGroup };


        public void Execute(Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {
            _logRepository.Info("My Sitecore scheduled task is being run!");
            int page = 6; int prodCount = 1;
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            while (prodCount > 0)
            {
                parameters.Clear();
                parameters.Add("page", page.ToString());
                prodCount = ReadProductJson(parameters);

                page = page + 1;
            }

            //    prodCount = UpdateProductsFromJSON();

            _logRepository.Info(" Import product from JSON  ->" + prodCount.ToString());
        }

        private int ReadProductJson(Dictionary<string, string> parameters)
        {

            APIResponse aPIResponse = new APIResponse();
            _logRepository.Info("get Products form ProductImportAPI");
            var response = aPIResponse.GetAPIResponse("GET", Sitecore.Configuration.Settings.GetSetting("ProductImportAPI"), null, parameters, "");
            APIresult aPIresult = JsonConvert.DeserializeObject<APIresult>(response);
            CreateProduct(aPIresult.data);

            return aPIresult.data.Count;
        }


        private void UpdateProductImagesPATHTemp()
        {
            Sitecore.Data.Database contextDB = GetContextDatabase();
            Item allProductsFolder = contextDB.GetItem(new ID("{11DFEA97-5FDD-4ECE-9623-1189B9AE91C5}"));

            var allProducts = allProductsFolder.Axes.GetDescendants().Where(p => p.TemplateID.Equals(Constant.templateId));

            foreach (Item newItem in allProducts)
            {
                newItem.Editing.BeginEdit();

                //  imagePath = uploadProductImage(product, product.image1);
                string imagePath = !string.IsNullOrEmpty(newItem[Constant.Image1]) ? newItem[Constant.Image1] : string.Empty;
                if (!string.IsNullOrEmpty(imagePath) && imagePath.IndexOf("Dutyfree/ProductImages") >= 0)
                {
                    // newItem[Constant.Image1] = imagePath;
                    newItem[Constant.Image1] = imagePath.IndexOf("/sitecore/") < 0 ? "/sitecore/media library" + imagePath : imagePath;
                }

                //imagePath = uploadProductImage(product, product.image2);
                imagePath = !string.IsNullOrEmpty(newItem[Constant.Image2]) ? newItem[Constant.Image2] : string.Empty;
                if (!string.IsNullOrEmpty(imagePath) && imagePath.IndexOf("Dutyfree/ProductImages") >= 0)
                {
                    //newItem[Constant.Image2] = imagePath;
                    newItem[Constant.Image2] = imagePath.IndexOf("/sitecore/") < 0 ? "/sitecore/media library" + imagePath : imagePath;
                }
                newItem.Editing.EndEdit();
            }
        }

        private void CreateProduct(List<Product> jsonObject)
        {

            if (jsonObject.Any(p => p.id > 4821 && p.id < 4846))
            {
                Sitecore.Data.Database contextDB = GetContextDatabase();
                _logRepository.Info("Gets the context database->" + contextDB.ToString());
                Sitecore.Data.Items.Item parentItem = contextDB.GetItem(Constant.productParentItemId);
                _logRepository.Info("Gets the Parent Item under which product need to be created->" + parentItem.ID.ToString());
                var productTemplate = contextDB.GetTemplate(Constant.templateId);
                _logRepository.Info("Gets the Template for product creation->" + productTemplate.ID.ToString());
                try
                {
                    _logRepository.Info("Checks for the null of Parent folder and the template");
                    if (parentItem != null && productTemplate != null)
                    {

                        _logRepository.Info("Checks passed for the Parent folder and the template");
                        using (new Sitecore.SecurityModel.SecurityDisabler())
                        {

                            _logRepository.Info("Disabeling Sitecore Security for content creation");
                            _logRepository.Info("Number of product to be imported -> " + jsonObject.Count);
                            string sku = "22A07874,22A05571";
                            int countCreated = 0;
                            int countUpdate = 0;
                            foreach (var product in jsonObject)
                            {
                                _logRepository.Info("Product importing start 6April -->" + product.id);

                                if (product.id >= 4822 && product.id <= 4845 )
                                {

                                    Item newItem = null;
                                    try
                                    {
                                        _logRepository.Info("Checks if the item already exists ->" + product.id);
                                        // newItem = GetExistingItemBasedOnLanguage("/sitecore/content/AirportHome/Datasource/Adani/Dutyfree/Products/" + product.flemingo_sku_code);
                                        string basePath = "/sitecore/content/AirportHome/Datasource/Adani/Dutyfree/Products/" + _helper.Sanitize(product.material_group_name).ToLower() + "/" + _helper.Sanitize(product.category_name).Replace(",", "").ToLower();
                                        newItem = GetExistingItemBasedOnLanguage(basePath + "/" + product.flemingo_sku_code);
                                        parentItem = GetProductCategoryFolderItem(_helper.Sanitize(product.material_group_name).ToLower(), _helper.Sanitize(product.category_name).Replace(",", "").ToLower());
                                        // newItem = GetSitecoreProductItem(_helper.Sanitize(product.material_group_name).ToLower(), _helper.Sanitize(product.category_name).ToLower(), product.flemingo_sku_code);
                                        //newItem = newItem == null ? parentItem.Add(product.flemingo_sku_code, productTemplate) : newItem;
                                        if (newItem == null)
                                        {
                                            newItem = parentItem.Add(product.flemingo_sku_code, productTemplate);
                                            countCreated = CreateSitecoreProduct(newItem, product, countCreated);
                                            countCreated = countCreated + 1;
                                        }
                                        else
                                        {
                                            countUpdate = UpdateSitecoreProduct(newItem, product, countUpdate);
                                            countUpdate = countUpdate + 1;
                                        }
                                        _logRepository.Info("Item Id of the item to be added -> " + product.id.ToString());

                                    }
                                    catch (Exception ex)
                                    {
                                        _logRepository.Error("Item creation failed due to -> " + ex.Message);
                                    }
                                    _logRepository.Info("Product importing End ->" + product.id);

                                }
                            }

                            _logRepository.Info("Product created 6April --> " + countCreated);
                            _logRepository.Info("Product updated 6April --> " + countCreated);
                        }


                    }
                }
                catch (Exception ex)
                {

                    _logRepository.Error("CreateProduct method failed due to -> " + ex.Message);
                }
            }
        }

        private Item AddUpdateBucketName(ref Database contextDB, string bucket_name)
        {
            string bucketNamePath = "/sitecore/content/AirportHome/Datasource/Adani/Dutyfree/Groups/" + _helper.Sanitize(bucket_name);
            Item bucketNameItem = GetExistingItem(bucketNamePath);
            try
            {
                if (bucketNameItem == null)
                {
                    Item bucketGroupFolderItem = contextDB.GetItem(Constant.BucketFolderTemplateId);
                    TemplateItem bucketTemplateItem = contextDB.GetTemplate(Constant.BucketTemplateId);
                    if (!string.IsNullOrEmpty(bucket_name) && bucketTemplateItem != null)
                    {

                        Item bucketItem = bucketGroupFolderItem.Add(_helper.Sanitize(bucket_name), bucketTemplateItem);

                        using (new EditContext(bucketItem))
                        {
                            bucketItem.Appearance.DisplayName = bucket_name;
                            bucketItem[Constant.BucketFieldName] = _helper.Sanitize(bucket_name);
                        }
                        PublishProduct(bucketItem);
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error("AddUpdateBucketName method failed due to -> " + ex.Message);
            }

            return bucketNameItem;
        }

        private void PublishProduct(Sitecore.Data.Items.Item item)
        {
            _logRepository.Info("Product Publish Started ");
            try
            {
                Sitecore.Data.Database master = Sitecore.Configuration.Factory.GetDatabase(Constant.master);
                _logRepository.Info("Gets the master/Source database ");
                Sitecore.Data.Database web = Sitecore.Configuration.Factory.GetDatabase(Constant.web);
                _logRepository.Info("Gets the Web/Target database ");
                Sitecore.Publishing.PublishOptions publishOptions = new Sitecore.Publishing.PublishOptions(master,
                                        web,
                                        Sitecore.Publishing.PublishMode.SingleItem,
                                        item.Language,
                                        System.DateTime.Now);
                _logRepository.Info("Publish started -> " + System.DateTime.Now.ToString());
                Sitecore.Publishing.Publisher publisher = new Sitecore.Publishing.Publisher(publishOptions);
                _logRepository.Info("Set publish Options -> ");
                publisher.Options.RootItem = item;
                _logRepository.Info("Set Root Item ");
                publisher.Options.Deep = true;

                publisher.Publish();
                _logRepository.Info("Item Published ");
            }
            catch (Exception ex)
            {

                _logRepository.Error(" Item Published Failed due to -> " + ex.Message);
            }

        }

        private Sitecore.Data.Database GetContextDatabase()
        {
            return Sitecore.Context.ContentDatabase;
        }

        private Sitecore.Data.Items.Item GetExistingItem(string ItemPath)
        {
            return Sitecore.Context.ContentDatabase.GetItem(ItemPath);
        }
        private Sitecore.Data.Items.Item GetExistingItemBasedOnLanguage(string ItemPath)
        {
            return Sitecore.Context.ContentDatabase.GetItem(ItemPath, Sitecore.Context.Language);
        }

        private Sitecore.Data.Items.Item GetSitecoreProductItem(string materialGroup, string category, string skuCode)
        {
            Item product = null;
            string basePath = "/sitecore/content/AirportHome/Datasource/Adani/Dutyfree/Products";
            string materialGroupPath = basePath + "/" + materialGroup;
            string categoryPath = "/" + materialGroupPath + "/" + category;

            Item materialGroupFolderItem = Sitecore.Context.ContentDatabase.GetItem(materialGroupPath, Sitecore.Context.Language);
            if (materialGroupFolderItem == null)
            {
                materialGroupFolderItem = CreateFolder(basePath, materialGroup, Constant.FolderTemplateId);
            }
            Item categotyFolderItem = Sitecore.Context.ContentDatabase.GetItem(categoryPath, Sitecore.Context.Language);

            if (categotyFolderItem == null)
            {
                categotyFolderItem = CreateFolder(materialGroupPath, category, Constant.FolderTemplateId);
            }

            if (materialGroupFolderItem != null && categotyFolderItem != null)
            {
                product = Sitecore.Context.ContentDatabase.GetItem(categoryPath + "/" + skuCode, Sitecore.Context.Language);
            }

            return product;
        }

        private Item CreateFolder(string basePath, string name, ID template)
        {
            Item folderItem = null;
            Item parrentItem = Sitecore.Context.ContentDatabase.GetItem(basePath, Sitecore.Context.Language);
            if (parrentItem != null)
            {
                folderItem = parrentItem.Add(name, Sitecore.Context.ContentDatabase.GetTemplate(template));
            }

            return folderItem;
        }

        internal Sitecore.Data.Items.Item CreateUpdateBrand(ref Sitecore.Data.Database contextDB, string brandName, string materialGroup)
        {
            Item brandItem = GetExistingItem("/sitecore/content/AirportHome/Datasource/Adani/Dutyfree/Products/Navigation/Brands/" + _helper.Sanitize(brandName));

            Item brandParentItem = contextDB.GetItem(Constant.BrandParentItemId);
            var brandTemplate = contextDB.GetTemplate(Constant.BrandtemplateId);
            try
            {
                if (brandParentItem != null && brandTemplate != null)
                {
                    brandItem = (brandItem == null) ? brandParentItem.Add(_helper.Sanitize(brandName).ToLower(), brandTemplate) : brandItem;
                    using (new EditContext(brandItem))
                    {
                        brandItem.Appearance.DisplayName = brandName;
                        brandItem[Constant.BrandCode] = _helper.Sanitize(brandName).ToLower();
                        brandItem[Constant.BrandName] = brandName;
                        brandItem[Constant.BrandMaterialGroup] = _helper.Sanitize(materialGroup.Trim().ToLower());
                        if (materialGroup.Equals("Liquor"))
                        {
                            Sitecore.Data.Fields.CheckboxField restrictedBrand = brandItem.Fields[Constant.BrandRestricted];
                            restrictedBrand.Checked = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error("Create Brand method failed due to -> " + ex.Message);

            }

            return brandItem;
        }

        internal void UpdateCategory(ref Sitecore.Data.Database contextDB, string materialGroup, string category, string brand, string subCategory)
        {
            Item brandItem = CreateUpdateBrand(ref contextDB, brand, materialGroup);
            string categoryPath = "sitecore/content/AirportHome/Datasource/Adani/Dutyfree/Products/Navigation/Material Group/" + _helper.Sanitize(materialGroup) + "/" + _helper.Sanitize(category);

            Item materialgroupItem = contextDB.GetItem("sitecore/content/AirportHome/Datasource/Adani/Dutyfree/Products/Navigation/Material Group/" + _helper.Sanitize(materialGroup));

            Item categoryItem = contextDB.GetItem(categoryPath);

            if (categoryItem == null)
            {
                TemplateItem categoryTemplate = contextDB.GetTemplate(Constant.CategoryTemplateId);
                if (!string.IsNullOrEmpty(category) && categoryTemplate != null)
                {
                    categoryItem = materialgroupItem.Add(_helper.Sanitize(category), categoryTemplate);

                    using (new EditContext(categoryItem))
                    {
                        categoryItem.Appearance.DisplayName = category;
                        categoryItem[Constant.SubCategoryCode] = _helper.Sanitize(category).ToLower();
                        categoryItem[Constant.SubCategoryName] = category;
                    }
                }
            }


            AddUpdateSubCategory(ref contextDB, ref categoryItem, ref categoryPath, ref brandItem, subCategory);


            if (!categoryItem.Fields[Constant.BrandsFieldName].Value.ToString().Contains(brandItem.ID.ToString()))
            {
                using (new EditContext(categoryItem))
                {
                    MultilistField brandsField = new MultilistField(categoryItem.Fields[Constant.BrandsFieldName]);
                    brandsField.Add(brandItem.ID.ToString());
                }
            }

        }


        internal void AddUpdateSubCategory(ref Sitecore.Data.Database contextDB, ref Item categoryItem, ref string categoryPath, ref Item brandItem, string subCategory)
        {
            try
            {
                if (!string.IsNullOrEmpty(subCategory))
                {
                    Item subCategoryItem = GetExistingItem(categoryPath + "/" + _helper.Sanitize(subCategory));

                    if (subCategoryItem == null)
                    {
                        TemplateItem subCategoryTemplate = contextDB.GetTemplate(Constant.SubCategoryTemplateId);
                        if (!string.IsNullOrEmpty(subCategory) && subCategoryTemplate != null)
                        {
                            subCategoryItem = categoryItem.Add(_helper.Sanitize(subCategory).ToLower(), subCategoryTemplate);

                            using (new EditContext(subCategoryItem))
                            {
                                subCategoryItem.Appearance.DisplayName = subCategory;
                                subCategoryItem[Constant.SubCategoryCode] = _helper.Sanitize(subCategory).ToLower();
                                subCategoryItem[Constant.SubCategoryName] = subCategory;
                            }
                        }
                    }

                    if (!subCategoryItem.Fields[Constant.BrandsFieldName].Value.ToString().Contains(brandItem.ID.ToString()))
                    {
                        using (new EditContext(subCategoryItem))
                        {
                            MultilistField brandsField = new MultilistField(subCategoryItem.Fields[Constant.BrandsFieldName]);
                            brandsField.Add(brandItem.ID.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error("AddUpdateSubCategory method failed due to -> " + ex.Message);

            }
        }

        internal Item CreateOffers(ref Sitecore.Data.Database contextDB, Promotion promotion, Product product)
        {
            Item offerItem = null;

            Item offerParentItem = contextDB.GetItem(Constant.OfferParentItemId);  // {E1337B98-22CB-4E08-95C0-A206EA8DAF36} 
            var offerTemplate = contextDB.GetTemplate(Constant.OffertemplateId);   // {ABEA7A9C-8827-4B28-9792-D70E2406A728}
            try
            {
                if (offerParentItem != null && offerTemplate != null)
                {
                    offerItem = offerParentItem.Add(_helper.Sanitize(promotion.promotion_code), offerTemplate);
                    using (new EditContext(offerItem))
                    {
                        offerItem.Appearance.DisplayName = promotion.promotion_code + " " + promotion.offer_display_text;
                        offerItem[Constant.OfferCode] = _helper.Sanitize(promotion.promotion_code);
                        offerItem[Constant.OfferTitle] = promotion.offer_display_text;
                        offerItem[Constant.OfferMaterialGroup] = _helper.Sanitize(product.material_group_name);
                        offerItem[Constant.OfferCategory] = _helper.Sanitize(product.category_name);
                        offerItem[Constant.OfferSubCategory] = _helper.Sanitize(product.sub_category_name);
                        Item brandItem = GetExistingItem("/sitecore/content/AirportHome/Datasource/Adani/Dutyfree/Products/Navigation/Brands/" + _helper.Sanitize(product.brand_name));
                        MultilistField brandsField = new MultilistField(offerItem.Fields[Constant.OfferBrand]);
                        if (brandItem != null)
                        {
                            brandsField.Add(brandItem.ID.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error("Create Offer method failed due to -> " + ex.Message);

            }

            return offerItem;
        }



        private Sitecore.Data.Items.Item GetProductCategoryFolderItem(string materialGroup, string category)
        {
            Item categotyFolderItem = null;
            string basePath = "/sitecore/content/AirportHome/Datasource/Adani/Dutyfree/Products";
            string materialGroupPath = basePath + "/" + materialGroup;
            string categoryPath = "/" + materialGroupPath + "/" + category;

            Item materialGroupFolderItem = Sitecore.Context.ContentDatabase.GetItem(materialGroupPath, Sitecore.Context.Language);
            if (materialGroupFolderItem == null)
            {
                materialGroupFolderItem = CreateFolder(basePath, materialGroup, Constant.FolderTemplateId);
            }
            categotyFolderItem = Sitecore.Context.ContentDatabase.GetItem(categoryPath, Sitecore.Context.Language);

            if (categotyFolderItem == null)
            {
                categotyFolderItem = CreateFolder(materialGroupPath, category, Constant.FolderTemplateId);
            }

            return categotyFolderItem;
        }

        private Sitecore.Data.Items.Item GetProductImageFolderItem(string materialGroup, string category, string brand, string skuCode)
        {
            Item productImageFolderItem = null;
            string basePath = "/sitecore/media library/Foundation/Adani/Dutyfree/ProductImages";
            string materialGroupPath = basePath + "/" + materialGroup;
            string categoryPath = "/" + materialGroupPath + "/" + category;
            string brandPath = "/" + materialGroupPath + "/" + category + "/" + brand;
            string ProductPath = "/" + materialGroupPath + "/" + category + "/" + brand + "/" + skuCode;

            Item materialGroupImageFolderItem = Sitecore.Context.ContentDatabase.GetItem(materialGroupPath, Sitecore.Context.Language);
            if (materialGroupImageFolderItem == null)
            {
                materialGroupImageFolderItem = CreateFolder(basePath, materialGroup, Constant.MediaFolderTemplateId);
            }
            Item categotyImageFolderItem = Sitecore.Context.ContentDatabase.GetItem(categoryPath, Sitecore.Context.Language);

            if (categotyImageFolderItem == null)
            {
                categotyImageFolderItem = CreateFolder(materialGroupPath, category, Constant.MediaFolderTemplateId);
            }

            Item brandImageFolderItem = Sitecore.Context.ContentDatabase.GetItem(brandPath, Sitecore.Context.Language);

            if (brandImageFolderItem == null)
            {
                brandImageFolderItem = CreateFolder(categoryPath, brand, Constant.MediaFolderTemplateId);
            }

            productImageFolderItem = Sitecore.Context.ContentDatabase.GetItem(ProductPath, Sitecore.Context.Language);

            if (productImageFolderItem == null)
            {
                productImageFolderItem = CreateFolder(brandPath, skuCode, Constant.MediaFolderTemplateId);
            }

            return productImageFolderItem;
        }

        private int CreateSitecoreProduct(Item newItem, Product product, int countCreate)
        {
            List<string> imageFile = null;
            Sitecore.Data.Database contextDB = GetContextDatabase();
            try
            {
                if (newItem != null)
                {
                    UpdateCategory(ref contextDB, product.material_group_name, product.category_name, product.brand_name.Replace("â€™", "'"), product.sub_category_name);
                   
                    newItem.Editing.BeginEdit();

                    newItem.Appearance.DisplayName = product.material_group_name + " " + product.category_name + " " + product.sku_name.Replace("â€™", "'") + " " + product.flemingo_sku_code;
                    // SKU Information 
                    newItem[Constant.SKUID] = !string.IsNullOrEmpty(product.id.ToString()) ? product.id.ToString() : string.Empty;
                    // _logRepository.Info("SKU ID Added to the item -> " + product.id.ToString());
                    newItem[Constant.SKUName] = !string.IsNullOrEmpty(product.sku_name.ToString()) ? product.sku_name.ToString().Replace("â€™", "'") : String.Empty;
                    //_logRepository.Info("SKU Name Added to the item -> " + product.sku_name.ToString());

                    newItem[Constant.SKUDescription] = !string.IsNullOrEmpty(product.sku_description) ? product.sku_description.ToString().Replace("â€™", "'") : string.Empty;
                    //_logRepository.Info("SKU Description Added to the item -> " + product.flemingo_sku_code);

                    newItem[Constant.SKUCode] = !string.IsNullOrEmpty(product.flemingo_sku_code.ToString()) ? product.flemingo_sku_code.ToString() : string.Empty;

                    newItem.Fields[Constant.MaterialGroup].Value = (!string.IsNullOrEmpty(product.material_group_name) && (product.material_group_name != "NA")) ? _helper.Sanitize(product.material_group_name).Trim().ToLower() : string.Empty;
                    //_logRepository.Info("Material group Added to the item -> " + product.material_group_name.ToString());
                    newItem.Fields[Constant.Category].Value = (!string.IsNullOrEmpty(product.category_name) && (product.category_name != "NA")) ? _helper.Sanitize(product.category_name) : string.Empty;
                    // _logRepository.Info("Category Added to the item -> " + product.category_name.ToString());
                    newItem.Fields[Constant.SubCategory].Value = (!string.IsNullOrEmpty(product.sub_category_name) && (product.sub_category_name != "NA")) ? _helper.Sanitize(product.sub_category_name).ToLower() : string.Empty;
                    //_logRepository.Info("Sub category Added to the item -> " + product.sub_category_name.ToString());

                    newItem.Fields[Constant.Brand].Value = (!string.IsNullOrEmpty(product.brand_name) && (product.brand_name != "NA")) ? _helper.Sanitize(product.brand_name.Replace("â€™", "'")).ToLower() : string.Empty;

                    newItem[Constant.Age] = (!string.IsNullOrEmpty(Convert.ToString(product.age_of_product_for_liquor)) && (Convert.ToString(product.age_of_product_for_liquor) != "NA")) ? Convert.ToString(product.age_of_product_for_liquor) : string.Empty;

                    Sitecore.Data.Fields.CheckboxField checkboxField = newItem.Fields[Constant.Active];
                    checkboxField.Checked = product.status;

                    newItem.Fields[Constant.ProductName].Value = SEOProductName(product.sku_name.Trim().Replace("â€™", ""));

                    

                    if (!string.IsNullOrEmpty(product.bucket_name))
                    {
                        newItem.Fields[Constant.bucketGroup].Value = (AddUpdateBucketName(ref contextDB, _helper.Sanitize(product.bucket_name))).Name;
                    }

                    if (product.isRecommended)
                    {
                        Sitecore.Data.Fields.CheckboxField ckhIsRecomended = newItem.Fields[Constant.isRecomended];
                        ckhIsRecomended.Checked = product.isRecommended;
                    }

                    newItem[Constant.travelExclusive] = (!string.IsNullOrEmpty(Convert.ToString(product.travel_exclusive))) ? Convert.ToString(product.travel_exclusive) : "0";

                    newItem[Constant.SoldTogether] = (!string.IsNullOrEmpty(Convert.ToString(product.soldTogether))) ? Convert.ToString(product.soldTogether) : string.Empty;

                    imageFile = GetImageFileName(product.flemingo_sku_code);

                    if (imageFile.Count() >= 1)
                    {
                        newItem[Constant.Image1] = uploadProductImage(product.material_group_name, product.category_name, product.brand_name, product.flemingo_sku_code, imageFile[0]);
                    }

                    if (imageFile.Count() >= 2)
                    {
                        newItem[Constant.Image2] = uploadProductImage(product.material_group_name, product.category_name, product.brand_name, product.flemingo_sku_code, imageFile[1]);
                    }

                    if (imageFile.Count() >= 3)
                    {
                        newItem[Constant.Image3] = uploadProductImage(product.material_group_name, product.category_name, product.brand_name, product.flemingo_sku_code, imageFile[2]);
                    }

                    if (imageFile.Count() >= 4)
                    {
                        newItem[Constant.Image4] = uploadProductImage(product.material_group_name, product.category_name, product.brand_name, product.flemingo_sku_code, imageFile[3]);
                    }

                    if (imageFile.Count() >= 5)
                    {
                        newItem[Constant.Image5] = uploadProductImage(product.material_group_name, product.category_name, product.brand_name, product.flemingo_sku_code, imageFile[4]);
                    }

                    if (imageFile.Count() >= 6)
                    {
                        newItem[Constant.Image6] = uploadProductImage(product.material_group_name, product.category_name, product.brand_name, product.flemingo_sku_code, imageFile[5]);
                    }

                    newItem.Editing.EndEdit();
                    _logRepository.Info("Product Imported Successfully -> " + newItem.ID.ToString() + "   SKU  -->  " + product.flemingo_sku_code);
                    //  PublishProduct(newItem);

                }
            }
            catch (Exception ex)
            {
                _logRepository.Error("Item creation failed due to -> " + ex.Message);
                _logRepository.Info("6A2023_Failed in create count -> " + countCreate.ToString() + "   SKU  -->  " + product.flemingo_sku_code);
                newItem.Editing.CancelEdit();
            }
            return countCreate;
        }


        private int UpdateSitecoreProduct(Item newItem, Product product, int countUpdate)
        {


            List<string> imageFile = null;
            if (product != null)
            {
                string imagePath = string.Empty;

                Sitecore.Data.Database contextDB = GetContextDatabase();
                //if(product.material_group_name.ToLower() == "beauty")
                //{
                //    _logRepository.Info("Check-> " + product.sku_name.ToString());
                //}
                try
                {
                    UpdateCategory(ref contextDB, product.material_group_name, product.category_name, product.brand_name.Replace("â€™", "'"), product.sub_category_name);

                    newItem.Editing.BeginEdit();

                    newItem.Appearance.DisplayName = product.material_group_name + " " + product.category_name + " " + product.sku_name.Replace("â€™", "'") + " " + product.flemingo_sku_code;
                    // SKU Information 
                    newItem[Constant.SKUID] = !string.IsNullOrEmpty(product.id.ToString()) ? product.id.ToString() : string.Empty;

                    newItem[Constant.SKUName] = !string.IsNullOrEmpty(product.sku_name.ToString()) ? product.sku_name.ToString().Replace("â€™", "'") : String.Empty;
                    // _logRepository.Info("SKU Name Added to the item -> " + product.sku_name.ToString());

                    newItem[Constant.SKUDescription] = !string.IsNullOrEmpty(product.sku_description) ? product.sku_description.ToString().Replace("\"", "").Replace("â€™", "'") : string.Empty;
                    //   _logRepository.Info("SKU Description Added to the item -> " + product.flemingo_sku_code);

                    newItem[Constant.SKUCode] = !string.IsNullOrEmpty(product.flemingo_sku_code.ToString()) ? product.flemingo_sku_code.ToString() : string.Empty;

                    newItem.Fields[Constant.MaterialGroup].Value = (!string.IsNullOrEmpty(product.material_group_name) && (product.material_group_name != "NA")) ? _helper.Sanitize(product.material_group_name).Trim().ToLower() : string.Empty;
                    //  _logRepository.Info("Material group Added to the item -> " + product.material_group_name.ToString());
                    newItem.Fields[Constant.Category].Value = (!string.IsNullOrEmpty(product.category_name) && (product.category_name != "NA")) ? _helper.Sanitize(product.category_name) : string.Empty;
                    //  _logRepository.Info("Category Added to the item -> " + product.category_name.ToString());
                    newItem.Fields[Constant.SubCategory].Value = (!string.IsNullOrEmpty(product.sub_category_name) && (product.sub_category_name != "NA")) ? _helper.Sanitize(product.sub_category_name).Replace(",", ""): string.Empty;
                    // _logRepository.Info("Sub category Added to the item -> " + product.sub_category_name.ToString());

                    newItem.Fields[Constant.Brand].Value = (!string.IsNullOrEmpty(product.brand_name) && (product.brand_name != "NA")) ? _helper.Sanitize(product.brand_name.Replace("â€™", "'")).ToLower() : string.Empty;

                    newItem.Fields[Constant.ProductName].Value = SEOProductName(product.sku_name.Trim().Replace("â€™", ""));                    


                    if (!string.IsNullOrEmpty(product.bucket_name))
                    {
                        newItem.Fields[Constant.bucketGroup].Value = (AddUpdateBucketName(ref contextDB, _helper.Sanitize(product.bucket_name.Replace("+", "").Replace(".", "").Replace("â€™", "'")))).Name;
                    }
                    /*
                    if (product.isRecommended)
                    {
                        Sitecore.Data.Fields.CheckboxField ckhIsRecomended = newItem.Fields[Constant.isRecomended];
                        ckhIsRecomended.Checked = product.isRecommended;
                    }

                    newItem[Constant.travelExclusive] = (!string.IsNullOrEmpty(Convert.ToString(product.travel_exclusive))) ? Convert.ToString(product.travel_exclusive) : "0";

                    newItem[Constant.SoldTogether] = (!string.IsNullOrEmpty(Convert.ToString(product.soldTogether))) ? Convert.ToString(product.soldTogether) : string.Empty;
                    */
                    imageFile = GetImageFileName(product.flemingo_sku_code);

                    if (imageFile.Count() >= 1)
                    {
                        newItem[Constant.Image1] = uploadProductImage(product.material_group_name, product.category_name, product.brand_name, product.flemingo_sku_code, imageFile[0]);
                    }

                    if (imageFile.Count() >= 2)
                    {
                        newItem[Constant.Image2] = uploadProductImage(product.material_group_name, product.category_name, product.brand_name, product.flemingo_sku_code, imageFile[1]);
                    }

                    if (imageFile.Count() >= 3)
                    {
                        newItem[Constant.Image3] = uploadProductImage(product.material_group_name, product.category_name, product.brand_name, product.flemingo_sku_code, imageFile[2]);
                    }

                    if (imageFile.Count() >= 4)
                    {
                        newItem[Constant.Image4] = uploadProductImage(product.material_group_name, product.category_name, product.brand_name, product.flemingo_sku_code, imageFile[3]);
                    }

                    if (imageFile.Count() >= 5)
                    {
                        newItem[Constant.Image5] = uploadProductImage(product.material_group_name, product.category_name, product.brand_name, product.flemingo_sku_code, imageFile[4]);
                    }

                    if (imageFile.Count() >= 6)
                    {
                        newItem[Constant.Image6] = uploadProductImage(product.material_group_name, product.category_name, product.brand_name, product.flemingo_sku_code, imageFile[5]);
                    }


                    newItem.Editing.EndEdit();
                    _logRepository.Info("Product Imported Successfully -> " + newItem.ID.ToString());
                    //  PublishProduct(newItem);


                }
                catch (Exception ex)
                {
                    _logRepository.Error("Item creation failed due to -> " + ex.Message);
                    _logRepository.Info("6A2023_Failed in update count -> " + countUpdate.ToString() + "   SKU  -->  " + product.flemingo_sku_code);
                    newItem.Editing.CancelEdit();
                }
            }

            return countUpdate;
        }

        private void UpdateProductImages(Item productItem, Product product)
        {
            Item parentItem = GetProductImageFolderItem(_helper.Sanitize(product.material_group_name), _helper.Sanitize(product.category_name), _helper.Sanitize(product.brand_name), product.flemingo_sku_code);

            Item productImage = productItem;

            productImage.Editing.BeginEdit();

            if (!string.IsNullOrEmpty(productItem.Fields["image 1"].Value) && productItem.Fields["image 1"].ToString().IndexOf("/sitecore/") != -1)
            {

                if (productImage != null)
                {
                    // productImage.MoveTo(parentItem);
                    productItem["image 1"] = parentItem.Paths.ContentPath + "/" + product.flemingo_sku_code;


                }
            }

            if (!string.IsNullOrEmpty(productItem.Fields["image 2"].Value) && productItem.Fields["image 2"].ToString().IndexOf("/sitecore/") != -1)
            {
                // productImage = Sitecore.Context.ContentDatabase.GetItem(productItem.Fields["image 2"].Value);
                if (productImage != null)
                {
                    //  productImage.MoveTo(parentItem);

                    productItem["image 2"] = parentItem.Paths.ContentPath + "/" + product.flemingo_sku_code;
                }
            }

            if (!string.IsNullOrEmpty(productItem.Fields["image 3"].Value) && productItem.Fields["image 3"].ToString().IndexOf("/sitecore/") != -1)
            {
                // productImage = Sitecore.Context.ContentDatabase.GetItem(productItem.Fields["image 3"].Value);
                if (productImage != null)
                {
                    // productImage.MoveTo(parentItem);

                    productItem["image 3"] = parentItem.Paths.ContentPath + "/" + product.flemingo_sku_code;
                }
            }

            productImage.Editing.EndEdit();

        }

        private string uploadProductImage(string material_group_name, string category_name, string brand, string skuCode, string filename)
        {
            var filewithextention = filename.Split('.');
            //string fileExt = filename.Substring(filename.LastIndexOf('.'), filename.Length);
            //filename = _helper.Sanitize(filename.Substring(0, filename.LastIndexOf('.'))).Replacebrand
            brand = brand.Replace("â€™", "");
            string imageSitecorePath = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(filename))
                {
                    MediaItem mediaItem = null;
                    Item parentItem = GetProductImageFolderItem(_helper.Sanitize(material_group_name), _helper.Sanitize(category_name), _helper.Sanitize(brand), skuCode);



                    if (filewithextention.Length > 0)
                    {
                        mediaItem = Sitecore.Context.ContentDatabase.GetItem(parentItem.Paths.ContentPath + "/" + filewithextention[0].Trim());
                    }

                    if (mediaItem == null)
                    {
                        mediaItem = AddFile(parentItem.Paths.ContentPath, filename, skuCode);
                    }


                    if (mediaItem != null)
                    {
                        imageSitecorePath = "/sitecore/media library" + mediaItem.MediaPath;
                    }
                }

            }
            catch (Exception ex)
            {
                _logRepository.Info("uploadProductImage error for SKU  " + skuCode + "       " + ex.Message);
            }

            return imageSitecorePath;
        }

        public List<string> GetImageFileName(string skuCode)
        {
            List<string> filename = new List<string>();
            try
            {
                DirectoryInfo place = new DirectoryInfo("D:\\Code\\Adani\\BaseImagePath\\" + skuCode);
                FileInfo[] Files = place.GetFiles();
                foreach (FileInfo fileinfo in Files)
                {

                    if (fileinfo.Name.IndexOf(".DS") < 0 && fileinfo.Length < 500000)
                    {
                        filename.Add(fileinfo.Name);
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Info("ImageField Path not avaialable for SKU  " + ex.Message);
            }
            return filename;
        }

        private MediaItem AddFile(string sitecorePath, string mediaItemName, string skuCode)
        {
            MediaItem mediaItem = null;
            try
            {
                string filePath = "D:\\Code\\Adani\\BaseImagePath\\" + skuCode + "\\" + mediaItemName;

                var filenamewithextention = mediaItemName.Split('.');

                if (filenamewithextention.Length > 0)
                {
                    mediaItemName = filenamewithextention[0].Trim();
                }

                // Create the options
                Sitecore.Resources.Media.MediaCreatorOptions options = new Sitecore.Resources.Media.MediaCreatorOptions();
                // Store the file in the database, not as a file
                options.FileBased = false;
                // Remove file extension from item name
                options.IncludeExtensionInItemName = false;

                // Do not make a versioned template
                options.Versioned = false;
                // set the path
                options.Destination = sitecorePath + "/" + mediaItemName;
                // Set the database
                options.Database = Sitecore.Configuration.Factory.GetDatabase("master");

                // Now create the file
                Sitecore.Resources.Media.MediaCreator creator = new Sitecore.Resources.Media.MediaCreator();
                mediaItem = creator.CreateFromFile(filePath, options);
            }
            catch (Exception ex)
            {
                _logRepository.Info("Image upload failed for SKU   " + skuCode + "  filename   " + mediaItemName + "    " + ex.Message);
            }

            return mediaItem;
        }

        private string SEOProductName(string name)
        {
            name = name.Replace("â€™", "").Replace("(", "").Replace(")", "").Replace("+", "-").Replace("&", "").Replace("%", "").Replace("/", "-").Replace(" ", "-");
            name = name.Replace("'", "").Replace("$", "").Replace(",", "").Replace(".", "-").Replace("---", "-");
            return name.ToLower();
        }

        private int UpdateProductsFromJSON()
        {
            StreamReader sr = new StreamReader("D:\\Code\\Adani\\Import\\Import.json");
            string jsonString = sr.ReadToEnd();
            sr.Close();
            var jsonObject = JsonConvert.DeserializeObject<List<Prod>>(jsonString);

            Sitecore.Data.Database contextDB = GetContextDatabase();
            Item allProductsFolder = contextDB.GetItem(new ID("{11DFEA97-5FDD-4ECE-9623-1189B9AE91C5}"));
            string basePath = "/sitecore/content/AirportHome/Datasource/Adani/Dutyfree/Products/";


            int count = 0;
            List<string> imageFile = null;
            foreach (Prod prod in jsonObject)
            {
                string bucket = string.Empty;
                string soldtogether = string.Empty;
                bool recommended = false;


                Item product = contextDB.GetItem(basePath + _helper.Sanitize(prod.Material_Group).ToLower() + "/" + _helper.Sanitize(prod.Category).ToLower() + "/" + prod.Sku_Code);

               
                if (product == null)
                {
                    var productTemplate = contextDB.GetTemplate(Constant.templateId);
                    Sitecore.Data.Items.Item parentItem = GetProductCategoryFolderItem(_helper.Sanitize(prod.Material_Group).ToLower(), _helper.Sanitize(prod.Category).Replace(",", "").ToLower());
                    product = parentItem.Add(prod.Sku_Code, productTemplate);                    
                }

                if (product != null)
                {
                    using (new EditContext(product))
                    {
                        product.Fields["SKU Description"].Value = !string.IsNullOrEmpty(prod.Sku_Description) ? prod.Sku_Description : string.Empty;
                        product.Appearance.DisplayName = prod.Material_Group + " " + prod.Category + " " + prod.Sku_Name + " " + prod.Sku_Code;
                        product.Fields[Constant.SKUName].Value = !string.IsNullOrEmpty(prod.Sku_Name.ToString()) ? prod.Sku_Name.ToString() : String.Empty;
                        product.Fields[Constant.Category].Value = (!string.IsNullOrEmpty(prod.Category) && (prod.Category != "NA")) ? _helper.Sanitize(prod.Category) : string.Empty;
                        product.Fields[Constant.SubCategory].Value = (!string.IsNullOrEmpty(prod.Sub_Category) && (prod.Sub_Category != "NA")) ? _helper.Sanitize(prod.Sub_Category) : string.Empty;
                        product.Fields[Constant.Brand].Value = (!string.IsNullOrEmpty(prod.Brand) && (prod.Brand != "NA")) ? _helper.Sanitize(prod.Brand) : string.Empty;
                        product.Fields[Constant.MaterialGroup].Value = _helper.Sanitize(prod.Material_Group).ToLower();

                        UpdateCategory(ref contextDB, prod.Material_Group, prod.Category, prod.Brand.Replace("â€™", "'"), prod.Sub_Category);
                        //product.Fields[Constant.SoldTogether].Value = !string.IsNullOrEmpty(soldtogether) ? soldtogether : String.Empty;

                        if (!string.IsNullOrEmpty(bucket))
                        {
                            product.Fields[Constant.bucketGroup].Value = bucket;
                        }

                        if (recommended)
                        {
                            Sitecore.Data.Fields.CheckboxField ckhIsRecomended = product.Fields[Constant.isRecomended];
                            ckhIsRecomended.Checked = recommended;
                        }

                        //product.Fields["SKU Description"].Value = !string.IsNullOrEmpty(prod.Sku_Description) ? prod.Sku_Description : string.Empty;
                        //product.Fields["About Brand"].Value = !string.IsNullOrEmpty(prod.About_Brand) ? prod.About_Brand : string.Empty;
                        //product.Fields["Product Highlights"].Value = !string.IsNullOrEmpty(prod.Product_Highlights) ? prod.Product_Highlights : string.Empty;
                        //product.Fields["Product Ingredients"].Value = !string.IsNullOrEmpty(prod.Key_Ingredients) ? prod.Key_Ingredients : string.Empty;
                        //product.Fields["Product Benefits"].Value = !string.IsNullOrEmpty(prod.Benefits) ? prod.Benefits : string.Empty;
                        //product.Fields["Manufacturer Details"].Value = !string.IsNullOrEmpty(prod.Manufacturer) ? prod.Manufacturer : string.Empty;
                        //product.Fields["Product Use"].Value = !string.IsNullOrEmpty(prod.How_To_Use) ? prod.How_To_Use : string.Empty;
                        //product.Fields["Product Safety"].Value = !string.IsNullOrEmpty(prod.Safety_Final) ? prod.Safety_Final : string.Empty;
                        //product.Fields["Product Flavour"].Value = !string.IsNullOrEmpty(prod.Flavour) ? prod.Flavour : string.Empty;
                        //product.Fields["Product Form"].Value = !string.IsNullOrEmpty(prod.Form) ? prod.Form : string.Empty;

                        //product.Fields["Frame Colour Front"].Value = !string.IsNullOrEmpty(prod.Frame_Colour_Front) ? prod.Frame_Colour_Front : string.Empty;
                        //product.Fields["Frame Colour Temple"].Value = !string.IsNullOrEmpty(prod.Frame_Colour_Temple) ? prod.Frame_Colour_Temple : string.Empty;
                        //product.Fields["Lens Colour"].Value = !string.IsNullOrEmpty(prod.Lens_Colour) ? prod.Lens_Colour : string.Empty;
                        //product.Fields["Material"].Value = !string.IsNullOrEmpty(prod.Material) ? prod.Material : string.Empty;
                        //product.Fields["Material Fitting Name"].Value = !string.IsNullOrEmpty(prod.Material_Fitting_Name) ? prod.Material_Fitting_Name : string.Empty;


                        imageFile = GetImageFileName(prod.Sku_Code);

                        if (imageFile.Count() >= 1)
                        {
                            product[Constant.Image1] = uploadProductImage(prod.Material_Group, prod.Category, prod.Brand, prod.Sku_Code, imageFile[0]);
                        }

                        if (imageFile.Count() >= 2)
                        {
                            product[Constant.Image2] = uploadProductImage(prod.Material_Group, prod.Category, prod.Brand, prod.Sku_Code, imageFile[1]);
                        }

                        if (imageFile.Count() >= 3)
                        {
                            product[Constant.Image3] = uploadProductImage(prod.Material_Group, prod.Category, prod.Brand, prod.Sku_Code, imageFile[2]);
                        }

                        if (imageFile.Count() >= 4)
                        {
                            product[Constant.Image4] = uploadProductImage(prod.Material_Group, prod.Category, prod.Brand, prod.Sku_Code, imageFile[3]);
                        }

                        if (imageFile.Count() >= 5)
                        {
                            product[Constant.Image5] = uploadProductImage(prod.Material_Group, prod.Category, prod.Brand, prod.Sku_Code, imageFile[4]);
                        }

                        if (imageFile.Count() >= 6)
                        {
                            product[Constant.Image6] = uploadProductImage(prod.Material_Group, prod.Category, prod.Brand, prod.Sku_Code, imageFile[5]);
                        }

                        _logRepository.Info("eyewear Products updated" + product.Name);
                    }
                    count = count + 1;
                }
            }

            return count;
        }

        private void UpdateBrand()
        {
            //Item brandItem = GetExistingItem("/sitecore/content/AirportHome/Datasource/Adani/Dutyfree/Products/Navigation/Brands/" + _helper.Sanitize(brandName));
            Sitecore.Data.Database contextDB = GetContextDatabase();
            Item brandParentItem = contextDB.GetItem(Constant.BrandParentItemId);
            int i = 1;
          
            try
            {
                if (brandParentItem != null  )
                {
                    foreach (Item brandItem in brandParentItem.GetChildren())
                    {
                        using (new EditContext(brandItem))
                        {
                            
                            brandItem[Constant.BrandCode] = brandItem[Constant.BrandCode].ToLower();
                           
                        }

                    }
                   
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error("update Brand method failed due to -> " + ex.Message);

            }

            return ;
        }
    }
}