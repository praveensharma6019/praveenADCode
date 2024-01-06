extern alias itextsharp;
using itextsharp::iTextSharp.text;
using itextsharp::iTextSharp.text.pdf;
using itextsharp::iTextSharp.text.pdf.draw;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;


namespace Sitecore.Feature.Accounts.Services
{
    public class ITDeclarationService
    {
        private
        static string FontRegular = "";
        private static string FontBold = "";
        private static string FontMedium = "";
        Font ColorFont = FontFactory.GetFont(FontRegular, 8, Font.NORMAL, new BaseColor(171, 49, 117));
        Font boldFont = FontFactory.GetFont(FontBold, 8, Font.NORMAL);
        Font RegularFont = FontFactory.GetFont(FontRegular, 10, Font.NORMAL);//23, 56, 133
        Font RegularFontWhite = FontFactory.GetFont(FontRegular, 8, Font.NORMAL, new BaseColor(255, 255, 255));
        Font RegularFont2 = FontFactory.GetFont(FontRegular, 8, Font.NORMAL, new BaseColor(23, 56, 133));
        Font smallFontWhite = FontFactory.GetFont(FontRegular, 6, new BaseColor(255, 255, 255));
        Font smallFont = FontFactory.GetFont(FontRegular, 6);
        Font smallredFont = FontFactory.GetFont(FontRegular, 6, Font.NORMAL, new BaseColor(209, 85, 82));
        Font FontTextmed8 = FontFactory.GetFont(FontMedium, 8, Font.NORMAL, new BaseColor(209, 85, 82));

        public byte[] GeneratePDF_LinkPANWithAdhar(string accountNumber, string panNumber, string aadharNumber, HttpServerUtilityBase serverObj)
        {
            using (MemoryStream output = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter writer = PdfWriter.GetInstance(document, output);
                document.Open();
                PdfContentByte cb = writer.DirectContent;

                //Add logo Image                

                String imagePath2 = serverObj.MapPath("/Images/powerofservicelogo.jpg");
                Image logoImage2 = Image.GetInstance(imagePath2.Replace("/PDFGenerate", ""));

                logoImage2.ScalePercent(50, 50);
                logoImage2.SetAbsolutePosition(20, 800);// 75,75
                document.Add(logoImage2);


                String imagePath3 = serverObj.MapPath("/Images/Adani_logo.jpg");
                Image logoImage3 = Image.GetInstance(imagePath3.Replace("/PDFGenerate", ""));

                logoImage3.ScalePercent(50, 50);
                logoImage3.SetAbsolutePosition(500, 800);// 75,75                
                document.Add(logoImage3);

                string text = @"Declaration for linking PAN with AADHAR, u/s 139 AA of income tax Act 1961";
                Paragraph paragraph = new Paragraph();
                paragraph.SpacingBefore = 10;
                paragraph.SpacingAfter = 10;
                paragraph.Alignment = Element.ALIGN_LEFT;
                paragraph.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f, BaseColor.ORANGE);
                paragraph.Add(text);
                document.Add(paragraph);

                string Date = DateTime.Now.ToString("dd/MM/yyyy");
                Paragraph Dates = new Paragraph();
                Dates.SpacingBefore = 10;
                Dates.SpacingAfter = 10;
                Dates.Alignment = Element.ALIGN_LEFT;
                Dates.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                Dates.Add("Date:" + Date);
                document.Add(Dates);
                string text1 = @"To,";
                Paragraph paragraph1 = new Paragraph();
                paragraph1.SpacingBefore = 10;
                paragraph1.SpacingAfter = 10;
                paragraph1.Alignment = Element.ALIGN_LEFT;
                paragraph1.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph1.Add(text1);
                document.Add(paragraph1);
                string text2 = @"Adani Electricity Mumbai Limited";
                Paragraph paragraph2 = new Paragraph();
                paragraph2.SpacingBefore = 10;
                paragraph2.SpacingAfter = 10;
                paragraph2.Alignment = Element.ALIGN_LEFT;
                paragraph2.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph2.Add(text2);
                document.Add(paragraph2);
                string text3 = @"Subject : Confirmation for linking of PAN with Aadhaar";
                Paragraph paragraph3 = new Paragraph();
                paragraph3.SpacingBefore = 10;
                paragraph3.SpacingAfter = 10;
                paragraph3.Alignment = Element.ALIGN_LEFT;
                paragraph3.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph3.Add(text3);
                document.Add(paragraph3);
                string text4 = @"Contract Account Number :" + accountNumber;
                Paragraph paragraph4 = new Paragraph();
                paragraph4.SpacingBefore = 10;
                paragraph4.SpacingAfter = 10;
                paragraph4.Alignment = Element.ALIGN_LEFT;
                paragraph4.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph4.Add(text4);
                document.Add(paragraph4);
                string text5 = @"Reference : " + "Section 139AA of the Income Tax Act, 1961 ('the Act')";
                Paragraph paragraph5 = new Paragraph();
                paragraph5.SpacingBefore = 10;
                paragraph5.SpacingAfter = 10;
                paragraph5.Alignment = Element.ALIGN_LEFT;
                paragraph5.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph5.Add(text5);
                document.Add(paragraph5);
                LineSeparator line = new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_LEFT, 1);
                document.Add(line);

                string text7 = @"In terms of the provisions of section 139AA of the Act, I, hereby confirm that my Permanent Account Number (PAN) " + panNumber + " has been linked with my Aadhar Number " + aadharNumber;
                Paragraph paragraph7 = new Paragraph();
                paragraph7.SpacingBefore = 10;
                paragraph7.SpacingAfter = 10;
                paragraph7.Alignment = Element.ALIGN_LEFT;
                paragraph7.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph7.Add(text7);
                document.Add(paragraph7);
                List list = new List(List.ORDERED);
                list.Add(new itextsharp.iTextSharp.text.ListItem("The above PAN is already registered with AEML against the CA number/s mentioned above.", RegularFont));
                list.Add(new itextsharp.iTextSharp.text.ListItem("Any change in the above declaration shall be communicated to AEML.", RegularFont));
                list.Add(new itextsharp.iTextSharp.text.ListItem("If the above declaration is found to be incorrect, AEML will be liable to deduct/collect TDS/TCS at higher rate, as applicable, along with interest/penalty, if applicable.", RegularFont));
                list.Add(new itextsharp.iTextSharp.text.ListItem("The above information is true and correct to the best of my knowledge.", RegularFont));
                string texting = "Terms and Conditions:";
                Paragraph paragraph9 = new Paragraph();
                paragraph9.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph9.Add(texting);

                document.Add(paragraph9);
                document.Add(list);
                string text10 = @"I shall indemnify you in case the above disclosure results in false declaration/undertaking.";
                Paragraph paragraph10 = new Paragraph();
                paragraph10.SpacingBefore = 10;
                paragraph10.SpacingAfter = 10;
                paragraph10.Alignment = Element.ALIGN_LEFT;
                paragraph10.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph10.Add(text10);
                document.Add(paragraph10);
                string text11 = @"I hereby, agree to above terms and conditions.";
                Paragraph paragraph11 = new Paragraph();
                paragraph11.SpacingBefore = 10;
                paragraph11.SpacingAfter = 10;
                paragraph11.Alignment = Element.ALIGN_LEFT;
                paragraph11.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph11.Add(text11);
                document.Add(paragraph11);
                string CurrentDate = DateTime.Now.ToShortDateString();
                string CurrentTime = DateTime.Now.ToShortTimeString();
                Paragraph CurrentTimes = new Paragraph();
                Paragraph CurrentDates = new Paragraph();
                CurrentDates.SpacingBefore = 10;
                CurrentDates.SpacingAfter = 10;
                CurrentDates.Alignment = Element.ALIGN_LEFT;
                CurrentTimes.SpacingBefore = 10;
                CurrentTimes.SpacingAfter = 10;
                CurrentTimes.Alignment = Element.ALIGN_LEFT;
                CurrentDates.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                CurrentDates.Add("Date:" + CurrentDate);
                CurrentTimes.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                CurrentTimes.Add("Time :" + CurrentTime);

                document.Add(CurrentDates);
                document.Add(CurrentTimes);

                document.Close();
                return output.ToArray();
            }
        }

        public byte[] GeneratePDF_194QApplicabilityofTDS(string accountNumber, string panNumber, HttpServerUtilityBase serverObj)
        {
            using (MemoryStream output = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter writer = PdfWriter.GetInstance(document, output);
                document.Open();
                PdfContentByte cb = writer.DirectContent;

                //Add logo Image                

                String imagePath2 = serverObj.MapPath("/Images/powerofservicelogo.jpg");
                Image logoImage2 = Image.GetInstance(imagePath2.Replace("/PDFGenerate", ""));

                logoImage2.ScalePercent(50, 50);
                logoImage2.SetAbsolutePosition(20, 800);// 75,75
                document.Add(logoImage2);


                String imagePath3 = serverObj.MapPath("/Images/Adani_logo.jpg");
                Image logoImage3 = Image.GetInstance(imagePath3.Replace("/PDFGenerate", ""));

                logoImage3.ScalePercent(50, 50);
                logoImage3.SetAbsolutePosition(500, 800);// 75,75                
                document.Add(logoImage3);

                string text = @"Declaration for applicability of TDS u/s 194Q of income tax Act 1961";
                Paragraph paragraph = new Paragraph();
                paragraph.SpacingBefore = 10;
                paragraph.SpacingAfter = 10;
                paragraph.Alignment = Element.ALIGN_LEFT;
                paragraph.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f, BaseColor.ORANGE);
                paragraph.Add(text);
                document.Add(paragraph);

                string Date = DateTime.Now.ToString("dd/MM/yyyy");
                Paragraph Dates = new Paragraph();
                Dates.SpacingBefore = 10;
                Dates.SpacingAfter = 10;
                Dates.Alignment = Element.ALIGN_LEFT;
                Dates.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                Dates.Add("Date:" + Date);
                document.Add(Dates);
                string text1 = @"To,";
                Paragraph paragraph1 = new Paragraph();
                paragraph1.SpacingBefore = 10;
                paragraph1.SpacingAfter = 10;
                paragraph1.Alignment = Element.ALIGN_LEFT;
                paragraph1.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph1.Add(text1);
                document.Add(paragraph1);
                string text2 = @"Adani Electricity Mumbai Limited";
                Paragraph paragraph2 = new Paragraph();
                paragraph2.SpacingBefore = 10;
                paragraph2.SpacingAfter = 10;
                paragraph2.Alignment = Element.ALIGN_LEFT;
                paragraph2.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph2.Add(text2);
                document.Add(paragraph2);
                string text3 = @"Subject : Confirmation for applicability of TDS U/s. 194Q";
                Paragraph paragraph3 = new Paragraph();
                paragraph3.SpacingBefore = 10;
                paragraph3.SpacingAfter = 10;
                paragraph3.Alignment = Element.ALIGN_LEFT;
                paragraph3.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph3.Add(text3);
                document.Add(paragraph3);
                string text4 = @"Contract Account Number :" + accountNumber;
                Paragraph paragraph4 = new Paragraph();
                paragraph4.SpacingBefore = 10;
                paragraph4.SpacingAfter = 10;
                paragraph4.Alignment = Element.ALIGN_LEFT;
                paragraph4.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph4.Add(text4);
                document.Add(paragraph4);
                string text5 = @"Reference : " + "Section 194Q of the Income Tax Act, 1961 ('the Act')";
                Paragraph paragraph5 = new Paragraph();
                paragraph5.SpacingBefore = 10;
                paragraph5.SpacingAfter = 10;
                paragraph5.Alignment = Element.ALIGN_LEFT;
                paragraph5.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph5.Add(text5);
                document.Add(paragraph5);
                LineSeparator line = new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_LEFT, 1);
                document.Add(line);

                int currentFYYear = DateTime.Now.Year;
                if (DateTime.Now.Month <= 3)
                {
                    currentFYYear = DateTime.Now.Year - 1;
                };

                string FY_1 = "FY " + (currentFYYear - 1).ToString() + "-" + ((currentFYYear) - 2000).ToString();
                string FY = "FY " + (currentFYYear).ToString() + "-" + ((currentFYYear + 1) - 2000).ToString();

                string text7 = @"In terms of the provisions of section 194Q of the Act, I/we, hereby confirm that our total sales/gross receipt /turnover during the previous year " + FY_1 + " is greater than INR 10 Crs and we are liable to deduct TDS U/s. 194Q on goods/materials purchased by us during " + FY + " for amount exceeding INR 50 Lacs against (PAN) " + panNumber + ".";
                Paragraph paragraph7 = new Paragraph();
                paragraph7.SpacingBefore = 10;
                paragraph7.SpacingAfter = 10;
                paragraph7.Alignment = Element.ALIGN_LEFT;
                paragraph7.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph7.Add(text7);
                document.Add(paragraph7);
                List list = new List(List.ORDERED);
                list.Add(new ListItem("PAN is already registered with AEML against the CA number/s mentioned above.", RegularFont));
                list.Add(new ListItem("Any change in the above declaration shall be communicated to AEML.", RegularFont));
                list.Add(new ListItem("We will communicate the break-up/details of TDS deducted in advance or within 24 hrs of payment made to AEML on portal(link provided), otherwise same will be treated as part/short payment.", RegularFont));
                list.Add(new ListItem("If we do not deduct TDS u/s. 194Q / No communication is sent within 24 hours of payment, AEML will be liable to collect TCS u/s. 206C(1H) of the Income Tax Act.", RegularFont));
                list.Add(new ListItem("The above information is true and correct to the best of my knowledge.", RegularFont));
                list.Add(new ListItem("Person who is submitting the declaration is authorized and responsible on behalf of the consumer.", RegularFont));
                string texting = "Terms and Conditions:";
                Paragraph paragraph9 = new Paragraph();
                paragraph9.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph9.Add(texting);

                document.Add(paragraph9);
                document.Add(list);
                string text10 = @"I shall indemnify you in case the above disclosure results in false declaration/undertaking.";
                Paragraph paragraph10 = new Paragraph();
                paragraph10.SpacingBefore = 10;
                paragraph10.SpacingAfter = 10;
                paragraph10.Alignment = Element.ALIGN_LEFT;
                paragraph10.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph10.Add(text10);
                document.Add(paragraph10);
                string text11 = @"I hereby, agree to above terms and conditions.";
                Paragraph paragraph11 = new Paragraph();
                paragraph11.SpacingBefore = 10;
                paragraph11.SpacingAfter = 10;
                paragraph11.Alignment = Element.ALIGN_LEFT;
                paragraph11.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph11.Add(text11);
                document.Add(paragraph11);
                string CurrentDate = DateTime.Now.ToShortDateString();
                string CurrentTime = DateTime.Now.ToShortTimeString();
                Paragraph CurrentTimes = new Paragraph();
                Paragraph CurrentDates = new Paragraph();
                CurrentDates.SpacingBefore = 10;
                CurrentDates.SpacingAfter = 10;
                CurrentDates.Alignment = Element.ALIGN_LEFT;
                CurrentTimes.SpacingBefore = 10;
                CurrentTimes.SpacingAfter = 10;
                CurrentTimes.Alignment = Element.ALIGN_LEFT;
                CurrentDates.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                CurrentDates.Add("Date:" + CurrentDate);
                CurrentTimes.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                CurrentTimes.Add("Time :" + CurrentTime);

                document.Add(CurrentDates);
                document.Add(CurrentTimes);

                document.Close();
                return output.ToArray();
            }
        }

        public byte[] GeneratePDF_206CFilingITR(string accountNumber, string panNumber, string fy_3_ach_no, string fy_3_ach_date, string fy_2_ach_no, string fy_2_ach_date, HttpServerUtilityBase serverObj)
        {
            using (MemoryStream output = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter writer = PdfWriter.GetInstance(document, output);
                document.Open();
                PdfContentByte cb = writer.DirectContent;

                //Add logo Image                

                String imagePath2 = serverObj.MapPath("/Images/powerofservicelogo.jpg");
                Image logoImage2 = Image.GetInstance(imagePath2.Replace("/PDFGenerate", ""));

                logoImage2.ScalePercent(50, 50);
                logoImage2.SetAbsolutePosition(20, 800);// 75,75
                document.Add(logoImage2);


                String imagePath3 = serverObj.MapPath("/Images/Adani_logo.jpg");
                Image logoImage3 = Image.GetInstance(imagePath3.Replace("/PDFGenerate", ""));

                logoImage3.ScalePercent(50, 50);
                logoImage3.SetAbsolutePosition(500, 800);// 75,75                
                document.Add(logoImage3);

                string text = @"Declaration for filing Income tax return for previous two years u/s 206AB / 206CCA.";
                Paragraph paragraph = new Paragraph();
                paragraph.SpacingBefore = 10;
                paragraph.SpacingAfter = 10;
                paragraph.Alignment = Element.ALIGN_LEFT;
                paragraph.Font = FontFactory.GetFont(FontFactory.HELVETICA, 12f, BaseColor.ORANGE);
                paragraph.Add(text);
                document.Add(paragraph);

                string Date = DateTime.Now.ToString("dd/MM/yyyy");
                Paragraph Dates = new Paragraph();
                Dates.SpacingBefore = 10;
                Dates.SpacingAfter = 10;
                Dates.Alignment = Element.ALIGN_LEFT;
                Dates.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                Dates.Add("Date:" + Date);
                document.Add(Dates);
                string text1 = @"To,";
                Paragraph paragraph1 = new Paragraph();
                paragraph1.SpacingBefore = 10;
                paragraph1.SpacingAfter = 10;
                paragraph1.Alignment = Element.ALIGN_LEFT;
                paragraph1.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph1.Add(text1);
                document.Add(paragraph1);
                string text2 = @"Adani Electricity Mumbai Limited";
                Paragraph paragraph2 = new Paragraph();
                paragraph2.SpacingBefore = 10;
                paragraph2.SpacingAfter = 10;
                paragraph2.Alignment = Element.ALIGN_LEFT;
                paragraph2.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph2.Add(text2);
                document.Add(paragraph2);
                string text3 = @"Subject : Declaration for filing of Income Tax Return for previous two years";
                Paragraph paragraph3 = new Paragraph();
                paragraph3.SpacingBefore = 10;
                paragraph3.SpacingAfter = 10;
                paragraph3.Alignment = Element.ALIGN_LEFT;
                paragraph3.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph3.Add(text3);
                document.Add(paragraph3);

                string text5 = @"Reference : " + "Section 206AB/ 206CCA of the Income Tax Act, 1961 ('the Act')";
                Paragraph paragraph5 = new Paragraph();
                paragraph5.SpacingBefore = 10;
                paragraph5.SpacingAfter = 10;
                paragraph5.Alignment = Element.ALIGN_LEFT;
                paragraph5.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph5.Add(text5);
                document.Add(paragraph5);
                string text4 = @"Contract Account Number :" + accountNumber;
                Paragraph paragraph4 = new Paragraph();
                paragraph4.SpacingBefore = 10;
                paragraph4.SpacingAfter = 10;
                paragraph4.Alignment = Element.ALIGN_LEFT;
                paragraph4.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph4.Add(text4);
                document.Add(paragraph4);
                string PanNo = @"Permanent Account Number (PAN) :" + panNumber;
                Paragraph Panparagraph4 = new Paragraph();
                Panparagraph4.SpacingBefore = 10;
                Panparagraph4.SpacingAfter = 10;
                Panparagraph4.Alignment = Element.ALIGN_LEFT;
                Panparagraph4.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                Panparagraph4.Add(PanNo);
                document.Add(Panparagraph4);
                LineSeparator line = new LineSeparator(1f, 100f, BaseColor.BLACK, Element.ALIGN_LEFT, 1);
                document.Add(line);

                int currentFYYear = DateTime.Now.Year;
                if (DateTime.Now.Month <= 3)
                {
                    currentFYYear = DateTime.Now.Year - 1;
                };

                string FY_3 = "FY " + (currentFYYear - 3).ToString() + "-" + ((currentFYYear - 2) - 2000).ToString();
                string FY_2 = "FY " + (currentFYYear - 2).ToString() + "-" + ((currentFYYear - 1) - 2000).ToString();
                string FY_1 = "FY " + (currentFYYear - 1).ToString() + "-" + ((currentFYYear) - 2000).ToString();
                string FY = "FY " + (currentFYYear).ToString() + "-" + ((currentFYYear + 1) - 2000).ToString();

                string text7 = @"In terms of the provisions of section 206AB/ 206CCA of the Act, we confirm that we have filed income tax returns for the " + FY_3 + " and " + FY_3 + " which have become due for the immediately preceding two financial years.";
                Paragraph paragraph7 = new Paragraph();
                paragraph7.SpacingBefore = 10;
                paragraph7.SpacingAfter = 10;
                paragraph7.Alignment = Element.ALIGN_LEFT;
                paragraph7.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph7.Add(text7);
                document.Add(paragraph7);
                string Acknowledgetext1 = @"The Acknowledgement Number of Income Tax Returns filed by us for last two Financial years is as follows:";
                Paragraph Acknowledgeparagraph = new Paragraph();
                Acknowledgeparagraph.SpacingBefore = 10;
                Acknowledgeparagraph.SpacingAfter = 10;
                Acknowledgeparagraph.Alignment = Element.ALIGN_LEFT;
                Acknowledgeparagraph.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                Acknowledgeparagraph.Add(Acknowledgetext1);
                document.Add(Acknowledgeparagraph);
                PdfPTable table = new PdfPTable(3);
                string AcknoeldgeDate = DateTime.Now.ToShortDateString();
                string AcknoeldgeDate2 = DateTime.Now.ToShortDateString();
                PdfPCell cell = new PdfPCell(new Phrase("Financial Year"));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Acknowledgement number"));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase("Date of filing return"));
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(FY_3));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(fy_3_ach_no));
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(fy_3_ach_date));

                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(FY_2));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(fy_2_ach_no));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(fy_2_ach_date));

                table.AddCell(cell);
                document.Add(table);
                string Declarationtexting = "This declaration is given for the " + FY + ".";
                Paragraph Declarartionparagraph9 = new Paragraph();
                Declarartionparagraph9.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                Declarartionparagraph9.Add(Declarationtexting);

                document.Add(Declarartionparagraph9);

                List list = new List(List.ORDERED);
                list.Add(new ListItem("The above PAN is already registered with AEML against the CA number/s mentioned above.", RegularFont));
                list.Add(new ListItem("Any change in the above declaration shall be communicated to AEML.", RegularFont));
                list.Add(new ListItem("If the above declaration is found to be incorrect, AEML will be liable to deduct/collect TDS/TCS at higher rate, as applicable, along with interest/penalty, if applicable.", RegularFont));
                list.Add(new ListItem("Person who is submitting the declaration is authorized and responsible on behalf of the consumer.", RegularFont));
                string texting = "Terms and Conditions:";
                Paragraph paragraph9 = new Paragraph();
                paragraph9.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph9.Add(texting);

                document.Add(paragraph9);
                document.Add(list);
                string text10 = @"I shall indemnify you in case the above disclosure results in false declaration/undertaking.";
                Paragraph paragraph10 = new Paragraph();
                paragraph10.SpacingBefore = 10;
                paragraph10.SpacingAfter = 10;
                paragraph10.Alignment = Element.ALIGN_LEFT;
                paragraph10.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph10.Add(text10);
                document.Add(paragraph10);
                string text11 = @"I hereby, agree to above terms and conditions.";
                Paragraph paragraph11 = new Paragraph();
                paragraph11.SpacingBefore = 10;
                paragraph11.SpacingAfter = 10;
                paragraph11.Alignment = Element.ALIGN_LEFT;
                paragraph11.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                paragraph11.Add(text11);
                document.Add(paragraph11);
                string CurrentDate = DateTime.Now.ToShortDateString();
                string CurrentTime = DateTime.Now.ToShortTimeString();
                Paragraph CurrentTimes = new Paragraph();
                Paragraph CurrentDates = new Paragraph();
                CurrentDates.SpacingBefore = 10;
                CurrentDates.SpacingAfter = 10;
                CurrentDates.Alignment = Element.ALIGN_LEFT;
                CurrentTimes.SpacingBefore = 10;
                CurrentTimes.SpacingAfter = 10;
                CurrentTimes.Alignment = Element.ALIGN_LEFT;
                CurrentDates.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                CurrentDates.Add("Date:" + CurrentDate);
                CurrentTimes.Font = FontFactory.GetFont(FontFactory.HELVETICA, 10f, BaseColor.BLACK);
                CurrentTimes.Add("Time :" + CurrentTime);

                document.Add(CurrentDates);
                document.Add(CurrentTimes);




                document.Close();
                return output.ToArray();
            }
        }
    }
}