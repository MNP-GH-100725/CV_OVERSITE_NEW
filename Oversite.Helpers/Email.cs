using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Mail;
namespace Oversite.Helpers
{
     public class Email
    {
        public void SendMail(string name, string Application, string BranchName,string TypeofCharge,string ChecqueorDD,string BankName,string TransactionDate,string emailId)
        {
            string MailData;
            MailData = "<html><table border='0' cellpadding=0 cellspacing=0><tr><td colspan=1>Mr./Ms " + name + ",</td></tr>";
            MailData = MailData + "<tr><td colspan=3>You have a deposit author function is pending to perform. Please author the same.</td></tr>";
            MailData = MailData + "<tr><td colspan=3>Application No:" + Application + "</td></tr>";
            MailData = MailData + "<tr><td colspan=3>Branch Name:" + BranchName + "</td></tr>";
            MailData = MailData + "<tr><td colspan=3>Type of Charge:" + TypeofCharge + "</td></tr>";
            MailData = MailData + "<tr><td colspan=3>Checque/DD/NEFT No:" + ChecqueorDD + "</td></tr>";
            MailData = MailData + "<tr><td colspan=3>Bank Name:" + BankName + "</td></tr>";
            MailData = MailData + "<tr><td colspan=3>Transaction Date:" + TransactionDate + "</td></tr>";
            MailData = MailData + "</table></br>";
            MailData = MailData + "</center><left>Regards";
            MailData = MailData + "<br>";
            MailData = MailData + "Manappuram Finance Limited, <br>";
            MailData = MailData + "IV / 470 (old) W638A (New), Manappuram House, Valapad,Thrissur <br>";
            MailData = MailData + "Kerala, India,Pin code : 680567 CIN: L65910KL1992PLC006623 1800-420-22-33(toll free) mail@manappuram.com<br>";
            MailData = MailData + "<br></left></html>";
           // var err0 = new ErrorLog().writeLog("-----------------Mail------------");
            try
            {
                string Subject = "Deposit author reminder for  Application:" + Application;
               // var err = new ErrorLog().writeLog("Send Mail Starting-------");
                //var err1 = new ErrorLog().writeLog("Emails:" + email);
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("mail.mactech.net.in");
                mail.From = new MailAddress("mail@macomsolutions.com");
                //string toMailAddresses = new AppConfigManager().GetDepositEmail1+";"+ new AppConfigManager().GetDepositEmail2 + ";"+ new AppConfigManager().GetDepositEmail3 + ";";
                //foreach (var address in toMailAddresses.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                //{
                //    mail.To.Add(address);
                //}
                mail.To.Add(emailId);
                mail.Subject = Subject;
                mail.Body = MailData;
                mail.IsBodyHtml = true;
                SmtpServer.Port = 25;
                SmtpServer.Credentials = new System.Net.NetworkCredential("macommail", "Z$GN_b#D3x");
                //SmtpServer.EnableSsl = true;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.Send(mail);
               // var err2 = new ErrorLog().writeLog("Send Mail Done");
            }
            catch (Exception ex)
            {

               // var err2 = new ErrorLog().writeLog("Exception");
               // var err3 = new ErrorLog().writeLog(ex.Message);
                if (ex.InnerException != null)
                {
                  //  var err21 = new ErrorLog().writeLog("Inner Exception");
                    //var err31 = new ErrorLog().writeLog(ex.InnerException.ToString());
                }
            }
           // var err01 = new ErrorLog().writeLog("---------------Mail End------------");




            //  MailSMS.SendMail("Query based on Application No:"+ Application, MailData, email);
        }
    }
}
