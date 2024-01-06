using ExpertPdf.HtmlToPdf;
using Sitecore.AdaniAirports.Website.Models;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.AdaniAirports.Website.Controllers
{
    public class AdaniAirportsController : Controller
    {

        public ActionResult EnvironmentQuizStartTime(EnvironmentDayQuiz environmentDayQuiz)
        {
            if (Session["QuizStartTime"] == null)
            {
                Session["QuizStartTime"] = DateTime.Now;
                return Json("200", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Operation not allowed");
            }
            
        }
        // GET: AdaniAirports
        [HttpPost]
        public ActionResult EnvironmentQuiz(EnvironmentDayQuiz environmentDayQuiz)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(HttpUtility.HtmlEncode(environmentDayQuiz.ShippingAddress));
            environmentDayQuiz.ShippingAddress = sb.ToString();
            String[] AnswerChoice = { "A", "B", "C", "D" };
            String[] UserType = { "Passenger", "AirportStaff" };
            bool isValid = true;
            int questionCount = 1;
            int quizResult = 0;
            bool validationStatus = true;
            var result = new { status = "1" };
            Log.Error("Validating Adani Airport Airport Quiz Feedback to stop auto script ", "Start");
            AdaniAirportsDataContext rdb = new AdaniAirportsDataContext();

            var AlreadyGiven = rdb.EnvironmentQuizAirports.Where(e => e.PhoneNumber == environmentDayQuiz.PhoneNumber || e.Email == environmentDayQuiz.Email).Any();
            if(AlreadyGiven)
            {
                isValid = false;
                result = new { status = "14" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            if (!String.IsNullOrEmpty(environmentDayQuiz.FirstName))
            {
                if (environmentDayQuiz.FirstName.Length < 3 || (!Regex.IsMatch(environmentDayQuiz.FirstName, (@"^[a-zA-Z ]*$"))))
                {
                    isValid = false;
                    result = new { status = "3" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            if (!String.IsNullOrEmpty(environmentDayQuiz.LastName))
            {
                if (environmentDayQuiz.LastName.Length < 3 || (!Regex.IsMatch(environmentDayQuiz.LastName, (@"^[a-zA-Z ]*$"))))
                {
                    isValid = false;
                    result = new { status = "4" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            if (!String.IsNullOrEmpty(environmentDayQuiz.PhoneNumber))
            {
                if (Regex.IsMatch(environmentDayQuiz.PhoneNumber, (@"^\d+$")))
                {
                    if (environmentDayQuiz.PhoneNumber.Length < 10 || environmentDayQuiz.PhoneNumber.Length > 10)
                    {
                        isValid = false;
                        result = new { status = "5" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (!Regex.IsMatch(environmentDayQuiz.PhoneNumber, (@"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[com]{2,9})$")))
                {

                    isValid = false;
                    result = new { status = "5" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            if (!Regex.IsMatch(environmentDayQuiz.Email, (@"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[com]{2,9})$")))
            {
                isValid = false;
                result = new { status = "6" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (!UserType.Contains(environmentDayQuiz.UserType))
            {
                isValid = false;
                result = new { status = "7" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (environmentDayQuiz.UserType == "Passenger")
                {
                    if (string.IsNullOrEmpty(environmentDayQuiz.City))
                    {
                        isValid = false;
                        result = new { status = "9" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (environmentDayQuiz.UserType == "AirportStaff")
                {
                    if (string.IsNullOrEmpty(environmentDayQuiz.Company))
                    {
                        isValid = false;
                        result = new { status = "8" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }

            }
            if (!AnswerChoice.Contains(environmentDayQuiz.Question1) || !AnswerChoice.Contains(environmentDayQuiz.Question2) || !AnswerChoice.Contains(environmentDayQuiz.Question3) || !AnswerChoice.Contains(environmentDayQuiz.Question4) || !AnswerChoice.Contains(environmentDayQuiz.Question5) || !AnswerChoice.Contains(environmentDayQuiz.Question6) || !AnswerChoice.Contains(environmentDayQuiz.Question7) || !AnswerChoice.Contains(environmentDayQuiz.Question8) || !AnswerChoice.Contains(environmentDayQuiz.Question9) || !AnswerChoice.Contains(environmentDayQuiz.Question10))
            {
                isValid = false;
                result = new { status = "10" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            if (validationStatus == true)
            {
                Log.Error("Quiz Start", "Start");
                Sitecore.Data.Database web = Sitecore.Configuration.Factory.GetDatabase("web");
                Sitecore.Data.Items.Item worldEnvironmentDay = web.GetItem("{2CEA46C4-B441-44B3-9DFC-3DDECECB23B3}");
                foreach (Sitecore.Data.Items.Item child in worldEnvironmentDay.Children)
                {
                    Log.Error("Quiz Question" + child, "Start");

                    string questionNumber = "Question" + questionCount;
                    foreach (Sitecore.Data.Items.Item subchild in child.Children)
                    {
                        Log.Error("Quiz Choice"+ subchild, "");

                        Sitecore.Data.Fields.CheckboxField IsAnswer = subchild.Fields["IsAnswer"];
                        if (IsAnswer != null && IsAnswer.Checked)
                        {
                            string choice = subchild.Fields["AnswerChoice"].ToString();
                            //string modelvalur = environmentDayQuiz + "." + questionCount;
                            #region Find value of models prop Question(questionCount)
                            string usersAnswer = (string)(environmentDayQuiz.GetType().GetProperty("Question" + questionCount).GetValue(environmentDayQuiz));
                            #endregion
                            //Logic Here
                            if (choice == usersAnswer)
                            {

                                quizResult++;
                            }
                        }                        
                    }
                    Log.Error("Quiz Result" + quizResult, "");

                    questionCount++;
                }
                try
                {
                    Log.Error("DB " + environmentDayQuiz, "");

                    EnvironmentQuizAirport quizDetails = new EnvironmentQuizAirport();
                    quizDetails.Id = (rdb.EnvironmentQuizAirports.Select(i => (long?)i.Id).Max() ?? 0) + 1;
                    quizDetails.FirstName = environmentDayQuiz.FirstName;
                    quizDetails.LastName = environmentDayQuiz.LastName;
                    quizDetails.PhoneNumber = environmentDayQuiz.PhoneNumber;
                    quizDetails.Email = environmentDayQuiz.Email;
                    quizDetails.City = environmentDayQuiz.City;
                    quizDetails.UserType = environmentDayQuiz.UserType;
                    quizDetails.Company = environmentDayQuiz.Company;
                    quizDetails.ShippingAddress = environmentDayQuiz.ShippingAddress;
                    quizDetails.Question1 = environmentDayQuiz.Question1;
                    quizDetails.Question2 = environmentDayQuiz.Question2;
                    quizDetails.Question3 = environmentDayQuiz.Question3;
                    quizDetails.Question4 = environmentDayQuiz.Question4;
                    quizDetails.Question5 = environmentDayQuiz.Question5;
                    quizDetails.Question6 = environmentDayQuiz.Question6;
                    quizDetails.Question7 = environmentDayQuiz.Question7;
                    quizDetails.Question8 = environmentDayQuiz.Question8;
                    quizDetails.Question9 = environmentDayQuiz.Question9;
                    quizDetails.Question10 = environmentDayQuiz.Question10;
                    quizDetails.Result = quizResult;
                    quizDetails.SubmittedOn = DateTime.Now;
                    quizDetails.TimeTakenInQuiz = (long)(DateTime.Now -((DateTime)Session["QuizStartTime"])).TotalMilliseconds;


                    Session["FirstName"] = environmentDayQuiz.FirstName;
                    Session["LastName"] = environmentDayQuiz.LastName;
                    Log.Error("Insert to DB " + environmentDayQuiz, "");

                    #region Insert to DB
                    rdb.EnvironmentQuizAirports.InsertOnSubmit(quizDetails);
                    rdb.SubmitChanges();
                    Log.Error("Inserted to DB " + environmentDayQuiz, "");


                }
                catch (Exception ex)
                {
                    Log.Error("Exception Error" + ex, "");

                    result = new { status = "0" };
                    Console.WriteLine(ex);
                }


                /*SendMeetandGreetEmail_Adani(m);
                SendMeetandGreetEmail_customer(m);
                SendMeetandGreetSMS(m);*/


            }
            else
            {
                result = new { status = "2" };
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewECertificate()
        {
            return View();
        }
        //public ActionResult Convert(int userId)
        //{       

        //    PdfConverter pdfConverter = new PdfConverter();
        //    string url = "https://stagecorpairport.adaniairports.com/world-environment-day/certificate?id=" + userId;
        //    return File(pdfConverter.GetPdfBytesFromUrl(url), "application/pdf","ParticipationCertificate.pdf");
        //}
    }
}