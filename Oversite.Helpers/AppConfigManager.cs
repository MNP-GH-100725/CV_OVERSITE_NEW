using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Oversite.Helpers
{
    public class AppConfigManager
    {
        public static string DbConnectionString;
        public static string DbPasword;
        public static string baseUrl;
        public static string DOMAIN_URL;
        public static string SharedSettings;
        public static string SPath;
        public static string HLConfigApiUrl;
        public static string PublicHomeApiUrl;
        public static string CibilBaseUrl;
        public static string CollectionSMSUrl;
        public static string WelcomletterURL;
        public static string WelcomletterURLForMail;
        public static string ApiBaseUrl;
        public static string AadhaarMaskingUrl;
        public static string OkycDataUrl;
        public static string OkycValidateOtp;
        public static string DepositEmail1;
        public static string DepositEmail2;
        public static string DepositEmail3;
        public static string leadCommonApiPath;
        public static string enableOtp;
        public static string mogodbname;
        public static string EquifaxServiceCustomerId;
        public static string EquifaxServiceUserId;
        public static string EquifaxServicePassword;
        public static string EquifaxServiceMemberNumber;
        public static string EquifaxServiceSecurityCode;
        public static string EquifaxServiceCustRefField;
        public static string EquifaxServiceReferenceUrl;
        public readonly string _mogodbserver = string.Empty;
        //public readonly string _mogodbport = string.Empty;


        public static string tokenDuration;

        public static string Server;
        public static string Port;
        public static string User;
        public static string Password;


        public AppConfigManager()
        {
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();


            DbConnectionString = root.GetSection("ConnectionStrings").GetSection("DbConnection").Value;
            DbPasword = root.GetSection("ConnectionStrings").GetSection("DbPasword").Value;
            HLConfigApiUrl = root.GetSection("ApiUrl").GetSection("HLConfigurationApiUrl").Value;
            CibilBaseUrl = root.GetSection("CibilBaseUrl").Value;
            CollectionSMSUrl = root.GetSection("CollectionSMSUrl").Value;
            WelcomletterURL = root.GetSection("WelcomletterURL").Value;
            WelcomletterURLForMail = root.GetSection("WelcomletterURLForMail").Value;
            OkycDataUrl = root.GetSection("OkycDataUrl").Value;
            OkycValidateOtp = root.GetSection("OkycValidateOtp").Value;
            ApiBaseUrl = root.GetSection("baseUrl").Value;
            DOMAIN_URL = root.GetSection("DOMAIN_URL").Value;
            DepositEmail1 = root.GetSection("DepositEmail1").Value;
            DepositEmail2 = root.GetSection("DepositEmail2").Value;
            DepositEmail3 = root.GetSection("DepositEmail3").Value;
            enableOtp = root.GetSection("enableOtp").Value;
            tokenDuration = root.GetSection("tokenDuration").Value;
            mogodbname = root.GetSection("ConnectionStrings").GetSection("MongoDBName").Value;

            _mogodbserver = root.GetSection("ConnectionStrings").GetSection("MongoServer").Value;
            //_mogodbport = root.GetSection("ConnectionStrings").GetSection("MongoPort").Value;


            EquifaxServiceCustomerId = root.GetSection("EquifaxServiceRequestHeader").GetSection("CustomerId").Value;
            EquifaxServiceUserId = root.GetSection("EquifaxServiceRequestHeader").GetSection("UserId").Value;
            EquifaxServicePassword = root.GetSection("EquifaxServiceRequestHeader").GetSection("Password").Value;
            EquifaxServiceMemberNumber = root.GetSection("EquifaxServiceRequestHeader").GetSection("MemberNumber").Value;
            EquifaxServiceSecurityCode = root.GetSection("EquifaxServiceRequestHeader").GetSection("SecurityCode").Value;
            EquifaxServiceCustRefField = root.GetSection("EquifaxServiceRequestHeader").GetSection("CustRefField").Value;
            EquifaxServiceReferenceUrl = root.GetSection("EquifaxServiceRequestHeader").GetSection("ReferenceUrl").Value;

            Server = root.GetSection("SFTP").GetSection("Server").Value;
            Port = root.GetSection("SFTP").GetSection("Port").Value;
            User = root.GetSection("SFTP").GetSection("User").Value;
            Password = root.GetSection("SFTP").GetSection("Password").Value;
        }
        public string MongoDBSERVER
        {
            get => _mogodbserver;
        }
        //public string MongoDBPort
        //{
        //    get => _mogodbport;
        //}
        public string GettokenDuration
        {
            get => tokenDuration;
        }
        public string GetEquifaxServiceCustomerId
        {
            get => EquifaxServiceCustomerId;
        }
        public string GetEquifaxServiceUserId
        {
            get => EquifaxServiceUserId;
        }
        public string GetEquifaxServicePassword
        {
            get => EquifaxServicePassword;
        }
        public string MongoDBName
        {
            get => mogodbname;
        }
        public string GetEquifaxServiceMemberNumber
        {
            get => EquifaxServiceMemberNumber;
        }
        public string GetEquifaxServiceSecurityCode
        {
            get => EquifaxServiceSecurityCode;
        }
        public string GetEquifaxServiceCustRefField
        {
            get => EquifaxServiceCustRefField;
        }
        public string GetEquifaxServiceReferenceUrl
        {
            get => EquifaxServiceReferenceUrl;
        }

        public string GetenableOtp
        {
            get => enableOtp;
        }
        public string GetleadCommonApiPath
        {
            get => leadCommonApiPath;
        }
        public string GetDepositEmail1
        {
            get => DepositEmail1;
        }
        public string GetDepositEmail2
        {
            get => DepositEmail1;
        }
        public string GetDepositEmail3
        {
            get => DepositEmail3;
        }

        public string GetDOMAIN_URL
        {
            get => DOMAIN_URL;
        }
        public string GetConnectionString
        {
            get => DbConnectionString;
        }
        public string GetCibilBaseUrl
        {
            get => CibilBaseUrl;
        }
        public string GetCollectionSMSUrl
        {
            get => CollectionSMSUrl;
        }
        public string GetWelcomletterURL
        {
            get => WelcomletterURL;
        }
        public string GetWelcomletterURLForMail
        {
            get => WelcomletterURLForMail;
        }
        public string GetDbPasword
        {
            get => DbPasword;
        }
        public string GetHLConfigApiUrl
        {
            get => HLConfigApiUrl;
        }
        public string GetApiBaseUrl
        {
            get => ApiBaseUrl;
        }
        public string GetOkycDataUrl
        {
            get => OkycDataUrl;
        }
        public string GetOKYCValidateOTP
        {
            get => OkycValidateOtp;
        }

        public string GetSFTPServer
        {
            get => Server;
        }
        public string GetSFTPPort
        {
            get => Port;
        }
        public string GetSFTPUser
        {
            get => User;
        }
        public string GetSFTPPassword
        {
            get => Password;
        }
    }
}
