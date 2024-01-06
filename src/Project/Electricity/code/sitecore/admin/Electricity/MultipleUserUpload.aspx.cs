using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.Data;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using DocumentFormat.OpenXml.Office.CustomUI;
using System.Linq;
using Sitecore.Data.Items;
using Sitecore.Collections;
using Sitecore.Data;
using System.Net;
using System.IO;
using Sitecore.Resources.Media;
using Sitecore.Configuration;
using Sitecore.SecurityModel;
using Sitecore.Links;
using Sitecore;
using System.Text.RegularExpressions;
using Sitecore.Electricity.Website.Model;
using System.Linq.Expressions;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;
using System.Text;

namespace Sitecore.Electricity.Website.sitecore.admin.Electricity
{
	public partial class MultipleUserUpload : System.Web.UI.Page
	{


		protected void Page_Load(object sender, EventArgs e)
		{

		}

		protected void btnUploadFile_Click(object sender, EventArgs e)
		{
			try
			{
				lblErroMsg.Visible = false;
				lblSuccessMsg.Visible = false;

				if (fuExcelselection.HasFile)
				{
					string savefilepath = Server.MapPath("~/sitecore/admin/MeterReading/temp/Import/" + DateTime.Now.ToString("yyyyMMddHHmmss") + fuExcelselection.FileName);
					fuExcelselection.SaveAs(savefilepath);
					using (XLWorkbook workBook = new XLWorkbook(savefilepath))
					{
						IXLWorksheet workSheet = workBook.Worksheet(1);

						//Create a new DataTable.
						DataTable dt = new DataTable();

						//Loop through the Worksheet rows.
						bool firstRow = true;
						foreach (IXLRow row in workSheet.Rows())
						{
							if (!row.IsEmpty())
							{
								if (firstRow)
								{
									foreach (IXLCell cell in row.Cells())
									{
										dt.Columns.Add(cell.Value.ToString());
									}
									firstRow = false;
								}
								else
								{
									//Add rows to DataTable.
									dt.Rows.Add();
									int i = 0;
									foreach (IXLCell cell in row.Cells())
									{
										dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
										i++;
									}
								}
							}
						}
						if (dt != null && dt.Rows.Count > 0)
						{
							Importdata(dt);
							lblSuccessMsg.Visible = true;

						}
					}
				}
			}
			catch (Exception ex)
			{
				lblErroMsg.Visible = true;
				lblErroMsg.Text = ex.Message;
				Log.Error(ex.Message, ex, this);
			}

		}

		private void Importdata(DataTable dt)
		{
			StringBuilder sb = new StringBuilder();
			string savefilepath = System.Web.Hosting.HostingEnvironment.MapPath("~/sitecore/admin/MeterReading/temp/Import/");
			sb.AppendLine("Importdata Started - " + System.DateTime.Now.ToString());
			File.AppendAllText(savefilepath + "log.txt", sb.ToString());
			sb.Clear();

			try
			{

				foreach (DataRow row in dt.Rows)
				{

					if (!string.IsNullOrEmpty(row["LoginName"].ToString()) && !row["LoginName"].ToString().Contains(",") && Exists(row["LoginName"].ToString()))
					{
						if (!string.IsNullOrEmpty(row["Account No"].ToString()))
						{
							var userDetail = GetUser(row["LoginName"].ToString(), row["Password"].ToString().Trim());
							if (userDetail == null)
							{
								sb.AppendLine("UserDetail getting null - " + System.DateTime.Now.ToString());
								File.AppendAllText(savefilepath + "log.txt", sb.ToString());
								sb.Clear();
							}
							else
							{
								var MultipleAccountItemId = userDetail.Profile.GetCustomProperty("Multiple Account");
								var primaryAccountItemId = userDetail.Profile.GetCustomProperty("Primary Account No");
							
								string multiplerecordlist = string.Empty;
								string primaryAcc = string.Empty;
								multiplerecordlist = primaryAccountItemId.ToString();
								try
								{
									if (!string.IsNullOrEmpty(row["SUBCA"].ToString()))
									{
										var multiplerecord = row["SUBCA"].ToString().Split(',');
										if (multiplerecord.Length > 0)
										{
											foreach (var subcaitem in multiplerecord)
											{
												Sitecore.Data.Items.Item subaccountitem = CreateAccountItem(subcaitem.Trim(), "0000000");

												multiplerecordlist = multiplerecordlist + "|" + subaccountitem.ID.ToString();
											}
										}
									}

									userDetail.Profile.SetCustomProperty("Multiple Account", multiplerecordlist);
									userDetail.Profile.Save();
									
								}
								catch (Exception ex)
								{
									Log.Error("Error RegisterUser User Upload Page-  " + ex.Message, this);
									sb.AppendLine("Getting Exception 1   - " + ex.Message + row["LoginName"].ToString() + " - " + multiplerecordlist + " - " + System.DateTime.Now.ToString());
									File.AppendAllText(savefilepath + "log.txt", sb.ToString());
									sb.Clear();
								}
							}
						}
					}
					else
					{
						Log.Info("User Upload Page - LoginName exists - " + row["LoginName"].ToString(), this);
						sb.AppendLine("Getting loginname exists   - " + row["Account No"].ToString() + " - " + System.DateTime.Now.ToString());
						File.AppendAllText(savefilepath + "log.txt", sb.ToString());
						sb.Clear();
					}
				}
			}
			catch (Exception ex)
			{
				// Log the message on any failure to sitecore log
				Sitecore.Diagnostics.Log.Error("User Upload Page Error Description : " + ex.Message, this);
				sb.AppendLine("Getting exception 2   - " + ex.Message + " - " + System.DateTime.Now.ToString());
				lblErroMsg.Text = ex.Message;
				lblErroMsg.Visible = true;
			}
		}

		public bool Exists(string userName)
		{
			var fullname = "electricity" + "\\" + userName;
			//  var fullName = Sitecore.Context.Domain.GetFullName(userName);
			return Sitecore.Security.Accounts.User.Exists(fullname);
		}


		public string GetAccountItemId(string accountNumber)
		{
			StringBuilder sb = new StringBuilder();

			string savefilepath = System.Web.Hosting.HostingEnvironment.MapPath("~/sitecore/admin/MeterReading/temp/Import/");
			sb.AppendLine("GetAccountItemId Started 1 - " + accountNumber + "-" + System.DateTime.Now.ToString());
			File.AppendAllText(savefilepath + "log.txt", sb.ToString());
			sb.Clear();

			using (IProviderSearchContext context = ContentSearchManager.GetIndex("sitecore_core_index").CreateSearchContext())
			{
				var predicateQuery = PredicateBuilder.True<SearchResultItem>();
				predicateQuery = predicateQuery.And(p => p.TemplateId == new Data.ID("{38FCD543-B37B-46EE-B867-40A000148DDB}")); //Sitecore.Feature.Accounts.Templates.AccountNumber.ItemId.ToString())
				predicateQuery = predicateQuery.And(p => p["account_number_s"] == accountNumber);


				var result = context.GetQueryable<SearchResultItem>().Where(predicateQuery).Where(x => x.Language == Sitecore.Globalization.Language.Current.Name);
				if (result != null && result.ToList().Count() > 0)
				{
					sb.AppendLine("GetAccountItemId Started 2 - " + accountNumber + "- ID - " + "result.FirstOrDefault().ItemId.ToString()" + "-" + System.DateTime.Now.ToString());
					File.AppendAllText(savefilepath + "log.txt", sb.ToString());
					sb.Clear();

					//return true;
					return result.FirstOrDefault().ItemId.ToString();
				}
			}
			return string.Empty;
		}

		public Sitecore.Data.Items.Item CreateAccountItem(string accountNumber, string meterNumber)
		{
			Sitecore.Data.Items.Item newItem = null;

			Expression<Func<SearchResultItem, bool>> predicateQuery = PredicateBuilder.True<SearchResultItem>();
			predicateQuery = predicateQuery.Or(p => p.TemplateId == new Data.ID("{38FCD543-B37B-46EE-B867-40A000148DDB}"));

			ISearchIndex index = ContentSearchManager.GetIndex("sitecore_core_index");
			using (IProviderSearchContext context = index.CreateSearchContext())
			{
				var searchResult = context.GetQueryable<SearchResultItem>().Where(predicateQuery).Where(x => x.Language == Sitecore.Globalization.Language.Current.Name && x.Name.Equals(accountNumber + "-" + meterNumber));

				bool flag = false;
				if (searchResult != null && searchResult.ToList().Count() > 0)
				{
					using (new Sitecore.SecurityModel.SecurityDisabler())
					{
						foreach (var resultitem in searchResult.ToList())
						{
							var resultdata = resultitem.GetItem();
							if (resultdata.Name.Equals(accountNumber + "-" + meterNumber))
							{
								newItem = resultdata;
								flag = true;
							}
						}
						
					}
				}
				if(flag == false)
				{

					using (new Sitecore.SecurityModel.SecurityDisabler())
					{

						Sitecore.Data.Database master = Sitecore.Data.Database.GetDatabase(Settings.ProfileItemDatabase);

						TemplateItem template = master.GetItem("/sitecore/templates/Feature/Accounts/AccountList");

						Sitecore.Data.Items.Item parentItem = master.GetItem("/sitecore/content/Home/Account List");

						newItem = parentItem.Add(accountNumber + "-" + meterNumber, template);

						newItem.Editing.BeginEdit();
						try
						{
							// Assign values to the fields of the new item
							newItem.Fields["Account Number"].Value = accountNumber;
							newItem.Fields["Meter Number"].Value = meterNumber;
							newItem.Editing.EndEdit();

							//Refresh sitecore core index adter creating new item
							ContentSearchManager.GetIndex("sitecore_core_index").Refresh((SitecoreIndexableItem)newItem);
						}
						catch (System.Exception ex)
						{
							Sitecore.Diagnostics.Log.Error("Could not update item " + newItem.Paths.FullPath + ": " + ex.Message, this);
							newItem.Editing.CancelEdit();
						}

					}
					// Get the master database
				}
			}
			return newItem;
		}

		public User GetUser(string userName, string password)
		{
			var UserName = string.Empty;
			UserName = "electricity" + "\\" + userName;
			if (!System.Web.Security.Membership.ValidateUser(UserName, password))
				return null;
			if (Sitecore.Security.Accounts.User.Exists(UserName))
				return Sitecore.Security.Accounts.User.FromName(UserName, true);
			return null;
		}
	}
}