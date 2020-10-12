using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BricksMeatballs.Models
{
    public class FinancialModel
    {
        //User input information
        public double monthlyFixedIncome { get; set; } //monthly fixed income
        public double monthlyVariableIncome { get; set; } //monthly variable income w 30% discount
        public double cashTowardsDownPayment { get; set; } //cash on hand, 5% min
        public double cpfOrdinaryAccount { get; set; } //cpf, cash+cpf 25% min
        public double creditMinPayments { get; set; }
        public double carLoan { get; set; }
        public double otherHomeLoan { get; set; }
        public double otherLoan { get; set; }
        public double propertyType { get; set; } //HDB, EC, Private
        public double loanTenure { get; set; } //4 - 35 yrs
        public double maximumAffordableHousePrice { get; set; }
        public int age { get; set; }
        public bool residency { get; set; }

        //Calculate house budget for user based on a 
        //60% total debt servicing ratio,
        //35% MSR limit on HDB/EC
        //25% minimum down payment,
        //5% minimum cash payment
        //Maximum LTV Ratio 55% (ends >65YO, >25 tenure) or 75%
        public double CalculateBudget()
        {
            double maxDebtCommitment = 0.6 * (this.monthlyFixedIncome + this.monthlyVariableIncome * 0.7); //60%TDSR, 30%MSR HDB/EC
            double minDownpayment = 4 * (this.cashTowardsDownPayment + this.cpfOrdinaryAccount); //25%
            double minCashDownpayment = this.cashTowardsDownPayment; //5%

            return maximumAffordableHousePrice;
        }
    }
}
