using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.Response
{
    public class Response<T> where T : class
    {
        public Response()
        {
            this.status = ResponseTypeContants.FAIL;
            this.apiStatus = ApiStatusConstants.NOT_COMPLETED;
            this.responseMsg= ResponseTypeContants.FAIL;
        }
		
		public string status { get; set; }
        public string apiStatus { get; set; }
        public string responseMsg { get; set; }
        public T Data { get; set; }
        public ErrorList ErrorList
        {
            get;
            set;
        }
        public void SetError(string errorMessage, string errorCode)
        {
            this.status = "ERROR";
            if (this.ErrorList == null)
            {
                this.ErrorList = new ErrorList();
            }
            this.ErrorList.Add(new Error()
            {
                ErrorCode = errorCode,
                //ErrorMessage = errorMessage,
                ErrorMessage = "Invalid Request",
                ErrorType = "ERROR"
            });
        }

        public void SetErrorList(ErrorList errorList)
        {
            this.status = "ERROR";
            if (this.ErrorList == null)
            {
                this.ErrorList = new ErrorList();
            }
            this.ErrorList.AddRange(errorList);
        }

        public void SetProcessingError(string errorMessage)
        {
            this.SetError(errorMessage, "PROCESSING_ERROR");
        }
		public void SetExceptionError(string errorMessage)
		{
			this.SetError(errorMessage, "EXCEPTION_ERROR");
		}
		public void SetRecordNotFoundError(string errorMessage)
        {
            this.SetError(errorMessage, "RECORD_NOT_FOUND");
        }

        public void SetValidationError(string errorMessage)
        {
            this.SetError(errorMessage, "VALIDATION_ERROR");
        }
    }
}
