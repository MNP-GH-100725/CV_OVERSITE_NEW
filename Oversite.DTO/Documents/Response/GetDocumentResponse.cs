using Oversite.DTO.Documents.Properties;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.Documents.Response
{
    public class GetDocumentResponse
    {
        public bool isDataAvailable { get; set; }
        public GetDocumentResponse()
        {
            isDataAvailable = false;
        }
        public string message { get; set; }
        public List<DocumentProperties> documentList { get; set; }
        public List<DocumentDataProperties> LoanList { get; set; }
        public decimal enableFlag { get; set; }
        public decimal popupflag { get; set; }


    }

    public class DocumentResponse
    {
        public bool isDataAvailable { get; set; }
        public DocumentResponse()
        {
            isDataAvailable = false;
        }
        public GetImageDocProperties ImageDatas { get; set; }
        public GetDocumentPropertiesfinal GetDocument{ get; set; }
        
        public List<DocumentRemarksProperties> Remarklist { get; set; }
        public string message { get; set; }
        public int  flag { get; set; }

    }
    public class DocumentResponsenew
    {
        public bool isDataAvailable { get; set; }
        public DocumentResponsenew()
        {
            isDataAvailable = false;
        }
        public GetImageDocProperties ImageDatas { get; set; }
        public GetDocumentPropertiesfinalnew GetDocument { get; set; }
        //public GetDocumentPropertiesnew GetDocument { get; set; }
        public List<DocumentRemarksProperties> Remarklist { get; set; }
        public string message { get; set; }

    }
    public class UpdateDocumentResponse
    {
        //public List<GetDocDetailProperties> DocumentList { get; set; }
        public List<PrimaryDataProperties> PrimaryDataList { get; set; }
        public List<FinnonecustProperties> Finnonecustlist { get; set; }
        public int LMS { get; set; }
    }
    public class AddDocumentResponse
    {
       public string DocNo { get; set; }
    }
    public class GetVerifiedDocumentResponse
    {
        public bool isDataAvailable { get; set; }
        public GetVerifiedDocumentResponse()
        {
            isDataAvailable = false;
        }
        public string message { get; set; }
        public List<VerifiedDocumentProperties> documentList { get; set; }
        public DocumentDataProperties LoanList { get; set; }
    }
} 
