using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.IO;
using Sitecore.Mvc;


namespace Sitecore.GarvHai.Website.sitecore.admin.GarvHai
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter da;
     


        protected void Page_Load(object sender, EventArgs e)
        {
            string connection = System.Configuration.ConfigurationManager.ConnectionStrings["SitecoreFormData"].ConnectionString;
            con = new SqlConnection(connection);
            con.Open();

        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string ApplyNowFormId = "";
            string FileName = "GarvHaiRecords.xls";
            //string FormId = DictionaryPhraseRepository.Current.Get("/Form/FormId", "");
            if (string.IsNullOrEmpty(ApplyNowFormId))
            {
                ApplyNowFormId = "DF8C5C1D-4841-4358-89C1-E6FBB712586F";
            }
            string ContactFormId = "1F8C8323-1535-429C-BE83-49FC956952B0";
           
            try
            {
                string query = "";
                DateTime fromdate, todate;
                fromdate = DateTime.Parse(TextBoxFrom.Text);
                todate = DateTime.Parse(TextBoxTo.Text);
                if (System.Convert.ToDateTime(TextBoxTo.Text) < System.Convert.ToDateTime(TextBoxFrom.Text))
                {
                    lblErroMsg.Text = "Please select proper date";
                    lblErroMsg.Visible = true;
                    return;
                }

                if (ddl1.SelectedIndex == 0)
                {
                    lblErroMsg.Text = "Please select the proper Form-Type";
                    lblErroMsg.Visible = true;
                    return;

                }
                else if (ddl1.SelectedIndex == 1)
                {
                    todate = todate.AddDays(1);

                    query = "SELECT [DateofBirth],SUBSTRING(BirthCertificate, 9, PATINDEX('%,%', BirthCertificate)-10) AS BirthCertificate,[SportType],[Event],[CategoryType],SUBSTRING(Photograph, 9, PATINDEX('%,%', Photograph)-10) AS Photograph,[Achievements],[FirstCompetitionDate],[FirstCompetitionName],[FirstCompetitionResultDate],SUBSTRING(FirstCompetitionCertificate, 9, PATINDEX('%,%', FirstCompetitionCertificate)-10) AS FirstCompetitionCertificate,[SecondCompetitionDate],[SecondCompetitionName],[SecondCompetitionResultDate],SUBSTRING(SecondCompetitionCertificate, 9, PATINDEX('%,%', SecondCompetitionCertificate)-10) AS SecondCompetitionCertificate,[ThirdCompetitionDate],[ThirdCompetitionName],[ThirdCompetitionResultDate],SUBSTRING(ThirdCompetitionCertificate, 9, PATINDEX('%,%', ThirdCompetitionCertificate)-10) AS ThirdCompetitionCertificate,[FirstName],[Surname],[AthletePhoneNumber],[AthleteEmail],[Education],[YearOfEducation],[SchoolCollegeName],[AddressLine1],[AddressLine2],[City],[District],[State],[FatherName],[FatherProfession],[FatherAnnualIncome],[FatherPhoneNumber],[MotherName],[MotherPhoneNumber],[MotherProfession],[MotherAnnualIncome],SUBSTRING(AddressProofCertificate, 9, PATINDEX('%,%', AddressProofCertificate)-10) AS AddressProofCertificate,SUBSTRING(ParentSalaryProofCertificate, 9, PATINDEX('%,%', ParentSalaryProofCertificate)-10) AS ParentSalaryProofCertificate,[PersonalBest],[SeasonBest],SUBSTRING(InternationalEventCertificate, 9, PATINDEX('%,%', InternationalEventCertificate)-10) AS InternationalEventCertificate, SUBSTRING(NationalRankingCertificate, 9, PATINDEX('%,%', NationalRankingCertificate)-10) AS NationalRankingCertificate,[Recordholder],[WorldRanking],SUBSTRING(PersonalBestSeasonBestCertificate, 9, PATINDEX('%,%', PersonalBestSeasonBestCertificate)-10) AS PersonalBestSeasonBestCertificate,[CoachName],[CoachNumber],[PracticeGroundNameAddress],[InjuryDetail],[CompetitiveGoals]from (SELECT [FormEntryID],[Value] AS[amount], [FieldName] AS[categ] FROM[dbo].[FieldData] where FormEntryID in (select ID from FormEntry where FormItemID = '" + ApplyNowFormId + "' and Created >='" + fromdate + "' and Created<='" + todate + "'))x pivot(max(amount) for categ in ([DateofBirth],[BirthCertificate],[SportType],[Event],[CategoryType], Photograph,[Achievements],[FirstCompetitionDate],[FirstCompetitionName],[FirstCompetitionResultDate], FirstCompetitionCertificate,[SecondCompetitionDate],[SecondCompetitionName],[SecondCompetitionResultDate], SecondCompetitionCertificate,[ThirdCompetitionDate],[ThirdCompetitionName],[ThirdCompetitionResultDate], ThirdCompetitionCertificate,[FirstName],[Surname],[AthletePhoneNumber],[AthleteEmail],[Education],[YearOfEducation],[SchoolCollegeName],[AddressLine1],[AddressLine2],[City],[District],[State],[FatherName],[FatherProfession],[FatherAnnualIncome],[FatherPhoneNumber],[MotherName],[MotherPhoneNumber],[MotherProfession],[MotherAnnualIncome], AddressProofCertificate, ParentSalaryProofCertificate,[PersonalBest],[SeasonBest], InternationalEventCertificate, NationalRankingCertificate,[Recordholder],[WorldRanking], PersonalBestSeasonBestCertificate,[CoachName],[CoachNumber],[PracticeGroundNameAddress],[InjuryDetail],[CompetitiveGoals])) p";
                    FileName = "GarvhaiApplyNowFormRecord.xls";
                }
                else if (ddl1.SelectedIndex == 2)
                {
                    todate = todate.AddDays(1);
                    query = "SELECT[Name],[Email],[Mobile],[Comments] from(SELECT[FormEntryID],  [Value] AS[amount],[FieldName] AS[categ] FROM[dbo].[FieldData] where FormEntryID in (select ID from FormEntry where FormItemID = '"+ContactFormId+ "'and Created >='" + fromdate + "' and Created<='" + todate + "')) x pivot(max(amount) for categ in ([Name],[Email],[Mobile],[Comments])) p";
                    FileName = "GarvhaiContactFormRecord.xls";
                }

                cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                GridView1.DataSource = dt;
                GridView1.DataBind();
                
                string savefilepath = Server.MapPath("~/sitecore/admin/Garvhai/SampleFile/GarvhaiApplyNowFormRecord.xls");
                //  DatatableToCSV(dt, savefilepath);
                //string attachment = "attachment; filename=GarvhaiApplyNowFormRecord.xls";
                //Response.ClearContent();
                //Response.AddHeader("content-disposition", attachment);
                //Response.ContentType = "application/vnd.ms-excel";
                //string tab = "";
                //foreach (DataColumn dc in dt.Columns)
                //{
                //    Response.Write(tab + dc.ColumnName);
                //    tab = "\t";
                //}
                //Response.Write("\n");
                //int i;
                //foreach (DataRow dr in dt.Rows)
                //{
                //    tab = "";
                //    for (i = 0; i < dt.Columns.Count; i++)
                //    {
                //        Response.Write(tab + dr[i].ToString());
                //        tab = "\t";
                //    }
                //    Response.Write("\n");
                //}
                //Response.End();
                Response.Clear();
                Response.Buffer = true;
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Charset = "";
                
                StringWriter strwritter = new StringWriter();
                HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                GridView1.GridLines = GridLines.Both;
                GridView1.HeaderStyle.Font.Bold = true;
                GridView1.RenderControl(htmltextwrtter);
                Response.Write(strwritter.ToString());
                Response.End();
            }
            catch (Exception ex)
            {

                lblErroMsg.Text = "There is some technically problem. Please contact administrator.";
                Sitecore.Diagnostics.Log.Error("Form data -Export Error  " + ex.Message, this);
            }


        }
        public void DatatableToCSV(DataTable dtDataTable, string strFilePath)
        {
            #region Old Method
            //StreamWriter sw = new StreamWriter(strFilePath, false);
            ////headers  
            //for (int i = 0; i < dtDataTable.Columns.Count; i++)
            //{
            //    sw.Write(dtDataTable.Columns[i]);
            //    if (i < dtDataTable.Columns.Count - 1)
            //    {
            //        sw.Write(",");
            //    }
            //}
            //sw.Write(sw.NewLine);
            //foreach (DataRow dr in dtDataTable.Rows)
            //{
            //    for (int i = 0; i < dtDataTable.Columns.Count; i++)
            //    {
            //        if (!System.Convert.IsDBNull(dr[i]))
            //        {
            //            string value = dr[i].ToString();
            //            if (value.Contains(','))
            //            {
            //                value = String.Format("\"{0}\"", value);
            //                sw.Write(value);
            //            }
            //            else
            //            {
            //                sw.Write(dr[i].ToString());
            //            }
            //        }
            //        if (i < dtDataTable.Columns.Count - 1)
            //        {
            //            sw.Write(",");
            //        }
            //    }
            //    sw.Write(sw.NewLine);
            //}
            //sw.Close();
            //DownLoad(strFilePath);
            #endregion

            #region New Method

            Stopwatch stw = new Stopwatch();
            stw.Start();
            StringBuilder sb = new StringBuilder();
            //Column headers  
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sb.Append(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append(Environment.NewLine);
            //Column Values
            foreach (DataRow dr in dtDataTable.Rows)
            {
                sb.AppendLine(string.Join(",", dr.ItemArray));
            }
            File.WriteAllText(strFilePath, sb.ToString());
            stw.Stop();
            DownLoad(strFilePath);

            #endregion
        }
        public void DownLoad(string FName)
        {
            string path = FName;
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            if (file.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=GarvhaiApplyNowFormRecord.xls");
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/excel";
                Response.WriteFile(file.FullName);
                Response.End();
            }
            else
            {
                Response.Write("This file does not exist.");
            }

        }

    }
}