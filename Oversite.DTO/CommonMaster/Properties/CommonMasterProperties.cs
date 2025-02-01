
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Oversite.DTO.CommonMaster.Properties
{
    public class CommonMasterProperties
    {
        
        public string commonDataId { get; set; }
        public string commonDataName { get; set; }
        public string desription { get; set; }
        
    }
    public class GetdocProperties
    {
        public string docType { get; set; }
        public string docName { get; set; }
    }
    public class RepaymentScheduleProperties
    {
        [JsonProperty("InstallmentNo")]
        public double INSTALLMENT_NO { get; set; }
        [JsonProperty("DueDate")]
        public DateTime DUE_DATE { get; set; }
        [JsonProperty("InstallmentAmount")]
        public double INSTALLMENT_AMOUNT { get; set; }

        [JsonProperty("PrincipalAmount")]
        public double PRINCIPAL_AMOUNT { get; set; }
        [JsonProperty("InterestAmount")]
        public double INTEREST_AMOUNT { get; set; }

        public double ClosingBalance { get; set; }
        public double OPENING_BALANCE { get; set; }
        public string EFFECTIVE_INTEREST_RATE { get; set; }
        public string effectivedays { get; set; }
    }
    public class RepaymentSumProperties
    {

        [JsonProperty("total_inst_amt")]
        public decimal total_inst_amt { get; set; }
        [JsonProperty("total_pricipal_amt")]
        public decimal total_pricipal_amt { get; set; }
        [JsonProperty("total_intr_amt")]
        public decimal total_intr_amt { get; set; }
    }
    public class CustomerDetailsProperties
    {
        [JsonProperty("CustID")]
        public string CUST_ID { get; set; }
        [JsonProperty("Name")]
        public string NAME { get; set; }
        [JsonProperty("HouseName")]
        public string HOUSE_NAME { get; set; }
        [JsonProperty("AddressLine2")]
        public string ADDRESS_LINE_2 { get; set; }
        [JsonProperty("AddressLine3")]
        public string ADDRESS_LINE_3 { get; set; }
        [JsonProperty("Pincode")]
        public string PINCODE { get; set; }
        [JsonProperty("LoanDate")]
        public DateTime LOAN_DATE { get; set; }
        [JsonProperty("LoanAmount")]
        public string LOAN_AMOUNT { get; set; }
        [JsonProperty("InterestRate")]
        public string INTEREST_RATE { get; set; }
        [JsonProperty("Tenure")]
        public string TENURE { get; set; }
        [JsonProperty("State")]
        public string state_name { get; set; }
        [JsonProperty("District")]
        public string district_name { get; set; }
        [JsonProperty("Country")]
        public string country_name { get; set; }
        [JsonProperty("mobileno")]
        public string mobile_phone { get; set; }
        [JsonProperty("primaryphone")]
        public string primary_phone { get; set; }
        [JsonProperty("emailid")]
        public string primary_email_id { get; set; }
        [JsonProperty("applicationId")]
        public string applicationId { get; set; }
        [JsonProperty("loanId")]
        public string loanId { get; set; }
        [JsonProperty("accIrr")]
        public string accIrr { get; set; }
        [JsonProperty("custIrr")]
        public string custIrr { get; set; }
        [JsonProperty("branch")]
        public string branch { get; set; }
        [JsonProperty("scheme")]
        public string scheme { get; set; }

    }
    public class LoanotherDataPropeties
    {
        public string APPLICATION_ID { get; set; }
        public string INT_RATE_TYPE { get; set; }
        public string repayment_frequency { get; set; }
        public string adv_inst { get; set; }
        public string loan_status { get; set; }
        public string installment_type { get; set; }
        public string product_name { get; set; }
        public string no_of_adv_installments { get; set; }
        public string adv_amount { get; set; }
    }
    public class Repaydecisionproperties
    {
        public string decisionflag { get; set; }
        public string remarks { get; set; }

    }
    public class repayemiproperties
    {
        [JsonProperty("emi")]
        public string emi { get; set; }

        [JsonProperty("instStartDate")]
        public DateTime instStartDate { get; set; }
        [JsonProperty("instEndDate")]
        public DateTime instEndDate { get; set; }
    }
}
