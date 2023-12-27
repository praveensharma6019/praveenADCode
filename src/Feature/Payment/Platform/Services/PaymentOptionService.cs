using Adani.SuperApp.Airport.Feature.Payment.Platform.Models;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Collections;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Mvc.Extensions;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.IPAddresses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adani.SuperApp.Airport.Feature.Payment.Platform.Services
{
    public class PaymentOptionService : IPaymentOptionService
    {
        private readonly IHelper _helper;

        public PaymentOptionService(IHelper helper)
        {

            this._helper = helper;
        }
        /// <summary>
        /// To get payment options
        /// </summary>
        /// <param name="dataSourceItem"></param>
        /// <returns></returns>
        public PaymentOptions GetPaymentOptions(Item dataSourceItem, string Type, string ChannelType, string url)
        {
            PaymentOptions paymentOptions = null;

            if (dataSourceItem != null)
            {
                paymentOptions = new PaymentOptions
                {
                    ChoosePaymentHeading = dataSourceItem.Fields[Templates.PaymentOption.Fields.paymentHeading].ToString(),
                    Hostname = _helper.GetUrlDomain(),
                    PromoCards = GetPromoCards(dataSourceItem),
                    PaymentTypeList = GetPaymentType(dataSourceItem, ChannelType, url),
                    CardDetail = GetCardDetail(dataSourceItem),
                    PayText = dataSourceItem?.Fields[Templates.PaymentOption.Fields.paymentText].ToString(),
                    SecurityCardDetail = GetSecurityCardDetail(dataSourceItem),
                    ApprovePayments = GetApprovePaymentPromoCard(dataSourceItem),
                    SafeIconBig = _helper.GetImageURLByFieldId(dataSourceItem, Templates.PaymentOption.Fields.SafeIconBigFieldId),
                    SafeIconSmall = _helper.GetImageURLByFieldId(dataSourceItem, Templates.PaymentOption.Fields.SafeIconSmallFieldId),
                    SafeText = dataSourceItem?.Fields[Templates.PaymentOption.Fields.SafeTextFieldId]?.ToString(),
                    UPIValidError = Translate.Text("upi_valid_error"),
                    OfferText = GetOfferText(dataSourceItem),
                    isReward = !string.IsNullOrEmpty(Type) && Type.ToLower() == "forex" ? false : true,
                    downTimeText = dataSourceItem.Fields[Templates.PaymentOption.Fields.DownTimeText] != null ? dataSourceItem.Fields[Templates.PaymentOption.Fields.DownTimeText].Value : string.Empty,
                    fluctuateTimeText = dataSourceItem.Fields[Templates.PaymentOption.Fields.FluctuateTimeText] != null ? dataSourceItem.Fields[Templates.PaymentOption.Fields.FluctuateTimeText].Value : string.Empty,
                };
            }
            return paymentOptions;
        }


        #region Private Methods

        /// <summary>
        /// To get Card details info
        /// </summary>
        /// <param name="dataSourceItem"></param>
        /// <returns></returns>
        private CardDetail GetCardDetail(Item dataSourceItem)
        {
            CardDetail cardDetail = null;

            ReferenceField cardOptionField = dataSourceItem.Fields[Templates.PaymentOption.Fields.CardOptionFieldId];

            if (cardOptionField != null && cardOptionField.TargetItem != null && cardOptionField.TargetItem.Fields.Any())
            {
                Item cardOptionItem = cardOptionField.TargetItem;

                cardDetail = new CardDetail
                {
                    CardImageBig = _helper.GetImageURLByFieldId(cardOptionItem, Templates.CardOption.CardImageBigFieldId),
                    CardImageSmall = _helper.GetImageURLByFieldId(cardOptionItem, Templates.CardOption.CardImageSmallFieldId),
                    CardNumberLabel = cardOptionItem.Fields[Templates.CardOption.CardNumberLabelFieldId]?.ToString(),
                    CvvLabel = cardOptionItem.Fields[Templates.CardOption.CvvLabelFieldId]?.ToString(),
                    IncorrectCardNumberErrMsg = cardOptionItem.Fields[Templates.CardOption.IncorrectCardNumberErrMsgFieldId]?.ToString(),
                    IncorrectNameCardErrMsg = cardOptionItem.Fields[Templates.CardOption.IncorrectNameCardErrMsgFieldId]?.ToString(),
                    IncorrectValidThruErrMsg = cardOptionItem.Fields[Templates.CardOption.IncorrectValidThruErrMsgFieldId]?.ToString(),
                    NameOnCardLabel = cardOptionItem.Fields[Templates.CardOption.NameOnCardLabelFieldId]?.ToString(),
                    RequiredCardNumberErrMsg = cardOptionItem.Fields[Templates.CardOption.RequiredCardNumberErrMsgFieldId]?.ToString(),
                    RequiredValidThruErrMsg = cardOptionItem.Fields[Templates.CardOption.RequiredValidThruErrMsgFieldId]?.ToString(),
                    SecureCardLabel = cardOptionItem.Fields[Templates.CardOption.SecureCardLabelFieldId]?.ToString(),
                    ValidThruLabel = cardOptionItem.Fields[Templates.CardOption.ValidThruLabelFieldId]?.ToString(),
                    ExpireIconBig = _helper.GetImageURLByFieldId(cardOptionItem, Templates.CardOption.ExpireIconBigFieldId),
                    ExpireIconSmall = _helper.GetImageURLByFieldId(cardOptionItem, Templates.CardOption.ExpireIconSmallFieldId),
                    InfoIconBig = _helper.GetImageURLByFieldId(cardOptionItem, Templates.CardOption.InfoIconBigFieldId),
                    InfoIconSmall = _helper.GetImageURLByFieldId(cardOptionItem, Templates.CardOption.InfoIconBigFieldId),
                    IncorrectCvvErrMsg = cardOptionItem.Fields[Templates.CardOption.IncorrectCvvFieldId]?.ToString(),
                    RequiredCvvErrMsg = cardOptionItem.Fields[Templates.CardOption.RequiredCvvFieldId]?.ToString(),
                    ViewOtherBankLabel = Translate.Text("view_bank")
                };
            }

            return cardDetail;
        }

        /// <summary>
        /// To Get Promo cards in Approve payment
        /// </summary>
        /// <param name="dataSourceItem"></param>
        /// <returns></returns>
        private PromoCard GetApprovePaymentPromoCard(Item dataSourceItem)
        {
            PromoCard promoCard = null;

            ReferenceField promoCards = dataSourceItem.Fields[Templates.PaymentOption.Fields.ApprovePaymentFieldId];

            if (promoCards != null && promoCards.TargetItem != null && promoCards.TargetItem.Fields.Any())
            {
                Item promoCardItem = promoCards.TargetItem;

                promoCard = new PromoCard()
                {
                    Heading = promoCardItem.Fields[Templates.Promocards.Fields.HeadingFieldId].ToString(),
                    Description = promoCardItem.Fields[Templates.Promocards.Fields.DescriptionFieldId].ToString(),
                    ImageSmall = _helper.GetImageURLByFieldId(promoCardItem, Templates.Promocards.Fields.ImageSmallFieldId),
                    ImageLarge = _helper.GetImageURLByFieldId(promoCardItem, Templates.Promocards.Fields.ImageLargeFieldId),
                    ButtonText = promoCardItem.Fields[Templates.Promocards.Fields.ButtonTextFieldId].ToString(),
                    Note = promoCardItem.Fields[Templates.Promocards.Fields.NoteFieldId].ToString()
                };
            }

            return promoCard;
        }

        /// <summary>
        /// To Get Promo cards in payment home page
        /// </summary>
        /// <param name="dataSourceItem"></param>
        /// <returns></returns>
        private List<PromoCard> GetPromoCards(Item dataSourceItem)
        {
            List<PromoCard> promoCardList = new List<PromoCard>();

            MultilistField promoCards = dataSourceItem.Fields[Templates.PaymentOption.Fields.PromoCardFieldId];

            if (promoCards != null && promoCards.GetItems() != null)
            {
                foreach (Item promoCardItem in promoCards.GetItems())
                {
                    PromoCard promoCard = new PromoCard()
                    {
                        Heading = promoCardItem.Fields[Templates.Promocards.Fields.HeadingFieldId].ToString(),
                        Description = promoCardItem.Fields[Templates.Promocards.Fields.DescriptionFieldId].ToString(),
                        ImageLarge = _helper.GetImageURLByFieldId(promoCardItem, Templates.Promocards.Fields.ImageLargeFieldId),
                        ImageSmall = _helper.GetImageURLByFieldId(promoCardItem, Templates.Promocards.Fields.ImageSmallFieldId)
                    };

                    promoCardList.Add(promoCard);
                }
            }

            return promoCardList;
        }

        /// <summary>
        /// To get security card details
        /// </summary>
        /// <param name="dataSourceItem"></param>
        /// <returns></returns>
        private SecurityCard GetSecurityCardDetail(Item dataSourceItem)
        {
            SecurityCard securityCard = null;

            ReferenceField securityCardField = dataSourceItem.Fields[Templates.PaymentOption.Fields.SecurityCardFieldId];

            if (securityCardField != null && securityCardField.TargetItem != null && securityCardField.TargetItem.Fields.Any())
            {
                Item securityCardItem = securityCardField.TargetItem;

                securityCard = new SecurityCard()
                {
                    SecurityCardHeading = securityCardItem.Fields[Templates.SecurityCard.Fields.SecurityCardHeadingFieldId].ToString(),
                    SecurityCardDesc = securityCardItem.Fields[Templates.SecurityCard.Fields.SecurityCardDescFieldId].ToString(),
                    SecurityImageSmall = _helper.GetImageURLByFieldId(securityCardItem, Templates.SecurityCard.Fields.SecurityImageSmallFieldId),
                    SecurityImageBig = _helper.GetImageURLByFieldId(securityCardItem, Templates.SecurityCard.Fields.SecurityImageBigFieldId),
                    SecurityPoint1 = securityCardItem.Fields[Templates.SecurityCard.Fields.SecurityPoint1FieldId].ToString(),
                    SecurityPoint2 = securityCardItem.Fields[Templates.SecurityCard.Fields.SecurityPoint2FieldId].ToString(),
                    AskLaterLink = securityCardItem.Fields[Templates.SecurityCard.Fields.AskLaterLinkFieldId].ToString(),
                    SecurePayLink = securityCardItem.Fields[Templates.SecurityCard.Fields.SecurePayLinkFieldId].ToString()
                };
            }
            return securityCard;
        }

        /// <summary>
        /// To Get Payment Types
        /// </summary>
        /// <param name="dataSourceItem"></param>
        /// <param name="paymentTypes"></param>
        /// <returns></returns>
        private List<PaymentType> GetPaymentType(Item dataSourceItem, string ChannelType, string url)
        {
            List<PaymentType> paymentTypes = new List<PaymentType>();

            MultilistField paymentTypeList = dataSourceItem?.Fields[Templates.PaymentOption.Fields.paymentTypeListFieldId];

            if (paymentTypeList?.GetItems()?.Length > 0)
            {
                foreach (Item paymentTypeItem in paymentTypeList.GetItems())
                {
                    if (paymentTypeItem != null && paymentTypeItem.Fields.Any())
                    {
                        PaymentType paymentType = new PaymentType
                        {
                         TypeFilter = paymentTypeItem.Fields[Templates.PaymentType.TypeFilter] != null ? paymentTypeItem.Fields[Templates.PaymentType.TypeFilter].Value : string.Empty,
                         Type = paymentTypeItem.Fields[Templates.PaymentType.PaymentTypeFieldId].ToString(),
                         Name = paymentTypeItem.Fields[Templates.PaymentType.PaymentTypeNameFieldId].ToString(),
                         IconBig = _helper.GetImageURLByFieldId(paymentTypeItem, Templates.PaymentType.PaymentTypeIconBigFieldId),
                         IconSmall = _helper.GetImageURLByFieldId(paymentTypeItem, Templates.PaymentType.PaymentTypeIconSmallFieldId)
                        };
                        paymentType.Options = new List<Card>();

                        paymentType.Options = paymentType.Type.ContainsText(Constants.Card) ? GetCardInfo(dataSourceItem) :
                                                     paymentType.Type.ContainsText(Constants.Upi) ? GetUPIInfo(dataSourceItem, ChannelType,url) :
                                                         paymentType.Type.ContainsText(Constants.Wallet) ? GetWalletInfo(dataSourceItem) :
                                                              paymentType.Type.ContainsText(Constants.NetBanking) ? GetNetBankInfo(dataSourceItem):
                                                              paymentType.Type.ContainsText(Constants.EMI) ? GetEMIInfo(dataSourceItem) : null;

                        if (paymentTypeItem.Name == Templates.PaymentType.UPIItem || paymentTypeItem.Name == Templates.PaymentType.WalletItem || paymentTypeItem.Name == Templates.PaymentType.NewUPIItem || paymentTypeItem.Name == Templates.PaymentType.NewWalletItem || paymentTypeItem.Name == Templates.PaymentType.AndroidUPIItem || paymentTypeItem.Name == Templates.PaymentType.IOSUPIItem || paymentTypeItem.Name == Templates.PaymentType.EMIItem)
                        {
                            Amount amountItem = new Amount();
                            {
                                amountItem.amountlimit = paymentTypeItem.Fields["Amountlimit"].Value;
                                amountItem.title = paymentTypeItem.Fields["Title"].Value;
                                amountItem.message = paymentTypeItem.Fields["Message"].Value;
                                amountItem.minAmount = paymentTypeItem.Fields["MinAmount"].Value;
                                amountItem.minAmountMessage = paymentTypeItem.Fields["MinAmountMessage"].Value;

                            }
                            paymentType.ActiveRule = amountItem;
                        }
                        paymentTypes.Add(paymentType);
                    }
                }
            }
            if (ChannelType.ToLower().Equals("android"))
            {
                paymentTypes = paymentTypes.Where(a => !(a.TypeFilter.ToLower().Equals("upi new") || a.TypeFilter.ToLower().Equals("ios"))).ToList();
            }
            else if (ChannelType.ToLower().Equals("ios"))
            {
                paymentTypes = paymentTypes.Where(a => !(a.TypeFilter.ToLower().Equals("android") || a.TypeFilter.ToLower().Equals("upi new"))).ToList();
            }
            else
            {
                paymentTypes = paymentTypes.Where(a => !(a.TypeFilter.ToLower().Equals("android") || a.TypeFilter.ToLower().Equals("ios"))).ToList();
            }

            return paymentTypes;
        }

        /// <summary>
        /// To Get NetBanking List Info
        /// </summary>
        /// <param name="dataSourceItem"></param>
        /// <returns></returns>
        private List<Card> GetNetBankInfo(Item dataSourceItem)
        {
            List<Card> netBankingOptionList = new List<Card>();

            ReferenceField netBankingList = dataSourceItem?.Fields[Templates.PaymentOption.Fields.NetBankingListFieldId];

            if (netBankingList != null && netBankingList.TargetItem != null)
            {
                ChildList childListItem = netBankingList.TargetItem.GetChildren();

                foreach (Item netBankingItem in childListItem)
                {
                    if (netBankingItem != null && netBankingItem.Fields.Any())
                    {
                        Card netBankingOptionItem = new Card
                        {
                            Code = netBankingItem?.Fields[Templates.NetBanking.NetBankingCodeFieldId]?.ToString(),
                            Name = netBankingItem?.Fields[Templates.NetBanking.NetBankingNameFieldId]?.ToString(),
                            LargeIcon = _helper.GetImageURLByFieldId(netBankingItem, Templates.NetBanking.BankIconLargeFieldId),
                            SmallIcon = _helper.GetImageURLByFieldId(netBankingItem, Templates.NetBanking.BankIconSmallFieldId),
                            IsActive = !string.IsNullOrEmpty(netBankingItem.Fields[Templates.NetBanking.NetBankingIsActiveFieldId]?.Value),
                            IsShowInPage = !string.IsNullOrEmpty(netBankingItem.Fields[Templates.NetBanking.IsShowInPageFieldId]?.Value)
                        };

                        netBankingOptionList.Add(netBankingOptionItem);
                    }
                }
            }

            return netBankingOptionList;
        }

        /// <summary>
        /// To Get wallet Info
        /// </summary>
        /// <param name="dataSourceItem"></param>
        /// <returns></returns>
        private List<Card> GetWalletInfo(Item dataSourceItem)
        {
            List<Card> walletOptionList = new List<Card>();

            ReferenceField walletList = dataSourceItem.Fields[Templates.PaymentOption.Fields.walletListFieldId];

            if (walletList != null && walletList.TargetItem != null)
            {
                ChildList childListItem = walletList.TargetItem.GetChildren();

                foreach (Item walletItem in childListItem)
                {
                    if (walletItem != null && walletItem.Fields.Any())
                    {
                        Card walletOptionItem = new Card
                        {
                            Code = walletItem.Fields[Templates.WalletOption.walletCodeFieldId].ToString(),
                            Name = walletItem.Fields[Templates.WalletOption.walletNameFieldId].ToString(),
                            LargeIcon = _helper.GetImageURLByFieldId(walletItem, Templates.WalletOption.WalletLargeIconFieldId),
                            SmallIcon = _helper.GetImageURLByFieldId(walletItem, Templates.WalletOption.WalletSmallIconFieldId),
                            IsActive = !string.IsNullOrEmpty(walletItem.Fields[Templates.WalletOption.walletIsActiveFieldId]?.Value)
                        };
                        walletOptionList.Add(walletOptionItem);
                    }
                }
            }

            return walletOptionList;
        }

        /// <summary>
        /// To get UPI info
        /// </summary>
        /// <param name="dataSourceItem"></param>
        /// <returns></returns>
        private List<Card> GetUPIInfo(Item dataSourceItem, string ChannelType, string url)
        {
            List<Card> upiOptionItems = new List<Card>();

            MultilistField upiReferenceField = dataSourceItem.Fields[Templates.PaymentOption.Fields.UpiOptionFieldId];

            if (upiReferenceField != null && upiReferenceField.GetItems().Count() > 0)
            {
                List<Item> childUpiListItem = upiReferenceField.GetItems().ToList();
               
                if(ChannelType == "" && url.Contains("PaymentHomePage"))
                {
                    foreach (Item upiItem in childUpiListItem)
                    {
                        if (upiItem != null && upiItem.Fields.Any())
                        {
                            foreach (Item item in upiItem.Children)
                            {
                                Card upiOptionItem = new Card()
                                {
                                    Code = item.Fields[Templates.UpiOption.UpiCodeFieldId].ToString(),
                                    Name = item.Fields[Templates.UpiOption.UpiLabelFieldId].ToString(),
                                    LargeIcon = _helper.GetImageURLByFieldId(item, Templates.UpiOption.UpiLargeIconFieldId),
                                    SmallIcon = _helper.GetImageURLByFieldId(item, Templates.UpiOption.UpiSmallIconFieldId),
                                    IsActive = !string.IsNullOrEmpty(item.Fields[Templates.UpiOption.UpiIsActiveFieldId]?.Value),
                                    PackageName = item.Fields[Templates.UpiOption.PackageNameId] != null ? item.Fields[Templates.UpiOption.PackageNameId].Value : string.Empty
                                };

                                upiOptionItems.Add(upiOptionItem);
                            }

                        }
                    }
                }
                else if(ChannelType == "" && url.Contains("PaymentMethod"))
                {
                    foreach (Item upiItem in childUpiListItem.Where(a => a.Fields["TypeFilter"].Value == "upi new"))
                    {
                        if (upiItem != null && upiItem.Fields.Any())
                        {
                            foreach (Item item in upiItem.Children)
                            {
                                Card upiOptionItem = new Card()
                                {
                                    Code = item.Fields[Templates.UpiOption.UpiCodeFieldId].ToString(),
                                    Name = item.Fields[Templates.UpiOption.UpiLabelFieldId].ToString(),
                                    LargeIcon = _helper.GetImageURLByFieldId(item, Templates.UpiOption.UpiLargeIconFieldId),
                                    SmallIcon = _helper.GetImageURLByFieldId(item, Templates.UpiOption.UpiSmallIconFieldId),
                                    IsActive = !string.IsNullOrEmpty(item.Fields[Templates.UpiOption.UpiIsActiveFieldId]?.Value),
                                    PackageName = item.Fields[Templates.UpiOption.PackageNameId] != null ? item.Fields[Templates.UpiOption.PackageNameId].Value : string.Empty
                                };

                                upiOptionItems.Add(upiOptionItem);
                            }

                        }
                    }
                }
                else
                {
                    var upiListItem = childUpiListItem.Where(a => a.Fields["TypeFilter"].Value == ChannelType);
                    foreach (Item upiItem in upiListItem)
                    {
                        if (upiItem != null && upiItem.Fields.Any())
                        {
                            foreach (Item item in upiItem.Children)
                            {
                                Card upiOptionItem = new Card()
                                {
                                    Code = item.Fields[Templates.UpiOption.UpiCodeFieldId].ToString(),
                                    Name = item.Fields[Templates.UpiOption.UpiLabelFieldId].ToString(),
                                    LargeIcon = _helper.GetImageURLByFieldId(item, Templates.UpiOption.UpiLargeIconFieldId),
                                    SmallIcon = _helper.GetImageURLByFieldId(item, Templates.UpiOption.UpiSmallIconFieldId),
                                    IsActive = !string.IsNullOrEmpty(item.Fields[Templates.UpiOption.UpiIsActiveFieldId]?.Value),
                                    PackageName = item.Fields[Templates.UpiOption.PackageNameId] != null ? item.Fields[Templates.UpiOption.PackageNameId].Value : string.Empty
                                };

                                upiOptionItems.Add(upiOptionItem);
                            }

                        }
                    }
                }

            }

            return upiOptionItems;
        }

        /// <summary>
        /// To get Card info
        /// </summary>
        /// <param name="dataSourceItem"></param>
        /// <returns></returns>
        private List<Card> GetCardInfo(Item dataSourceItem)
        {
            List<Card> cardImageIcons = new List<Card>();

            ReferenceField cardOptionField = dataSourceItem.Fields[Templates.PaymentOption.Fields.CardOptionFieldId];

            if (cardOptionField != null && cardOptionField.TargetItem != null)
            {
                Item cardOptionItem = cardOptionField.TargetItem;

                if (cardOptionItem != null && cardOptionItem.Fields.Any())
                {
                    MultilistField cardImageFields = cardOptionItem.Fields[Templates.CardOption.CardImagesFieldId];

                    if (cardImageFields != null && cardImageFields.GetItems() != null)
                    {
                        foreach (Item cardImageItem in cardImageFields.GetItems())
                        {
                            Card cardImageIcon = new Card()
                            {
                                LargeIcon = _helper.GetImageURLByFieldId(cardImageItem, Templates.CardOption.CardIconBigFieldId),
                                SmallIcon = _helper.GetImageURLByFieldId(cardImageItem, Templates.CardOption.CardIconSmallFieldId),
                                CVVLength = cardImageItem.Fields[Templates.CardOption.CVVLengthFieldId].ToString(),
                                IsNameMandatory = !string.IsNullOrEmpty(cardImageItem.Fields[Templates.CardOption.IsNameMandatoryFieldId]?.Value),
                                RegexForCardNumber = cardImageItem.Fields[Templates.CardOption.RegexForCardNumberFieldId].ToString(),
                                Name = cardImageItem?.DisplayName,
                                Code = cardImageItem?.Name,
                                IsActive = !string.IsNullOrEmpty(cardImageItem.Fields[Templates.CardOption.IsActiveFieldId]?.Value),
                                IsShowInPage = !string.IsNullOrEmpty(cardImageItem.Fields[Templates.CardOption.IsShowInPageFieldId]?.Value),
                                
                            };
                            cardImageIcons.Add(cardImageIcon);
                        }
                    }
                }
            }

                return cardImageIcons;
        }


        /// To get EMI info
        private List<Card> GetEMIInfo(Item dataSourceItem)
        {
            
                List<Card> EMIOptionList = new List<Card>();

                MultilistField EMIList = dataSourceItem.Fields[Templates.PaymentOption.Fields.EMIListFieldId];


                if (EMIList != null && EMIList.GetItems().Count() > 0)
                {

                    List<Item> childListItem = EMIList.GetItems().ToList();

                    foreach (Item EMIItem in childListItem)
                    {
                        if (EMIItem != null && EMIItem.Fields.Any())
                        {
                            foreach (Item item in EMIItem.Children)
                            {
                                Card EMIOptionItem = new Card();


                                EMIOptionItem.Code = item.Fields[Templates.EMIOption.EMICodeFieldId].ToString();
                                EMIOptionItem.Name = item.Fields[Templates.EMIOption.EMINameFieldId].ToString();
                                EMIOptionItem.LargeIcon = _helper.GetImageURLByFieldId(item, Templates.EMIOption.EMILargeIconFieldId);
                                EMIOptionItem.SmallIcon = _helper.GetImageURLByFieldId(item, Templates.EMIOption.EMISmallIconFieldId);
                                EMIOptionItem.IsActive = !string.IsNullOrEmpty(item.Fields[Templates.EMIOption.EMIIsActiveFieldId]?.Value);
                                EMIOptionItem.TnC = _helper.GetLinkURL(item, Templates.EMIOption.TNCFieldId);
                                EMIOptionItem.tnCDC = _helper.GetLinkURL(item, Templates.EMIOption.TNCDC);


                            EMIOptionList.Add(EMIOptionItem);
                            }

                        }
                    }
                }

                return EMIOptionList;
            
        }
        
private PromoCard GetOfferText(Item dataSourceItem)
    {
        PromoCard offerText = new PromoCard();
        ReferenceField OfferFieldInfo = dataSourceItem.Fields[Templates.PaymentOption.Fields.OfferTextFieldId];
        if (OfferFieldInfo != null && OfferFieldInfo.TargetItem != null)
        {
            Item OfferFieldItem = OfferFieldInfo.TargetItem;
            offerText.Heading = OfferFieldItem.Fields[Templates.Promocards.Fields.HeadingFieldId].ToString();
            offerText.Description = OfferFieldItem.Fields[Templates.Promocards.Fields.DescriptionFieldId].ToString();
            offerText.ImageLarge = _helper.GetImageURLByFieldId(OfferFieldItem, Templates.Promocards.Fields.ImageLargeFieldId);
            offerText.ImageSmall = _helper.GetImageURLByFieldId(OfferFieldItem, Templates.Promocards.Fields.ImageSmallFieldId);
            offerText.BtnLink = _helper.LinkUrl(OfferFieldItem.Fields[Templates.Promocards.Fields.BtnLinkFieldId]);
        }
        return offerText;
    }


    #endregion Private Methods
}
}