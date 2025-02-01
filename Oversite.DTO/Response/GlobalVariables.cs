using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oversite.DTO.Response
{
    public static class GlobalVariables
    {
        public enum LoginStatus
        {
            validUser = 1,
            inValidUser = 0

        }

        public enum ProcessStatus
        {
            success = 1,
            failed = 0
        }

        public enum APIStatus
        {
            success = 1,
            failed = 0,
            exception = 2,
            pagenotfound = 4,
            internalexception = 5,
            invalidParameter = 6,
            invalidParameterValue = 7,
            SRM_RM_Approval_Required = 8,
            no_Data_Found = 9,
            faileTOGenerateToken = 10,
            validationFailed = 11,
            alreadyExist = 12,
            neftModificationRequired = 13,
            invalidIFSCCode = 14,
            OTP_SentTOClient = 15,
            NoMobileNumber = 16,
            OTP_AlreadySent = 17,
            badRequest = 18
        }

        public static string getAPIResponseMessages(APIStatus status)
        {
            string msg = "";
            switch (status)
            {
                case APIStatus.success:
                    break;
                case APIStatus.failed:
                    break;
                case APIStatus.exception:
                    break;
                case APIStatus.pagenotfound:
                    break;
                case APIStatus.internalexception:
                    break;
                case APIStatus.invalidParameter:
                    break;
                case APIStatus.invalidParameterValue:
                    break;
                case APIStatus.SRM_RM_Approval_Required:
                    break;
                case APIStatus.no_Data_Found:
                    break;
                case APIStatus.faileTOGenerateToken:
                    break;
                default:
                    break;
            }
            return msg;
        }
        public enum FieldLength
        {
            phoneNumber = 10,
            customerID = 14

        }

        public enum LoginFormMode
        {
            Login = 1,
            Add_Inventory = 2,
            GL1_Verification = 3,
            GL2_Verification = 4,
            ABH_Verification = 5,
            BH_Verification = 6,
            Repledge = 7,
            Settlement = 8,
            Inventory_Release = 9,
            FormK = 10,
            CashNeftDisburse = 11,
            MaxValue = 12
        }

        public enum OTPFlags
        {
            Customer_OTP = 1,
            Employee_Code = 2,
            Login_OTP = 3
        }

        public enum FormModeMobile
        {
            Add_Customer = 1,
            Modify_Customer = 2
        }

        public enum Durations
        {
            LoginOTP = 30,
            Token = 30,
        }

        public enum CheckStatus
        {
            Checked = 1,
            notChecked = 0
        }

        public class ResponseStatus
        {
            public ProcessStatus flag { get; set; }
            public APIStatus code { get; set; }
            public string message { get; set; }
            public string timeStamp { get; set; }
        }

        public class Constants
        {
            public static class Strings
            {
                public const string dateFormat = "dd-MMM-yyyy";
                public const string cashFormat = "#.00";
                public const string dateTimeFormat = "dd-MMM-yyyy hh:mm:ss";
                public const string nameFormat = @"^[a-zA-Z\s]+$";
                public const string numberFormat = "^[0-9]+$";
                public const string alphaNumericFormat = "^[a-zA-Z0-9]*$";
                public const string panNoFormat = @"^[A-Z]{5}[0-9]{4}[A-Z]{1}$";
                public const string IfscCodeFormat = @"^[A-Za-z]{4}0[a-zA-Z0-9]{6}$";
                public const string validPanChars = "P,C,F,H,T,A,B,G,J,L";
                public const string emailFormat = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
                public const string mobilenumberFormat = "^([6-9]{1})([0-9]{9})$";
                //public const string passportFormat = "^(?!^0+$)[a-zA-Z0-9]{8}$";
                public const string passportFormat = "^([A-Za-z]{1})([0-9]{7})$";
                public const string adharCardFormat = "[0-9]{12}";
                public const string kycNumberFormat = "^[a-zA-Z0-9/-]*$";

            }

            public static class Ints
            {
                public const int rangeValidatorFrom_0 = 0;
                public const int rangeValidatorTo = int.MaxValue;
                public const int rangeValidatorFrom_1 = 1;
                public const int mobileNumberLength = 10;
                public const int pinSerialLength = 6;
                public const int customerIDLength = 14;
                public const int vehicleLoanNumberLength = 22;
                public const int otpLength = 5;
                public const int empCodeLength = 8;
                public const int empCodeminLength = 4;
                public const int panNumberLength = 10;
                public const int landLineNumberMinLength = 6;
                public const int landLineNumberMaxLength = 15;
                public const int zeroOneMaxValue = 10;
                public const int customer_NRI = 8;
                public const int customer_Normal = 1;
                public const int customer_Recommend = 2;
                public const int smsmessageLength = 160;
            }
            public static class TokenStatus
            {

                public const int open = 1;
                public const int manualExpired = 0;
                public const int autoExpired = 2;
            }

            public static class LoginFlags
            {
                public const int Flag_Success = 1;
                public const int Flag_Failed = 0;
                public const int Flag_NoMobileNumber = 2;
                public const int Flag_OTP_Sent = 3;
                public const int Flag_OTP_Already_Sent = 4;

            }

            public enum SMSAccMode
            {

                transactionsms = 2,
                OtpSms = 3
            }
        }
    }
}
