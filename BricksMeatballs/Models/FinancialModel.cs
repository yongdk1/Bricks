using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.ComponentModel;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Globalization;
using System.Threading;
using System.Reflection.Metadata.Ecma335;

namespace BricksMeatballs.Models
{
    public enum ApplicantStatus { Single, Joint }
    /// <summary>
    /// 
    /// </summary>
    public enum Residency { Singaporean, PR, Foreigner }
    public enum PropertyType { HDB, EC, Private }


    /// <summary>
    /// The main Financial class
    /// Used to calculate maximum property price and its required prerequisites
    /// Contains all attributes user is required to input for calculation
    /// Contains all methods for calculating prerequisite information and maximum property price
    /// </summary>
    public class FinancialModel
    {
        //User input information
        //Applicant Information
        [DisplayName("Applicant Status")]
        public ApplicantStatus ApplicantStatus { get; set; } //Single or Joint application
        [DisplayName("Age")]
        public int Age { get; set; } //Affects LTV Ratio
        [DisplayName("Residency")]
        public Residency Residency { get; set; } //Affects BSD and ABSD

        //Property Information
        [DisplayName("Number of Properties")]
        public int NumProperties { get; set; } //Affects BSD and ABSD
        [DisplayName("Number of Home Loans")]
        public int NumLoans { get; set; } //Affects LTV

        //Income Information
        [DisplayName("Monthly Fixed Income")]
        public double MonthlyFixedIncome { get; set; } //monthly fixed income
        [DisplayName("Monthly Variable Income")]
        public double MonthlyVariableIncome { get; set; } //monthly variable income w 30% discount

        //Funds Information
        [DisplayName("Cash on Hand")]
        public double Cash { get; set; } //cash on hand
        
        [DisplayName("CPF Ordinary Account")]
        public double Cpf { get; set; } //cpf
        

        //Debt Information
        [DisplayName("Credit Card Minimum Payments")]
        public double Credit { get; set; }
        [DisplayName("Car Loan(s) Payments")]
        public double CarLoan { get; set; }
        [DisplayName("House Loan(s) Payments")]
        public double OtherHomeLoan { get; set; }
        [DisplayName("Other Loan(s) Payments")]
        public double OtherLoan { get; set; }

        //Other Information
        [DisplayName("Property Type")]
        public PropertyType PropertyType { get; set; } //HDB, EC, Private, Affects LTV
        [DisplayName("Loan Tenure")]
        public int LoanTenure { get; set; } //4 - 35 yrs, Affects LTV
        [DisplayName("Interest Rate")]
        public double InterestRate { get; set; } //0.1 - 4%, Affects maximum bank loan


        //Displays for user input attributes
        public string DoubleDisplay(double d)
        {
            return d.ToString("C0", CultureInfo.CreateSpecificCulture("en-US")); 
        }
        public string LoanTenureDisplay()
        {
            string temp = this.LoanTenure.ToString("F0", CultureInfo.InvariantCulture);
            return (temp + " Years");
        }
        public string InterestRateDisplay()
        {
            double temp = InterestRate / 100;
            return temp.ToString("P1", CultureInfo.InvariantCulture);
        }


        //Calculate house budget for user based on a 
        //60% total debt servicing ratio,
        //30% MSR limit on HDB/EC
        //25% minimum down payment,
        //5% minimum cash payment
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
        public double MaxMonthlyPayment() //Monthly Repayment to pay off loan
        {
            double monthlyDebt = this.Credit + this.CarLoan + this.OtherHomeLoan + this.OtherLoan; //Add all (4) sources of debt
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
            double monthlyIR = this.InterestRate / 1200;
            double LoanTenureMonths = this.LoanTenure * 12;
            return Math.Round(this.MaxMonthlyPayment() * ((Math.Pow((1 + monthlyIR), LoanTenureMonths) - 1) /
                (monthlyIR * Math.Pow((1 + monthlyIR), LoanTenureMonths))));
        }
        public string MSRLimitDisplay()
        {
            if (this.PropertyType == PropertyType.Private)
            {
                return "-";
            }
            else
            {
                return this.DoubleDisplay(this.MSRLimit());
            }
        }
        public double CalculateBSD(double Price)
        {
            double BSD = 0;

            // 1% for first 180,000
            if (Price > 180000)
            {
                BSD += 1800;
                Price -= 180000;

                // 2% for next 180,000
                if (Price > 180000)
                {
                    BSD += 3600;
                    Price -= 180000;

                    // 3% for next 640,000
                    if (Price > 640000)
                    {
                        BSD += 19200;
                        Price -= 640000;

                        // 4% for remaining amount
                        BSD += Price * 0.04;
                    }
                    else
                    {
                        BSD += Price * 0.03;
                    }
                }
                else
                {
                    BSD += Price * 0.02;
                }

            }
            else
            {
                BSD += Price * 0.01;
            }

            return BSD;
        }
        public double CalculateABSDPercentage() //SGPOREAN 12% 2nd, 15% rest; PR 5% 1st, 15% rest; FOREIGN 20% rest
        {
            if (this.Residency == Residency.Singaporean) // 0% first, 12% 2nd, 15% rest
            {
                if (this.NumProperties == 0)
                {
                    return 0;
                }
                else if (this.NumProperties == 1)
                {
                    return 0.12;
                }
                else
                {
                    return 0.15;
                }
            }
            else if (this.Residency == Residency.PR) // 5% first, 15% rest
            {
                if (this.NumProperties == 0)
                {
                    return 0.05;
                }
                else
                {
                    return 0.15;
                }
            }
            else //20% everything for foreigners 
            {
                return 0.2;
            }
        }
        public double CalculateABSD(double Price)
        {
            return this.CalculateABSDPercentage() * Price;
        }

        public double CalculateLTV() 
        {
            int both = this.Age + this.LoanTenure;

            //HDB: 0 loan: (5%)75% < 65, < 25, (10%)55% otherwise; 1 loan: (25%)45% < 65, < 30, (25%)25% otherwise; 2+ loan: (25%)35% < 65, < 30, (25%)15% otherwise
            if (this.PropertyType == PropertyType.HDB)
            {
                switch (this.NumLoans)
                {
                    case 0:
                        if (both < 65 && this.LoanTenure <= 25) return 0.75;
                        else return 0.55;
                    case 1:
                        if (both < 65 && this.LoanTenure <= 25) return 0.45;
                        else return 0.25;
                    default:
                        if (both < 65 && this.LoanTenure <= 25) return 0.35;
                        else return 0.15;
                }
            }

            //Private, EC: 0 loan: (5%)75% < 65, < 30, (10%)55% otherwise; 1 loan: (25%)45% < 65, <30, (25%)25% otherwise; 2+ loan: (25%)35% < 65, < 30, (25%)15% otherwise
            else
            {
                switch (this.NumLoans)
                {
                    case 0:
                        if (both < 65 && this.LoanTenure <= 30) return 0.75;
                        else return 0.55;
                    case 1:
                        if (both < 65 && this.LoanTenure <= 30) return 0.45;
                        else return 0.25;
                    default:
                        if (both < 65 && this.LoanTenure <= 30) return 0.35;
                        else return 0.15;
                }
            }
        }

        public double CalculateCashDownpayment()
        {
            if (this.CalculateLTV() == 0.75)
            {
                return 0.05;
            } 
            else if (this.CalculateLTV() == 0.55)
            {
                return 0.10;
            }
            else
            {
                return 0.25;
            }
        }

        public string LTVDisplay()
        {
            return this.CalculateLTV().ToString("P0", CultureInfo.InvariantCulture);
        }

        public double CalculateTrueMax()
        {
            double TrueMax;
            double ABSDPercent = this.CalculateABSDPercentage();
            double LTV = this.CalculateLTV();
            double both = this.Cash + this.Cpf;

            //done
            if (LTV == 0.75)
            {
                // If Cash is limiting factor
                if ((this.Cash / (0.06 + ABSDPercent)) < 180000)
                {
                    TrueMax = this.Cash / (0.06 + ABSDPercent);
                }
                else if ((this.Cash + 1800) / (0.07 + ABSDPercent) < 360000)
                {
                    TrueMax = (this.Cash + 1800) / (0.07 + ABSDPercent);
                }
                else if ((this.Cash + 5400) / (0.08 + ABSDPercent) < 1000000)
                {
                    TrueMax = (this.Cash + 5400) / (0.08 + ABSDPercent);
                }
                else
                {
                    TrueMax = (this.Cash + 15400) / (0.09 + ABSDPercent);
                }

                // If Cpf is limiting factor
                if (both < 0.25 * TrueMax)
                {
                    if ((both / (0.26 + ABSDPercent)) < 180000)
                    {
                        TrueMax = both / (0.26 + ABSDPercent);
                    }
                    else if ((both + 1800) / (0.27 + ABSDPercent) < 360000)
                    {
                        TrueMax = (both + 1800) / (0.27 + ABSDPercent);
                    }
                    else if ((both + 5400) / (0.28 + ABSDPercent) < 1000000)
                    {
                        TrueMax = (both + 5400) / (0.28 + ABSDPercent);
                    }
                    else
                    {
                        TrueMax = (both + 15400) / (0.29 + ABSDPercent);
                    }
                }
            }
            //done
            else if (LTV == 0.55)
            {
                // If Cash is limiting factor
                if ((this.Cash / (0.11 + ABSDPercent)) < 180000)
                {
                    TrueMax = this.Cash / (0.11 + ABSDPercent);
                }
                else if ((this.Cash + 1800) / (0.12 + ABSDPercent) < 360000)
                {
                    TrueMax = (this.Cash + 1800) / (0.12 + ABSDPercent);
                }
                else if ((this.Cash + 5400) / (0.13 + ABSDPercent) < 1000000)
                {
                    TrueMax = (this.Cash + 5400) / (0.13 + ABSDPercent);
                }
                else
                {
                    TrueMax = (this.Cash + 15400) / (0.14 + ABSDPercent);
                }

                // If Cpf is limiting factor
                if (both < 0.45 * TrueMax)
                {
                    if ((both / (0.46 + ABSDPercent)) < 180000)
                    {
                        TrueMax = both / (0.46 + ABSDPercent);
                    }
                    else if ((both + 1800) / (0.47 + ABSDPercent) < 360000)
                    {
                        TrueMax = (both + 1800) / (0.47 + ABSDPercent);
                    }
                    else if ((both + 5400) / (0.48 + ABSDPercent) < 1000000)
                    {
                        TrueMax = (both + 5400) / (0.48 + ABSDPercent);
                    }
                    else
                    {
                        TrueMax = (both + 15400) / (0.49 + ABSDPercent);
                    }
                }
            }
            //
            else if (LTV == 0.45)
            {
                // If Cash is limiting factor
                if ((this.Cash / (0.26 + ABSDPercent)) < 180000)
                {
                    TrueMax = this.Cash / (0.26 + ABSDPercent);
                }
                else if ((this.Cash + 1800) / (0.27 + ABSDPercent) < 360000)
                {
                    TrueMax = (this.Cash + 1800) / (0.27 + ABSDPercent);
                }
                else if ((this.Cash + 5400) / (0.28 + ABSDPercent) < 1000000)
                {
                    TrueMax = (this.Cash + 5400) / (0.28 + ABSDPercent);
                }
                else
                {
                    TrueMax = (this.Cash + 15400) / (0.29 + ABSDPercent);
                }

                // If Cpf is limiting factor
                if (both < 0.55 * TrueMax)
                {
                    if ((both / (0.56 + ABSDPercent)) < 180000)
                    {
                        TrueMax = both / (0.56 + ABSDPercent);
                    }
                    else if ((both + 1800) / (0.57 + ABSDPercent) < 360000)
                    {
                        TrueMax = (both + 1800) / (0.57 + ABSDPercent);
                    }
                    else if ((both + 5400) / (0.58 + ABSDPercent) < 1000000)
                    {
                        TrueMax = (both + 5400) / (0.58 + ABSDPercent);
                    }
                    else
                    {
                        TrueMax = (both + 15400) / (0.59 + ABSDPercent);
                    }
                }
            }
            else if (LTV == 0.35)
            {
                // If Cash is limiting factor
                if ((this.Cash / (0.26 + ABSDPercent)) < 180000)
                {
                    TrueMax = this.Cash / (0.26 + ABSDPercent);
                }
                else if ((this.Cash + 1800) / (0.27 + ABSDPercent) < 360000)
                {
                    TrueMax = (this.Cash + 1800) / (0.27 + ABSDPercent);
                }
                else if ((this.Cash + 5400) / (0.28 + ABSDPercent) < 1000000)
                {
                    TrueMax = (this.Cash + 5400) / (0.28 + ABSDPercent);
                }
                else
                {
                    TrueMax = (this.Cash + 15400) / (0.29 + ABSDPercent);
                }

                // If Cpf is limiting factor
                if (both < 0.65 * TrueMax)
                {
                    if ((both / (0.66 + ABSDPercent)) < 180000)
                    {
                        TrueMax = both / (0.66 + ABSDPercent);
                    }
                    else if ((both + 1800) / (0.67 + ABSDPercent) < 360000)
                    {
                        TrueMax = (both + 1800) / (0.67 + ABSDPercent);
                    }
                    else if ((both + 5400) / (0.68 + ABSDPercent) < 1000000)
                    {
                        TrueMax = (both + 5400) / (0.68 + ABSDPercent);
                    }
                    else
                    {
                        TrueMax = (both + 15400) / (0.69 + ABSDPercent);
                    }
                }
            }
            else if (LTV == 0.25)
            {
                // If Cash is limiting factor
                if ((this.Cash / (0.26 + ABSDPercent)) < 180000)
                {
                    TrueMax = this.Cash / (0.26 + ABSDPercent);
                }
                else if ((this.Cash + 1800) / (0.27 + ABSDPercent) < 360000)
                {
                    TrueMax = (this.Cash + 1800) / (0.27 + ABSDPercent);
                }
                else if ((this.Cash + 5400) / (0.28 + ABSDPercent) < 1000000)
                {
                    TrueMax = (this.Cash + 5400) / (0.28 + ABSDPercent);
                }
                else
                {
                    TrueMax = (this.Cash + 15400) / (0.29 + ABSDPercent);
                }

                // If Cpf is limiting factor
                if (both < 0.75 * TrueMax)
                {
                    if ((both / (0.76 + ABSDPercent)) < 180000)
                    {
                        TrueMax = both / (0.76 + ABSDPercent);
                    }
                    else if ((both + 1800) / (0.77 + ABSDPercent) < 360000)
                    {
                        TrueMax = (both + 1800) / (0.77 + ABSDPercent);
                    }
                    else if ((both + 5400) / (0.78 + ABSDPercent) < 1000000)
                    {
                        TrueMax = (both + 5400) / (0.78 + ABSDPercent);
                    }
                    else
                    {
                        TrueMax = (both + 15400) / (0.79 + ABSDPercent);
                    }
                }
            }
            else
            {
                // If Cash is limiting factor
                if ((this.Cash / (0.26 + ABSDPercent)) < 180000)
                {
                    TrueMax = this.Cash / (0.26 + ABSDPercent);
                }
                else if ((this.Cash + 1800) / (0.27 + ABSDPercent) < 360000)
                {
                    TrueMax = (this.Cash + 1800) / (0.27 + ABSDPercent);
                }
                else if ((this.Cash + 5400) / (0.28 + ABSDPercent) < 1000000)
                {
                    TrueMax = (this.Cash + 5400) / (0.28 + ABSDPercent);
                }
                else
                {
                    TrueMax = (this.Cash + 15400) / (0.29 + ABSDPercent);
                }

                // If Cpf is limiting factor
                if (both < 0.85 * TrueMax)
                {
                    if ((both / (0.86 + ABSDPercent)) < 180000)
                    {
                        TrueMax = both / (0.86 + ABSDPercent);
                    }
                    else if ((both + 1800) / (0.87 + ABSDPercent) < 360000)
                    {
                        TrueMax = (both + 1800) / (0.87 + ABSDPercent);
                    }
                    else if ((both + 5400) / (0.88 + ABSDPercent) < 1000000)
                    {
                        TrueMax = (both + 5400) / (0.88 + ABSDPercent);
                    }
                    else
                    {
                        TrueMax = (both + 15400) / (0.89 + ABSDPercent);
                    }
                }
            }

            //if bank loan is limiting factor
            if (both + this.MaxBankLoan() < TrueMax)
            {
                TrueMax = both + this.MaxBankLoan();
            }

            return TrueMax;
        }

        public double BSD()
        {
            return CalculateBSD(this.CalculateTrueMax());
        }

        public double ABSD()
        {
            return CalculateABSD(this.CalculateTrueMax());
        }
        
        public double CashDownpayment()
        {
            return this.CalculateTrueMax() * this.CalculateCashDownpayment();
        }
        public double CashCpfDownpayment()
        {
            return this.CalculateTrueMax() * (1 - this.CalculateLTV());
        }
    }
}
