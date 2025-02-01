using Microsoft.VisualBasic;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Oversite.DTO.Documents.Properties
{
    public class DocumentProperties
    {
        public string customerName { get; set; }

        public string applicantType { get; set; }

        public string documentType { get; set; }
        public string documentName { get; set; }
        public string documentStatus { get; set; }

        public string remarks { get; set; }

        public string docNo { get; set; }
        public string customerid { get; set; }
        public string applicationId { get; set; }
        public string productId { get; set; }
        public string  oldDoc { get;set;}
        public string his { get; set; }
        public decimal sendback_flag { get; set; }

    }
    public class DocumentDataProperties
    {
        public string customerName { get; set; }

        public string productName { get; set; }
        public int productId { get; set; }

        public string schemeName { get; set; }
        public string schemeId { get; set; }

        //[JsonConverter(typeof(DateFormatConverter), "dd-MM-yyyy")]
        public string loanDate { get; set; }

        public string loanAmount { get; set; }
        public string branch { get; set; }
        public string applicationId { get; set; }
        public string loanID{ get; set; }

        public int decisionId { get; set; }
        public int categoryId { get; set; }
    }
    public class PrimaryDataProperties
    {
        public string customerName { get; set; }
        
        public string applicationId { get; set; }
        public string loanId { get; set; }
        public string productName { get; set; }
        
        public string productId { get; set; }

        public string schemeName { get; set; }
        public int schemeId { get; set; }

        public DateTime loanDate { get; set; }

        public string loanAmount { get; set; }
        public string branch { get; set; }
        public string customerId { get; set; }
        public string applicantType { get; set; }
        public string applicantTypeId { get; set; }

        public string docName { get; set; }
        public string docType { get; set; }
        public string docNo { get; set; }
        public string remarks { get; set; }
        public string nohRemarks { get; set; }

        public string nchRemarks { get; set; }

     

    }
    public class FinnonecustProperties
    {
        public string applicantType { get; set; }
        public string applicantTypedesc { get; set; }
        public string customerId { get; set; }
        public string customerName { get; set; }
    }
    public class DocUploadProperties
    {
        public string applicationId { get; set; }
        public string loanId { get; set; }
        public string documentType { get; set; }
        public string docNo { get; set; }
        [Required(ErrorMessage = "Document is required")]
        public string Document { get; set; }
        [Required(ErrorMessage = "Document Filr format is required")]
        [RegularExpression(@"^([a-zA-Z.]*)+$", ErrorMessage = "Please use a valid document file format")]
        public string DocExtension { get; set; }
        public string EnterBy { get; set; }
        //[Required(ErrorMessage = "Applicant Type is required")]
        public string applicantType { get; set; }
        public int ProductID { get; set; }
        public string customerId { get; set; }
    }
    public class OvrsiteDocUpadteProperties
    {
        public string applicationId { get; set; }
        public string customerId { get; set; }
        public string documentType { get; set; }
        public string productId { get; set; }
        public string enterBy { get; set; }
        public string loanId { get; set; }


    }
    public class Adddocproperties
    {
        public decimal lms { get; set; }
        public decimal status { get; set; }
        public int product_id { get; set; }
    }
    public class GetDocumentProperties
    {
       
        public ObjectId Id { get; set; }
        public string DocumentNo { get; set; }
        public int ApplicationId { get; set; }
        public string Extension { get; set; }
        public byte[] Data { get; set; }
        public int NewDocID { get; set; }
        public int DocID { get; set; }
    }
    public class GetDocumentPropertiesfinal
    {
        public ObjectId Id { get; set; }
        public string DocumentNo { get; set; }
        public string ApplicationId { get; set; }
        public string Extension { get; set; }
        public byte[] Data { get; set; }
        public int NewDocID { get; set; }
        public int DocID { get; set; }
    }
    public class GetDocumentPropertiesfinalnew
    {
        public string Id { get; set; }
        public string DocumentNo { get; set; }
        public string ApplicationId { get; set; }
        public string Extension { get; set; }
        public byte[] Data { get; set; }
        public int NewDocID { get; set; }
        public int DocID { get; set; }
    }
    public class GetDocumentsPropertiesnew
    {

        public ObjectId Id { get; set; }
        public int DocumentNo { get; set; }
        public int ApplicationId { get; set; }
        public string Extension { get; set; }
        public byte[] Data { get; set; }
        public int NewDocID { get; set; }
        public int DocID { get; set; }
    }
    public class GetDocumentsPropertiesnewfin
    {

        public ObjectId Id { get; set; }
        public int DocumentNo { get; set; }
        public string ApplicationId { get; set; }
        public string Extension { get; set; }
        public byte[] Data { get; set; }
        public int NewDocID { get; set; }
        public int DocID { get; set; }
    }
    public class GetDocumentPropertiesnew
    {
      
        public string Id { get; set; }
        public string DocumentNo { get; set; }
        public int ApplicationId { get; set; }
        public string Extension { get; set; }
        public byte[] Data { get; set; }
        public int NewDocID { get; set; }
        public int DocID { get; set; }
    }

    public class DocumentRemarksProperties
    {
        public string nchRemarks { get; set; }
        public string nhoRemarks { get; set; }
        public string rchRemarks { get; set; }
        public string rhoRemarks { get; set; }


    }
    public class ApplicationDocumentProperties
    {
        public ObjectId Id { get; set; }
        public int DocumentNo { get; set; }
        public string ApplicationId { get; set; }
        public string Extension { get; set; }
        public byte[] Data { get; set; }
        public int NewDocID { get; set; }
        public int DocID { get; set; }
    }

    public class ApplicationDocumentPropertiesnew
    {
        public ObjectId Id { get; set; }
        public string DocumentNo { get; set; }
        public string ApplicationId { get; set; }
        public string Extension { get; set; }
        public byte[] Data { get; set; }
        public int NewDocID { get; set; }
        public int DocID { get; set; }
    }
    public class GetImageDocProperties
    {
        public string imageid { get; set; }
        public byte[] documentimage { get; set; }
        public string documentimagetype { get; set; }
    }
    public class VerifiedDocumentProperties
    {
        public string customer_name { get; set; }

        public string applicant_type { get; set; }
        public string document_description { get; set; }
        public string docno { get; set; }

        public string nho_verify_by { get; set; }
        public string nho_verified_date { get; set; }

        public string nho_verified_remarks { get; set; }

        public string rho_verify_by { get; set; }
        public string rho_verified_date { get; set; }
        public string rho_remarks { get; set; }
        public string sendback_by { get; set; }
        public string sendback_dt { get; set; }
        public string sendback_remarks { get; set; }
        public string recaptured_by { get; set; }
        public string recaptured_dt { get; set; }
        public string recaptured_remarks { get; set; }

        public string rho_sendback_by { get; set; }
        public string rho_sendback_date { get; set; }
        public string rho_sendback_remarks { get; set; }

      

    }

}
