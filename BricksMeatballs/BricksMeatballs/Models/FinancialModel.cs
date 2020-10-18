using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace BricksMeatballs.Models
{
    public class FinancialModel
    {
        //User input information
        //Applicant Information
        [DisplayName("Applicant Status")]
        public bool applicantStatus { get; set; } //Single or Joint application
        [DisplayName("Age")]
        public int age { get; set; } //Affects LTV Ratio
        [DisplayName("Residency")]
        public bool residency { get; set; } //Not entirely sure what this affects

        //Property Information
        [DisplayName("Number of Properties")]
        public int numProperties { get; set; }
        [DisplayName("Number of Home Loans")]
        public int numLoans { get; set; }

        //Income Information
  
        [DisplayName("Monthly Fixed Income")]
        public double monthlyFixedIncome { get; set; } //monthly fixed income
        [DisplayName("Monthly Variable Income")]
        public double monthlyVariableIncome { get; set; } //monthly variable income w 30% discount

        //Funds Information
        [DisplayName("Cash on Hand")]
        public double cashTowardsDownPayment { get; set; } //cash on hand, 5% min
        [DisplayName("CPF ")]
        public double cpfOrdinaryAccount { get; set; } //cpf, cash+cpf 25% min

        //Debt Information
        [DisplayName("Credit Card Minimum Payments")]
        public double creditMinPayments { get; set; }
        [DisplayName("Car Loan(s) Payments")]
        public double carLoan { get; set; }
        [DisplayName("House Loan(s) Payments")]
        public double otherHomeLoan { get; set; }
        [DisplayName("Other Loan(s) Payments")]
        public double otherLoan { get; set; }

        //Other Information
        [DisplayName("Property Type")]
        public double propertyType { get; set; } //HDB, EC, Private
        [DisplayName("Loan Tenure")]
        public double loanTenure { get; set; } //4 - 35 yrs
        
        //Calculate house budget for user based on a 
        //60% total debt servicing ratio,
        //35% MSR limit on HDB/EC
        //25% minimum down payment,
        //5% minimum cash payment
        //Maximum LTV Ratio 55% (ends >65YO, >25 tenure) or 75%
        //BSD: 1% on first 180,000; 2% on second 180,000; 3% on third 640,000; 4% on rest
        //ABSD: SGPOREAN 12% 2nd, 15% rest; PR 5% 1st, 15% rest; FOREIGN 20% rest
        public double CalculateBudget()
        {
            double maxDebtCommitment = 0.6 * (this.monthlyFixedIncome + this.monthlyVariableIncome * 0.7); //60%TDSR, 30%MSR HDB/EC
            double minDownpaymentCashCPF = 4 * (this.cashTowardsDownPayment + this.cpfOrdinaryAccount); //25%
            double minDownpaymentCash = 20 * this.cashTowardsDownPayment; //5%


            return 0;
        }
    }
}
