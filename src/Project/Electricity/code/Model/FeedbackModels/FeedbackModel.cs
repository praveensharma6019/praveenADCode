namespace Sitecore.Electricity.Website.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Foundation.Dictionary.Repositories;

    [Serializable]
    public class FeedbackModel
    {
        public FeedbackModel()
        {
            #region Rating
            Rating = new List<RatingDetails>();
            Rating.Add(new RatingDetails
            {
                RatingClass = "Excellent",
                RatingName = "Excellent",
                RatingValue = 5
            });
            Rating.Add(new RatingDetails
            {
                RatingClass = "Good",
                RatingName = "Good",
                RatingValue = 4
            });
            Rating.Add(new RatingDetails
            {
                RatingClass = "Average",
                RatingName = "Average",
                RatingValue = 3
            });
            Rating.Add(new RatingDetails
            {
                RatingClass = "Poor",
                RatingName = "Poor",
                RatingValue = 2
            });
            Rating.Add(new RatingDetails
            {
                RatingClass = "Very-Poor",
                RatingName = "Very Poor",
                RatingValue = 1
            });

            #endregion

            #region Attitude_Empathy_Unsatisfied_List

            Attitude_Empathy_Unsatisfied_List = new List<CheckBoxes>();
            Attitude_Empathy_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 5,
                Text = "Was not courteous",
                Checked = false
            });
            Attitude_Empathy_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 4,
                Text = "Did not have a professional approach",
                Checked = false
            });
            Attitude_Empathy_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 3,
                Text = "Unhelpful attitude",
                Checked = false
            });
            Attitude_Empathy_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 2,
                Text = "wasn’t customer centric",
                Checked = false
            });
            Attitude_Empathy_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 1,
                Text = "Wasn’t active listener",
                Checked = false
            });

            #endregion

            #region Quality_Unsatisfied_List

            Quality_Unsatisfied_List = new List<CheckBoxes>();
            Quality_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 5,
                Text = "Call landed to other CSR without consent",
                Checked = false
            });
            Quality_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 4,
                Text = "Hold time was long",
                Checked = false
            });
            Quality_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 3,
                Text = "Difficult to communicate",
                Checked = false
            });
            Quality_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 2,
                Text = "Voice was not clear",
                Checked = false
            });
            Quality_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 1,
                Text = "Took time to connect",
                Checked = false
            });
            #endregion Quality_Unsatisfied_List

            #region Process_Unsatisfied_List
            Process_Unsatisfied_List = new List<CheckBoxes>();
            Process_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 5,
                Text = "Tedious",
                Checked = false
            });
            Process_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 4,
                Text = "Time Consuming",
                Checked = false
            });
            Process_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 3,
                Text = "Multiple Documentation",
                Checked = false
            });
            Process_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 2,
                Text = "Unwanted followup",
                Checked = false
            });
            Process_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 1,
                Text = "not streamlined",
                Checked = false
            });

            #endregion

            #region Number_Contacted_List

            Number_Contacted_List = new List<RatingDetails>();
            Number_Contacted_List.Add(new RatingDetails
            {
                RatingName = "Once",
                RatingValue = 1
            });
            Number_Contacted_List.Add(new RatingDetails
            {
                RatingName = "Mutiple Calls",
                RatingValue = 2
            });
            #endregion

            #region Concerns_addressed_List

            Concerns_addressed_List = new List<RatingDetails>();
            Concerns_addressed_List.Add(new RatingDetails
            {
                RatingName = "Extremely satisfied",
                RatingValue = 1
            });
            Concerns_addressed_List.Add(new RatingDetails
            {
                RatingName = "Somewhat satisfied",
                RatingValue = 2
            });
            Concerns_addressed_List.Add(new RatingDetails
            {
                RatingName = "Neither satisfied nor dissatisfied",
                RatingValue = 3
            });
            Concerns_addressed_List.Add(new RatingDetails
            {
                RatingName = "Somewhat dissatisfied",
                RatingValue = 4
            });
            Concerns_addressed_List.Add(new RatingDetails
            {
                RatingName = "Extremely dissatisfied",
                RatingValue = 5
            });
            #endregion

            #region Ease_of_register_concerns_List

            Ease_of_register_concerns_List = new List<RatingDetails>();
            Ease_of_register_concerns_List.Add(new RatingDetails
            {
                RatingName = "Strongly Agree",
                RatingValue = 1
            });
            Ease_of_register_concerns_List.Add(new RatingDetails
            {
                RatingName = "Agree",
                RatingValue = 2
            });
            Ease_of_register_concerns_List.Add(new RatingDetails
            {
                RatingName = "Neutral",
                RatingValue = 3
            });
            Ease_of_register_concerns_List.Add(new RatingDetails
            {
                RatingName = "Disagree",
                RatingValue = 4
            });
            Ease_of_register_concerns_List.Add(new RatingDetails
            {
                RatingName = "Strongly Disagree",
                RatingValue = 5
            });
            #endregion

            #region Informed_alternate_digital_channels

            Informed_alternate_digital_channels_List = new List<RatingDetails>();
            Informed_alternate_digital_channels_List.Add(new RatingDetails
            {
                RatingName = "Yes",
                RatingValue = 1
            });
            Informed_alternate_digital_channels_List.Add(new RatingDetails
            {
                RatingName = "No",
                RatingValue = 2
            });
            Informed_alternate_digital_channels_List.Add(new RatingDetails
            {
                RatingName = "Yes, but it wasn’t clear",
                RatingValue = 3
            });
            Informed_alternate_digital_channels_List.Add(new RatingDetails
            {
                RatingName = "Yes, but looking for detailed information",
                RatingValue = 4
            });
            #endregion

            #region AEML_could_be_doing_differently

            AEML_could_be_doing_differently_List = new List<RatingDetails>();
            AEML_could_be_doing_differently_List.Add(new RatingDetails
            {
                RatingName = "Being empathetic",
                RatingValue = 1
            });
            AEML_could_be_doing_differently_List.Add(new RatingDetails
            {
                RatingName = "Building Trust",
                RatingValue = 2
            });
            //AEML_could_be_doing_differently_List.Add(new RatingDetails
            //{
            //    RatingName = "Somewhat likely",
            //    RatingValue = 3
            //});
            AEML_could_be_doing_differently_List.Add(new RatingDetails
            {
                RatingName = "Creating awareness programs for consumers",
                RatingValue = 4
            });
            AEML_could_be_doing_differently_List.Add(new RatingDetails
            {
                RatingName = "Others/Please specify",
                RatingValue = 5
            });
            #endregion
        }

        public FeedbackModel(string type)
        {
            #region Rating
            Rating = new List<RatingDetails>();
            Rating.Add(new RatingDetails
            {
                RatingClass = "Excellent",
                RatingName = "Excellent",
                RatingValue = 5
            });
            Rating.Add(new RatingDetails
            {
                RatingClass = "Good",
                RatingName = "Good",
                RatingValue = 4
            });
            Rating.Add(new RatingDetails
            {
                RatingClass = "Average",
                RatingName = "Average",
                RatingValue = 3
            });
            Rating.Add(new RatingDetails
            {
                RatingClass = "Poor",
                RatingName = "Poor",
                RatingValue = 2
            });
            Rating.Add(new RatingDetails
            {
                RatingClass = "Very-Poor",
                RatingName = "Very Poor",
                RatingValue = 1
            });

            #endregion

            #region Attitude_Empathy_Unsatisfied_List

            Attitude_Empathy_Unsatisfied_List = new List<CheckBoxes>();
            Attitude_Empathy_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 5,
                Text = "Was not courteous",
                Checked = false
            });
            Attitude_Empathy_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 4,
                Text = "Did not have a professional approach",
                Checked = false
            });
            Attitude_Empathy_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 3,
                Text = "Unhelpful attitude",
                Checked = false
            });
            Attitude_Empathy_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 2,
                Text = "wasn’t customer centric",
                Checked = false
            });
            Attitude_Empathy_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 1,
                Text = "Wasn’t active listener",
                Checked = false
            });

            #endregion

            #region Unpleasent_Intraction_with_List

            Unpleasent_Intraction_with_List = new List<CheckBoxes>();

            Unpleasent_Intraction_with_List.Add(new CheckBoxes
            {
                Value = 4,
                Text = "House Keeping",
                Checked = false
            });
            Unpleasent_Intraction_with_List.Add(new CheckBoxes
            {
                Value = 3,
                Text = "Officer",
                Checked = false
            });
            Unpleasent_Intraction_with_List.Add(new CheckBoxes
            {
                Value = 2,
                Text = "Cashier",
                Checked = false
            });
            Unpleasent_Intraction_with_List.Add(new CheckBoxes
            {
                Value = 1,
                Text = "Security Guard",
                Checked = false
            });

            #endregion

            #region Number_Contacted_List

            Number_Contacted_List = new List<RatingDetails>();
            Number_Contacted_List.Add(new RatingDetails
            {
                RatingName = "Once",
                RatingValue = 1
            });
            Number_Contacted_List.Add(new RatingDetails
            {
                RatingName = "Mutiple Calls",
                RatingValue = 2
            });
            #endregion

            #region Quality_Unsatisfied_List

            switch (type)
            {
                case "3":
                    #region CC
                    Quality_Unsatisfied_List = new List<CheckBoxes>();
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 5,
                        Text = "Call landed to other CSR without consent",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 4,
                        Text = "Hold time was long",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 3,
                        Text = "Difficult to communicate",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 2,
                        Text = "Voice was not clear",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 1,
                        Text = "Took time to connect",
                        Checked = false
                    });

                    break;
                #endregion
                case "4":
                    #region CCC
                    Quality_Unsatisfied_List = new List<CheckBoxes>();
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 4,
                        Text = "Poor communication",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 3,
                        Text = "Was not able to solve my problem",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 2,
                        Text = "Could not understand my problem",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 1,
                        Text = "Lack of subject knowledge",
                        Checked = false
                    });

                    #region Number_Contacted_List

                    Number_Contacted_List = new List<RatingDetails>();
                    Number_Contacted_List.Add(new RatingDetails
                    {
                        RatingName = "Once",
                        RatingValue = 1
                    });
                    Number_Contacted_List.Add(new RatingDetails
                    {
                        RatingName = "Mutiple Visits",
                        RatingValue = 2
                    });
                    #endregion

                    break;
                #endregion
                case "5":
                    #region Email
                    Quality_Unsatisfied_List = new List<CheckBoxes>();
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 5,
                        Text = "All points were not answered",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 4,
                        Text = "Content wasn’t appropriate",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 3,
                        Text = "Was not able to solve my problem",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 2,
                        Text = "Could not understand my problem",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 1,
                        Text = "Lack of subject knowledge",
                        Checked = false
                    });

                    #region Number_Contacted_List

                    Number_Contacted_List = new List<RatingDetails>();
                    Number_Contacted_List.Add(new RatingDetails
                    {
                        RatingName = "Once",
                        RatingValue = 1
                    });
                    Number_Contacted_List.Add(new RatingDetails
                    {
                        RatingName = "Mutiple Emails",
                        RatingValue = 2
                    });
                    #endregion
                    break;
                #endregion
                case "6":
                    #region Digital
                    Quality_Unsatisfied_List = new List<CheckBoxes>();
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 5,
                        Text = "Others/Please specify",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 4,
                        Text = "Could not understand my problem",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 3,
                        Text = "Lack of subject knowledge",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 2,
                        Text = "Irrelevant Response",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 1,
                        Text = "Delayed Response",
                        Checked = false
                    });

                    break;
                #endregion
                case "7":
                    #region Backend
                    Quality_Unsatisfied_List = new List<CheckBoxes>();
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 5,
                        Text = "Others/Please specify",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 4,
                        Text = "Could not understand my problem",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 3,
                        Text = "Lack of subject knowledge",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 2,
                        Text = "Irrelevant Response",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 1,
                        Text = "Delayed Response",
                        Checked = false
                    });
                    break;

                #endregion
                default:
                    #region default
                    Quality_Unsatisfied_List = new List<CheckBoxes>();
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 5,
                        Text = "Call landed to other CSR without consent",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 4,
                        Text = "Hold time was long",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 3,
                        Text = "Difficult to communicate",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 2,
                        Text = "Voice was not clear",
                        Checked = false
                    });
                    Quality_Unsatisfied_List.Add(new CheckBoxes
                    {
                        Value = 1,
                        Text = "Took time to connect",
                        Checked = false
                    });

                    break;
                    #endregion
            }

            #endregion Quality_Unsatisfied_List

            #region Process_Unsatisfied_List
            Process_Unsatisfied_List = new List<CheckBoxes>();
            Process_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 5,
                Text = "Tedious",
                Checked = false
            });
            Process_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 4,
                Text = "Time Consuming",
                Checked = false
            });
            Process_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 3,
                Text = "Multiple Documentation",
                Checked = false
            });
            Process_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 2,
                Text = "Unwanted followup",
                Checked = false
            });
            Process_Unsatisfied_List.Add(new CheckBoxes
            {
                Value = 1,
                Text = "not streamlined",
                Checked = false
            });

            #endregion

           

            #region Concerns_addressed_List

            Concerns_addressed_List = new List<RatingDetails>();
            Concerns_addressed_List.Add(new RatingDetails
            {
                RatingName = "Extremely satisfied",
                RatingValue = 1
            });
            Concerns_addressed_List.Add(new RatingDetails
            {
                RatingName = "Somewhat satisfied",
                RatingValue = 2
            });
            Concerns_addressed_List.Add(new RatingDetails
            {
                RatingName = "Neither satisfied nor dissatisfied",
                RatingValue = 3
            });
            Concerns_addressed_List.Add(new RatingDetails
            {
                RatingName = "Somewhat dissatisfied",
                RatingValue = 4
            });
            Concerns_addressed_List.Add(new RatingDetails
            {
                RatingName = "Extremely dissatisfied",
                RatingValue = 5
            });
            #endregion

            #region Ease_of_register_concerns_List

            Ease_of_register_concerns_List = new List<RatingDetails>();
            Ease_of_register_concerns_List.Add(new RatingDetails
            {
                RatingName = "Strongly Agree",
                RatingValue = 1
            });
            Ease_of_register_concerns_List.Add(new RatingDetails
            {
                RatingName = "Agree",
                RatingValue = 2
            });
            Ease_of_register_concerns_List.Add(new RatingDetails
            {
                RatingName = "Neutral",
                RatingValue = 3
            });
            Ease_of_register_concerns_List.Add(new RatingDetails
            {
                RatingName = "Disagree",
                RatingValue = 4
            });
            Ease_of_register_concerns_List.Add(new RatingDetails
            {
                RatingName = "Strongly Disagree",
                RatingValue = 5
            });
            #endregion

            #region Informed_alternate_digital_channels

            Informed_alternate_digital_channels_List = new List<RatingDetails>();
            Informed_alternate_digital_channels_List.Add(new RatingDetails
            {
                RatingName = "Yes",
                RatingValue = 1
            });
            Informed_alternate_digital_channels_List.Add(new RatingDetails
            {
                RatingName = "No",
                RatingValue = 2
            });
            Informed_alternate_digital_channels_List.Add(new RatingDetails
            {
                RatingName = "Yes, but it wasn’t clear",
                RatingValue = 3
            });
            Informed_alternate_digital_channels_List.Add(new RatingDetails
            {
                RatingName = "Yes, but looking for detailed information",
                RatingValue = 4
            });
            #endregion

            #region AEML_could_be_doing_differently

            AEML_could_be_doing_differently_List = new List<RatingDetails>();
            AEML_could_be_doing_differently_List.Add(new RatingDetails
            {
                RatingName = "Being empathetic",
                RatingValue = 1
            });
            AEML_could_be_doing_differently_List.Add(new RatingDetails
            {
                RatingName = "Building Trust",
                RatingValue = 2
            });
            //AEML_could_be_doing_differently_List.Add(new RatingDetails
            //{
            //    RatingName = "Somewhat likely",
            //    RatingValue = 3
            //});
            AEML_could_be_doing_differently_List.Add(new RatingDetails
            {
                RatingName = "Creating awareness programs for consumers",
                RatingValue = 4
            });
            AEML_could_be_doing_differently_List.Add(new RatingDetails
            {
                RatingName = "Others/Please specify",
                RatingValue = 5
            });
            #endregion
        }

        public bool IsInputCorrect { get; set; }
        public string ErrorMessage { get; set; }

        public string Captcha { get; set; }

        public string SR_Number { get; set; }
        public string Source { get; set; }
        public string FeedbackType { get; set; }

        public bool AnswerMoreQuestions { get; set; }

        public List<RatingDetails> Rating { get; set; }

        //[Required(ErrorMessage = "Required field")]
        public string OverallExperience { get; set; }

        //[Required(ErrorMessage = "Required field")]
        public string Attitude_Empathy { get; set; }
        public List<CheckBoxes> Attitude_Empathy_Unsatisfied_List { get; set; }
        public string[] Attitude_Empathy_Unsatisfied_Ids { get; set; }

        public string Unpleasent_Intraction_with { get; set; }
        public List<CheckBoxes> Unpleasent_Intraction_with_List { get; set; }
        public string[] Unpleasent_Intraction_with_Ids { get; set; }

        //[Required(ErrorMessage = "Required field")]
        public string Quality { get; set; }
        public List<CheckBoxes> Quality_Unsatisfied_List { get; set; }
        public string[] Quality_Unsatisfied_Ids { get; set; }

        //[Required(ErrorMessage = "Required field")]
        public string Process { get; set; }
        public List<CheckBoxes> Process_Unsatisfied_List { get; set; }
        public string[] Process_Unsatisfied_Ids { get; set; }

        public string Recomandation_scale_Adani_Electricity { get; set; }

        public List<RatingDetails> Number_Contacted_List { get; set; }
        public string Number_Contacted { get; set; }

        public List<RatingDetails> Concerns_addressed_List { get; set; }
        public string Concerns_addressed { get; set; }

        public List<RatingDetails> Ease_of_register_concerns_List { get; set; }
        public string Ease_of_register_concerns { get; set; }

        public List<RatingDetails> Informed_alternate_digital_channels_List { get; set; }
        public string Informed_alternate_digital_channels { get; set; }

        public List<RatingDetails> AEML_could_be_doing_differently_List { get; set; }
        public string AEML_could_be_doing_differently { get; set; }
        public string AEML_could_be_doing_differently_Other_Text { get; set; }
    }

    public class RatingDetails
    {
        public int RatingValue { get; set; }
        public string RatingName { get; set; }
        public string RatingClass { get; set; }
    }

    public class CheckBoxes
    {
        public int Value { get; set; }
        public string Text { get; set; }
        public bool Checked { get; set; }
    }

    [Serializable]
    public class FeedbackResult
    {
        public bool IsSuccess { get; set; }
        public string Flag { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string OutputMessage { get; set; }
    }

}