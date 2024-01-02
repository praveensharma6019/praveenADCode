using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public class DepartureLandingContent : IDepartureLandingContent
    {
        private readonly ILogRepository _logRepository;

        public DepartureLandingContent(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public DepartureLanding GetDepartureLanding(Sitecore.Data.Items.Item item)
        {
            DepartureLanding departLading = new DepartureLanding();
            try
            {
                departLading.TabContent = new List<Tab>();
                if (item == null) return new DepartureLanding();
                MultilistField tabs = item?.Fields[Templates.DepartureParent.DepartureParentFields.Tabs];
                if (tabs != null && tabs.GetItems().Length > 0)
                {
                    foreach (var tabItem in tabs.GetItems())
                    {
                        departLading.TabContent.Add(GetTabs(tabItem));
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return departLading;
        }

        private Tab GetTabs(Sitecore.Data.Items.Item item)
        {
            Tab tab = new Tab();
            try
            {
                tab.Title = item?.Fields[Templates.DepartureTab.TabFields.Title]?.Value;
                tab.Text = item?.Fields[Templates.DepartureTab.TabFields.Description]?.Value;
                tab.SubTitles = GetSubTitle(item);
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return tab;
        }

        private SubTitle GetSubTitle(Sitecore.Data.Items.Item item)
        {
            SubTitle subTitle = new SubTitle();
            try
            {
                subTitle.SubTitleList = new List<DepartureType>();
                MultilistField departTypeItems = item?.Fields[Templates.DepartureTab.TabFields.DepartureTypes];
                if (departTypeItems != null && departTypeItems.GetItems().Length > 0)
                {
                    foreach (var departItem in departTypeItems.GetItems())
                    {
                        DepartureType departType = new DepartureType();
                        departType.SubTitle = departItem?.Fields[Templates.DepartureType.DepartureTypeFields.DepartureTypeTitle]?.Value;
                        departType.SubSubTitle = departItem?.Fields[Templates.DepartureType.DepartureTypeFields.DepartureTypeSubTitle]?.Value;
                        departType.SubText = departItem?.Fields[Templates.DepartureType.DepartureTypeFields.DepartureTypeDescription]?.Value;
                        departType.Table = GetPackageTable(departItem);
                        departType.List = GetSpecialOfferings(departItem);
                        departType.AddOns = GetAddOnsList(departItem);
                        departType.Information = GetInformation(departItem);
                        subTitle.SubTitleList.Add(departType);
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return subTitle;
        }

        private OfferingList GetSpecialOfferings(Sitecore.Data.Items.Item departItem)
        {
            OfferingList list = new OfferingList();
            try
            {
                list.Title = departItem?.Fields[Templates.DepartureType.DepartureTypeFields.ServiceOfferingsTitle]?.Value;
                list.Items = new List<Models.OfferingItem>();
                MultilistField offerField = departItem?.Fields[Templates.DepartureType.DepartureTypeFields.OfferingsList];
                if (offerField != null && offerField.GetItems().Length > 0)
                {
                    foreach (var offerItem in offerField.GetItems())
                    {
                        Adani.SuperApp.Airport.Feature.Pranaam.Models.OfferingItem item = new Models.OfferingItem();
                        item.Text = offerItem?.Fields[Templates.DepartureOfferings.DepartureChargeFields.OfferingText]?.Value;
                        item.Value = offerItem?.Fields[Templates.DepartureOfferings.DepartureChargeFields.Value]?.Value;
                        list.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return list;
        }

        private ChargeTable GetPackageTable(Sitecore.Data.Items.Item item)
        {
            ChargeTable table = new ChargeTable();
            try
            {
                MultilistField headerField = item?.Fields[Templates.DepartureType.DepartureTypeFields.TableHeaders];
                table.Header = new List<TableHeader>();
                table.Rows = new List<TableRow>();
                if (headerField != null && headerField.GetItems().Length > 0)
                {
                    foreach (var tableItem in headerField.GetItems())
                    {
                        TableHeader header = new TableHeader();
                        header.Title = tableItem?.Fields[Templates.DepartureTableHeader.DepartTableHeaderFields.Title]?.Value;
                        header.Value = tableItem?.Fields[Templates.DepartureTableHeader.DepartTableHeaderFields.Value]?.Value;
                        table.Header.Add(header);
                    }
                }
                MultilistField chargesField = item?.Fields[Templates.DepartureType.DepartureTypeFields.ChargesList];
                if (chargesField != null && chargesField.GetItems().Length > 0)
                {
                    foreach (var tableRowItem in chargesField.GetItems())
                    {
                        TableRow row = new TableRow();
                        row.GuestInfo = tableRowItem?.Fields[Templates.DepartureCharge.DepartureChargeFields.Guest]?.Value;
                        row.Charges = tableRowItem?.Fields[Templates.DepartureCharge.DepartureChargeFields.GuestCharges]?.Value;
                        row.GST = tableRowItem?.Fields[Templates.DepartureCharge.DepartureChargeFields.GST]?.Value;
                        row.Total = tableRowItem?.Fields[Templates.DepartureCharge.DepartureChargeFields.Total]?.Value;
                        table.Rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return table;
        }

        private AdditionalAddOns GetAddOnsList(Item departItem)
        {
            AdditionalAddOns AddOnsList = new AdditionalAddOns();
            try
            {
                AddOnsList.AddOnsTitle = departItem?.Fields[Templates.DepartureType.DepartureTypeFields.AdditionalAddOnsTitle]?.Value;
                List<OfferingItem> additionaladdons = new List<OfferingItem>();
                MultilistField addOnsField = departItem?.Fields[Templates.DepartureType.DepartureTypeFields.AdditionalAddOns];
                if (addOnsField != null && addOnsField.GetItems() != null && addOnsField.GetItems().Length > 0)
                {
                    foreach (var addOns in addOnsField.GetItems())
                    {
                        OfferingItem item = new OfferingItem();
                        item.Text = addOns?.Fields[Templates.DepartureOfferings.DepartureChargeFields.OfferingText]?.Value;
                        item.Value = addOns?.Fields[Templates.DepartureOfferings.DepartureChargeFields.Value]?.Value;
                        additionaladdons.Add(item);
                    }
                    AddOnsList.Item = additionaladdons;
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return AddOnsList;
        }

        private GeneralInformation GetInformation(Sitecore.Data.Items.Item departItem)
        {
            GeneralInformation information = new GeneralInformation();
                try
                {
                    if (departItem != null)
                    {
                        information.Title = departItem?.Fields[Templates.DepartureType.DepartureTypeFields.ExpressBookingTitle]?.Value;
                        information.Description = departItem?.Fields[Templates.DepartureType.DepartureTypeFields.ExpressBooking]?.Value;
                    }
                }
                catch (Exception ex)
                {
                    _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
                }
            return information;
        }         
    }
}
