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
    public enum Residency { Singaporean, PR, Foreigner }
    public enum PropertyType { HDB, EC, Private }


    public class FinancialModel
    {
        readonly string specifier = "C0";
        readonly CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

        //User input information
        //Applicant Information
        [DisplayName("Applicant Status")]
        public ApplicantStatus ApplicantStatus { get; set; } //Single or Joint application
        [DisplayName("Age")]
        public int Age { get; set; } //Affects LTV Ratio
        [DisplayName("Residency")]
        public Residency Residency { get; set; } //Not entirely sure what this affects

        //Property Information
        [DisplayName("Number of Properties")]
        public int NumProperties { get; set; }
        [DisplayName("Number of Home Loans")]
        public int NumLoans { get; set; }

        //Income Information

        [DisplayName("Monthly Fixed Income")]
        public double MonthlyFixedIncome { get; set; } //monthly fixed income
        public string MonthlyFixedIncomeDisplay()
        {
            return this.MonthlyFixedIncome.ToString(this.specifier, this.culture);
        }
        [DisplayName("Monthly Variable Income")]
        public double MonthlyVariableIncome { get; set; } //monthly variable income w 30% discount
        public string MonthlyVariableIncomeDisplay()
        {
            return this.MonthlyVariableIncome.ToString(this.specifier, this.culture);
        }

        //Funds Information
        [DisplayName("Cash on Hand")]
        public double Cash { get; set; } //cash on hand, 5% min
        public string CashDisplay()
        {
            return this.Cash.ToString(this.specifier, this.culture);
        }
        [DisplayName("CPF Ordinary Account")]
        public double Cpf { get; set; } //cpf, cash+cpf 25% min
        public string CpfDisplay()
        {
            return this.Cpf.ToString(this.specifier, this.culture);
        }

        //Debt Information
        [DisplayName("Credit Card Minimum Payments")]
        public double Credit { get; set; }
        public string CreditDisplay()
        {
            return this.Credit.ToString(this.specifier, this.culture);
        }
        [DisplayName("Car Loan(s) Payments")]
        public double CarLoan { get; set; }
        public string CarLoanDisplay()
        {
            return this.CarLoan.ToString(this.specifier, this.culture);
        }
        [DisplayName("House Loan(s) Payments")]
        public double OtherHomeLoan { get; set; }
        public string OtherHomeLoanDisplay()
        {
            return this.OtherHomeLoan.ToString(this.specifier, this.culture);
        }
        [DisplayName("Other Loan(s) Payments")]
        public double OtherLoan { get; set; }
        public string OtherLoanDisplay()
        {
            return this.OtherLoan.ToString(this.specifier, this.culture);
        }

        //Other Information
        [DisplayName("Property Type")]
        public PropertyType PropertyType { get; set; } //HDB, EC, Private
        [DisplayName("Loan Tenure")]
        public double LoanTenure { get; set; } //4 - 35 yrs
        public string LoanTenureDisplay()
        {
            string temp = this.LoanTenure.ToString("F0", CultureInfo.InvariantCulture);
            return (temp + " Years");
        }
        [DisplayName("Interest Rate")]
        public double InterestRate { get; set; } //0.1 - 4%
        public string InterestRateDisplay()
        {

            double temp = InterestRate / 100;
            return temp.ToString("P0", CultureInfo.InvariantCulture);
        }

        //Calculate house budget for user based on a 
        //60% total debt servicing ratio,
        //30% MSR limit on HDB/EC
        //25% minimum down payment,
        //5% minimum cash payment
        //Maximum LTV Ratio 55% (ends >65YO, >25 tenure) or 75%
        //BSD: 1% on first 180,000; 2% on second 180,000; 3% on third 640,000; 4% on rest
        //ABSD: SGPOREAN 12% 2nd, 15% rest; PR 5% 1st, 15% rest; FOREIGN 20% rest

        //Calculate maximum bank loan via: TDSR/MSR -> Max Monthly Repayment -> Max Bank Loan
        public double TDSRLimit() //60%TDSR Private
        {
            return 0.6 * (this.MonthlyFixedIncome + this.MonthlyVariableIncome * 0.7);
        }

        public string TDSRLimitDisplay()
        {
            return this.TDSRLimit().ToString(this.specifier, this.culture);
        }

        public double MSRLimit() //30%MSR HDB/EC
        {
            return 0.3 * (this.MonthlyFixedIncome + this.MonthlyVariableIncome * 0.7);
        }

        public string MSRLimitDisplay()
        {
            if (this.PropertyType == PropertyType.HDB)
            {
                return this.MSRLimit().ToString(this.specifier, this.culture);
            }
            else
            {
                return "-";
            }
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
        public string MaxMonthlyPaymentDisplay()
        {
            return this.MaxMonthlyPayment().ToString(this.specifier, this.culture);
        }

        public double MaxBankLoan()
        {
            double monthlyIR = this.InterestRate / 1200;
            double LoanTenureMonths = this.LoanTenure * 12;
            return Math.Round(this.MaxMonthlyPayment() * ((Math.Pow((1 + monthlyIR), LoanTenureMonths) - 1) /
                (monthlyIR * Math.Pow((1 + monthlyIR), LoanTenureMonths))));
        }

        public string MaxBankLoanDisplay()
        {
            return this.MaxBankLoan().ToString(this.specifier, this.culture);
        }


        // Calculate maximum price of property via: CashCPF/Cash Downpayment -> BSD/ABSD Calculation ->  
        
        public double CalculateSoftMax(double Money)
        {
            double SoftMax = Money * 20;
            return SoftMax;
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

        public double CalculateABSD(double Price)
        {
            {
                if (this.Residency == Residency.Singaporean) // 0% first, 12% 2nd, 15% rest
                {
                    if (this.NumProperties == 0)
                    {
                        return 0 * Price;
                    }
                    else if (this.NumProperties == 1)
                    {
                        return 0.12 * Price;
                    }
                    else
                    {
                        return 0.15 * Price;
                    }
                }
                else if (this.Residency == Residency.PR) // 5% first, 15% rest
                {
                    if (this.NumProperties == 0)
                    {
                        return 0.05 * Price;
                    }
                    else
                    {
                        return 0.15 * Price;
                    }
                }
                else //20% everything for foreigners 
                {
                    return 0.2 * Price;
                }
            }
        }

        public double ABSDPercentage() //SGPOREAN 12% 2nd, 15% rest; PR 5% 1st, 15% rest; FOREIGN 20% rest
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

        public double CalculateLTV()
        {
            if (((this.Age + this.LoanTenure) > 65 || this.LoanTenure > 25) && (this.PropertyType == PropertyType.HDB || this.PropertyType == PropertyType.EC))
            {
                return 0.55;
            }
            else
            {
                return 0.75;
            }
        }

        public string LTVDisplay()
        {
            return this.CalculateLTV().ToString("P0", CultureInfo.InvariantCulture);
        }

        public double CalculateTrueMax()
        {
            double TrueMax;
            double ABSDPercent = this.ABSDPercentage();
            



            if (((this.Age + this.LoanTenure) > 65 || this.LoanTenure > 25) && (this.PropertyType == PropertyType.HDB || this.PropertyType == PropertyType.EC))
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
                double both = this.Cash + this.Cpf;
                if (both < 0.25 * TrueMax)
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

                // If Loan is limiting factor
                double loan = this.MaxBankLoan();
                if ((both - CalculateBSD(TrueMax) - CalculateABSD(TrueMax)) < TrueMax - loan)
                {
                    double total = loan + both;

                    if ((total / (1.01 + ABSDPercent)) < 180000)
                    {
                        TrueMax = total / (1.01 + ABSDPercent);
                    }
                    else if ((total + 1800) / (1.02 + ABSDPercent) < 360000)
                    {
                        TrueMax = (total + 1800) / (1.02 + ABSDPercent);
                    }
                    else if ((total + 5400) / (1.03 + ABSDPercent) < 1000000)
                    {
                        TrueMax = (total + 5400) / (1.03 + ABSDPercent);
                    }
                    else
                    {
                        TrueMax = (total + 15400) / (1.04 + ABSDPercent);
                    }
                }
            }
            else
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
                double both = this.Cash + this.Cpf;
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

                // If Loan is limiting factor
                double loan = this.MaxBankLoan();
                if ((both - CalculateBSD(TrueMax) - CalculateABSD(TrueMax)) < TrueMax - loan)
                {
                    double total = loan + both;

                    if ((total / (1.01 + ABSDPercent)) < 180000)
                    {
                        TrueMax = total / (1.01 + ABSDPercent);
                    }
                    else if ((total + 1800) / (1.02 + ABSDPercent) < 360000)
                    {
                        TrueMax = (total + 1800) / (1.02 + ABSDPercent);
                    }
                    else if ((total + 5400) / (1.03 + ABSDPercent) < 1000000)
                    {
                        TrueMax = (total + 5400) / (1.03 + ABSDPercent);
                    }
                    else
                    {
                        TrueMax = (total + 15400) / (1.04 + ABSDPercent);
                    }
                }
            }
            return TrueMax;
        }
        
        public string TrueMaxDisplay()
        {
            return this.CalculateTrueMax().ToString(this.specifier, this.culture);
        }
        
        public string BSDDisplay()
        {
            double BSD = CalculateBSD(this.CalculateTrueMax());
            return BSD.ToString(this.specifier, this.culture);
        }

        public string ABSDDisplay()
        {
            double ABSD = CalculateABSD(this.CalculateTrueMax());
            return ABSD.ToString(this.specifier, this.culture);
        }
    }
}
