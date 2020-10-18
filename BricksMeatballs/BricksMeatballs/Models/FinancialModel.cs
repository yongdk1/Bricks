using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace BricksMeatballs.Models
{
    public enum PropertyType { HDB, EC, Private }

    public class FinancialModel
    {
        //User input information
        //Applicant Information
        [DisplayName("Applicant Status")]
        public bool ApplicantStatus { get; set; } //Single or Joint application
        [DisplayName("Age")]
        public int Age { get; set; } //Affects LTV Ratio
        [DisplayName("Residency")]
        public bool Residency { get; set; } //Not entirely sure what this affects

        //Property Information
        [DisplayName("Number of Properties")]
        public int NumProperties { get; set; }
        [DisplayName("Number of Home Loans")]
        public int NumLoans { get; set; }

        //Income Information
  
        [DisplayName("Monthly Fixed Income")]
        public double MonthlyFixedIncome { get; set; } //monthly fixed income
        [DisplayName("Monthly Variable Income")]
        public double MonthlyVariableIncome { get; set; } //monthly variable income w 30% discount

        //Funds Information
        [DisplayName("Cash on Hand")]
        public double CashTowardsDownPayment { get; set; } //cash on hand, 5% min
        [DisplayName("CPF ")]
        public double CpfOrdinaryAccount { get; set; } //cpf, cash+cpf 25% min

        //Debt Information
        [DisplayName("Credit Card Minimum Payments")]
        public double CreditMinPayments { get; set; }
        [DisplayName("Car Loan(s) Payments")]
        public double CarLoan { get; set; }
        [DisplayName("House Loan(s) Payments")]
        public double OtherHomeLoan { get; set; }
        [DisplayName("Other Loan(s) Payments")]
        public double OtherLoan { get; set; }

        //Other Information
        [DisplayName("Property Type")]
        public PropertyType PropertyType { get; set; } //HDB, EC, Private
        [DisplayName("Loan Tenure")]
        public double LoanTenure { get; set; } //4 - 35 yrs
        [DisplayName("Interest Rate")]
        public double InterestRate { get; set; } //0.1 - 4%
        
        //Calculate house budget for user based on a 
        //60% total debt servicing ratio,
        //30% MSR limit on HDB/EC
        //25% minimum down payment,
        //5% minimum cash payment
        //Maximum LTV Ratio 55% (ends >65YO, >25 tenure) or 75%
        //BSD: 1% on first 180,000; 2% on second 180,000; 3% on third 640,000; 4% on rest
        //ABSD: SGPOREAN 12% 2nd, 15% rest; PR 5% 1st, 15% rest; FOREIGN 20% rest
        public double TDSRLimit() //60%TDSR Private
        {
            return 0.6 * (this.MonthlyFixedIncome + this.MonthlyVariableIncome * 0.7); 
        }
        public double MSRLimit() //30%MSR HDB/EC
        {
            return 0.3 * (this.MonthlyFixedIncome + this.MonthlyVariableIncome * 0.7); 
        }
        
        public double MaxMonthlyPayment() //monthlyRepayment to pay off loan
        {
            double monthlyDebt = this.CreditMinPayments + this.CarLoan + this.OtherHomeLoan + this.OtherLoan; //Add all (4) sources of debt
            double limitAfterExpenses = this.TDSRLimit() - monthlyDebt;
            if (limitAfterExpenses < 0) limitAfterExpenses = 0;
            if (this.PropertyType == PropertyType.Private) //60%
            {
                return limitAfterExpenses;
            }
            else //30%
            {
                if (limitAfterExpenses < this.MSRLimit()) return limitAfterExpenses;
                return this.MSRLimit();
            }
        }

        public double MaxBankLoan()
        {
            return this.MaxMonthlyPayment() * ((Math.Pow((1 + this.InterestRate / 1200), (this.LoanTenure * 12)) - 1) /
                ((this.InterestRate / 1200) * (Math.Pow((1 + this.InterestRate / 1200), (this.LoanTenure * 12)))));
        }
        
        public double CalculateBudget()
        {
            
            double minDownpaymentCashCPF = 4 * (this.CashTowardsDownPayment + this.CpfOrdinaryAccount); //25%
            double minDownpaymentCash = 20 * this.CashTowardsDownPayment; //5%


            return 0;
        }
    }
}
