using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Sitecore.ICAN.Website.Helper;

namespace Sitecore.ICAN.Website.Models
{
    public class IcanSubmitYourIdeaModel
    {


        public string FirstName_1
        {
            get;
            set;
        }
        public string LastName_1
        {
            get;
            set;
        }
        public DateTime? DOB_1
        {
            get;
            set;
        }
        public string Grade_1
        {
            get;
            set;
        }
        public string FirstName_2
        {
            get;
            set;
        }
        public string LastName_2
        {
            get;
            set;
        }
        public DateTime? DOB_2
        {
            get;
            set;
        }
        public string Grade_2
        {
            get;
            set;
        }
        public string FirstName_3
        {
            get;
            set;
        }

        public string LastName_3
        {
            get;
            set;
        }
        public DateTime? DOB_3
        {
            get;
            set;
        }
        public string Grade_3
        {
            get;
            set;
        }
        public string FirstName_4
        {
            get;
            set;
        }
        public string LastName_4
        {
            get;
            set;
        }
        public DateTime? DOB_4
        {
            get;
            set;
        }
        public string Grade_4
        {
            get;
            set;
        }
        public string GroupName
        {
            get;
            set;
        }
        public string FormName
        {
            get;
            set;
        }

        public string MobileNo
        {
            get;
            set;
        }

        public string EmailID
        {
            get;
            set;
        }


        public string reResponse
        {
            get;
            set;
        }

        public string YouAre
        {
            get;
            set;
        }
        public string TitleForProject
        {
            get;
            set;
        }
        public string TweetLengthIntro
        {
            get;
            set;
        }
        public string ExplainatoryText
        {
            get;
            set;
        }

        public string Institution
        {
            get;
            set;
        }


        public DateTime SubmitOnDate
        {
            get;
            set;
        }


        public string SubmittedBy
        {
            get;
            set;
        }




        public DateTime DateofBirth
        {
            get;
            set;
        }
        public HttpPostedFileBase VideoLink { get; set; }
        public HttpPostedFileBase Image1 { get; set; }
        public HttpPostedFileBase Image2 { get; set; }
        public HttpPostedFileBase Image3 { get; set; }
        public HttpPostedFileBase Image4 { get; set; }
        public HttpPostedFileBase Image5 { get; set; }
        public HttpPostedFileBase Image6 { get; set; }
        public HttpPostedFileBase Image7 { get; set; }
        //public HttpPostedFileBase YLink { get; set; }
        public string YLink
        {
            get;
            set;
        }
        public string CreatedBy
        {
            get;
            set;
        }

        public Guid Id
        {
            get;
            set;
        }
        public string RegistrationNo
        {
            get;
            set;
        }
        public IcanSubmitYourIdeaModel()
        {
            SubmitIdeaHelper helper = new SubmitIdeaHelper();


        }
        public List<ICANSubmitYourIdea> ideas
        {
            get;
            set;
        }

        public ICANSubmitYourIdea idea
        {
            get;
            set;
        }
    }

    public class IcanSubmitYourIdeaListModel
    {
        public List<IcanSubmitYourIdeaModel> ideas
        {
            get;
            set;
        }
        public IcanSubmitYourIdeaListModel()
        {
            this.ideas = new List<IcanSubmitYourIdeaModel>();

        }

    }

    public class IcanSignUpForm
    {
        public int Id
        {
            get;
            set;
        }
        public string RegistrationNo
        {
            get;
            set;
        }
        public string SchoolUDISENumber
        {
            get;
            set;
        }
        public string SchoolAddress
        {
            get;
            set;
        }
        public string PrincipalName
        {
            get;
            set;
        }
        public string PrincipalEmailID
        {
            get;
            set;
        }
        public string SchoolBoard
        {
            get;
            set;
        }
        public string TeamCoordinatorName
        {
            get;
            set;
        }
        public string TeamCoordinatorEmailID
        {
            get;
            set;
        }
        public string TeamCoordinatorMobileNumber
        {
            get;
            set;
        }
        public string OTP
        {
            get;
            set;
        }
        public string SchoolCoordinateNumber
        {
            get;
            set;
        }
        public string GroupName
        {
            get;
            set;
        }
        public DateTime? ApplicationDate
        {
            get;
            set;
        }
        public int? ReviewedCount
        {
            get;
            set;
        }
        public List<IcanSignUpForm> jury
        {
            get;
            set;
        }
        public IcanSignUpForm()
        {
            this.jury = new List<IcanSignUpForm>();
        }
       
    }


    public class IcanSignINForm
    {
        public int Id
        {
            get;
            set;
        }
        public string SchoolCoordinateNumber
        {
            get;
            set;
        }
    }

    public class IcanJurySignINForm
    {
        public int Id
        {
            get;
            set;
        }
        public string JuryMobileNumber
        {
            get;
            set;
        }
        public string OTP
        {
            get;
            set;
        }
    }

    public class ICANJuryScoreModel
    {
        public int Id
        {
            get;
            set;
        }
        public string RegistrationNo
        {
            get;
            set;
        }
        public int? CreativityScore
        {
            get;
            set;
        }
        public int? DetailingScore
        {
            get;
            set;
        }
        public int? ScientificScore
        {
            get;
            set;
        }
        public int? PermanencyScore
        {
            get;
            set;
        }
        public int? ClimateScore
        {
            get;
            set;
        }
        public int? OverallPresentationScore
        {
            get;
            set;
        }
        public int? TotalScore
        {
            get;
            set;
        }
        public string GroupName
        {
            get;
            set;
        }
        public string Notes
        {
            get;
            set;
        }
        public string JuryMobileNumber
        {
            get;
            set;
        }
        public DateTime? CreatedOn
        {
            get;
            set;
        }
        public DateTime? ModifiedOn
        {
            get;
            set;
        }
        public string SchoolUDISENumber
        {
            get;
            set;
        }
        public string SchoolAddress
        {
            get;
            set;
        }
        public string PrincipalName
        {
            get;
            set;
        }
        public string PrincipalEmailID
        {
            get;
            set;
        }
        public string SchoolBoard
        {
            get;
            set;
        }
        public string TeamCoordinatorName
        {
            get;
            set;
        }
        public string TeamCoordinatorEmailID
        {
            get;
            set;
        }
        public string TeamCoordinatorMobileNumber
        {
            get;
            set;
        }
        public DateTime? DateOfApplication
        {
            get;
            set;
        }
        public string DateOfApplication1
        {
            get;
            set;
        }
        public string StudentName1
        {
            get;
            set;
        }
        public DateTime? StudentDOB1
        {
            get;
            set;
        }
        public string Student1DOB
        {
            get;
            set;
        }
        public string StudentGrade1
        {
            get;
            set;
        }
        public string StudentName2
        {
            get;
            set;
        }
        public DateTime? StudentDOB2
        {
            get;
            set;
        }
        public string Student2DOB
        {
            get;
            set;
        }
        public string StudentGrade2
        {
            get;
            set;
        }
        public string StudentName3
        {
            get;
            set;
        }
        public DateTime? StudentDOB3
        {
            get;
            set;
        }
        public string Student3DOB
        {
            get;
            set;
        }
        public string StudentGrade3
        {
            get;
            set;
        }
        public string StudentName4
        {
            get;
            set;
        }
        public DateTime? StudentDOB4
        {
            get;
            set;
        }
        public string Student4DOB
        {
            get;
            set;
        }
        public string StudentGrade4
        {
            get;
            set;
        }
        public string ProjectTitle
        {
            get;
            set;
        }
        public string ExplanatoryText
        {
            get;
            set;
        }
        public string Image1
        {
            get;
            set;
        }
        public string Image2
        {
            get;
            set;
        }
        public string PDF
        {
            get;
            set;
        }
        public string YouTubeLink
        {
            get;
            set;
        }
        public string Video
        {
            get;
            set;
        }
        public List<ICANJuryScoreModel> juryScore
        {
            get;
            set;
        }
        public ICANJuryScoreModel()
        {
            this.juryScore = new List<ICANJuryScoreModel>();
        }
    }

    public class ICANFinaistModel
    {
        public int? Id
        {
            get;
            set;
        }
        public string GroupName
        {
            get;
            set;
        }
       
        public string JuryMobileNumber
        {
            get;
            set;
        }
        public DateTime? CreatedOn
        {
            get;
            set;
        }
        public DateTime? ModifiedOn
        {
            get;
            set;
        }
        public string SchoolUDISENumber
        {
            get;
            set;
        }
        public string ProjectName
        {
            get;
            set;
        }
        public string SchoolName
        {
            get;
            set;
        }
        public string CoordinateName
        {
            get;
            set;
        }
        public string TeamCoordinatorMobileNumber
        {
            get;
            set;
        }
        public string DateOfApplication
        {
            get;
            set;
        }
        public string StudentName1
        {
            get;
            set;
        }
        public string Student1DOB
        {
            get;
            set;
        }
        public string StudentGrade1
        {
            get;
            set;
        }
        public string StudentName2
        {
            get;
            set;
        }
        public DateTime? StudentDOB2
        {
            get;
            set;
        }
        public string Student2DOB
        {
            get;
            set;
        }
        public string StudentGrade2
        {
            get;
            set;
        }
        public string StudentName3
        {
            get;
            set;
        }
        public DateTime? StudentDOB3
        {
            get;
            set;
        }
        public string Student3DOB
        {
            get;
            set;
        }
        public string StudentGrade3
        {
            get;
            set;
        }
        public string StudentName4
        {
            get;
            set;
        }
        public DateTime? StudentDOB4
        {
            get;
            set;
        }
        public string Student4DOB
        {
            get;
            set;
        }
        public string StudentGrade4
        {
            get;
            set;
        }
        public string ProjectTitle
        {
            get;
            set;
        }
        public string ExplanatoryText
        {
            get;
            set;
        }
        public string Image1
        {
            get;
            set;
        }
        public string Image2
        {
            get;
            set;
        }
        public string PDF
        {
            get;
            set;
        }
        public string YouTubeLink
        {
            get;
            set;
        }
        public string Video
        {
            get;
            set;
        }
        public List<ICANFinaistModel> ICANFinalist
        {
            get;
            set;
        }
        public ICANFinaistModel()
        {
            this.ICANFinalist = new List<ICANFinaistModel>();
        }
    }

}