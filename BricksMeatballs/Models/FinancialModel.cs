using System;
using System.ComponentModel;
using System.Globalization;

namespace BricksMeatballs.Models
{
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
        /// <summary>
        /// User input information used to calculate factors affecting house affordability
        /// </summary>

        //User input information
        //Applicant Information
        /// <summary>
        /// Instance variable <c>Age</c> represents the user's age in years
        /// Used to determine LTV ratio, and affects maximum house price.
        /// </summary>
        [DisplayName("Age (Years Old)")]
        public int Age { get; set; } //Affects LTV Ratio
        /// <summary>
        /// Instance variable <c>Residency</c> represents the user's citizenship status with respect to Singapore.
        /// Used to determine BSD and ABSD values.
        /// </summary>
        [DisplayName("Residency")]
        public Residency Residency { get; set; } //Affects BSD and ABSD

        //Property Information
        /// <summary>
        /// Instance variable <c>NumProperties</c> represents the user's total number of properties owned.
        /// Used to determine BSD and ABSD values.
        /// </summary>
        [DisplayName("Number of Properties")]
        public int NumProperties { get; set; } //Affects BSD and ABSD
        /// <summary>
        /// Instance variable <c>NumLoans</c> represents the user's current number of outstanding housing loans.
        /// Used to determine LTV ratio, and affects maximum house price.
        /// </summary>
        [DisplayName("Number of Home Loans")]
        public int NumLoans { get; set; } //Affects LTV

        //Income Information
        /// <summary>
        /// Instance variable <c>MonthlyFixedIncome</c> represents the user's monthly fixed income.
        /// Used to determine TDSR, MSR (if applicable), and maximum bank loan.
        /// </summary>
        [DisplayName("Monthly Fixed Income")]
        public double MonthlyFixedIncome { get; set; } //monthly fixed income
        /// <summary>
        /// Instance variable <c>VariableFixedIncome</c> represents the user's variable fixed income. Discounted by 30% when used for computation.
        /// Used to determine TDSR, MSR (if applicable), and maximum bank loan.
        /// </summary>
        [DisplayName("Monthly Variable Income")]
        public double MonthlyVariableIncome { get; set; } //monthly variable income w 30% discount

        //Funds Information
        /// <summary>
        /// Instance variable <c>Cash</c> represents the user's currently available cash for a house downpayment.
        /// Used to determine maximum house price, minimally at either 5%, 10% or 25% of maximum house price.
        /// </summary>
        [DisplayName("Cash on Hand")]
        public double Cash { get; set; } //cash on hand
        /// <summary>
        /// Instance variable <c>Cpf</c> represents the user's currently available cpf (ordinary account) for a house downpayment.
        /// Used to determine maximum house price, between 20% to 60%
        /// </summary>
        [DisplayName("CPF")]
        public double Cpf { get; set; } //cpf

        //Debt Information
        /// <summary>
        /// Instance variable <c>Credit</c> represents the user's credit card minimum payments.
        /// Used to determine TDSR, MSR (if applicable), and maximum bank loan.
        /// </summary>
        [DisplayName("Credit Card Minimum Payments")]
        public double Credit { get; set; }
        /// <summary>
        /// Instance variable <c>CarLoan</c> represents the user's payments on car loan(s).
        /// Used to determine TDSR, MSR (if applicable), and maximum bank loan.
        /// </summary>
        [DisplayName("Car Loan Payments")]
        public double CarLoan { get; set; }
        /// <summary>
        /// Instance variable <c>OtherHomeLoan</c> represents the user's payments on other home loan(s).
        /// Used to determine TDSR, MSR (if applicable), and maximum bank loan.
        /// </summary>
        [DisplayName("Other Home Loan Payments")]
        public double OtherHomeLoan { get; set; }
        /// <summary>
        /// Instance variable <c>OtherLoan</c> represents the user's payments on all other loan(s).
        /// Used to determine TDSR, MSR (if applicable), and maximum bank loan.
        /// </summary>
        [DisplayName("Other Loan Payments")]
        public double OtherLoan { get; set; }

        //Other Information
        /// <summary>
        /// Instance variable <c>PropertyType</c> represents the user's desired housing type to be purchased.
        /// Used to determine if MSR is applicable (HDB or EC) or not applicable (Private).
        /// Used to determine LTV value, and affects maximum house price.
        /// </summary>
        [DisplayName("Property Type")]
        public PropertyType PropertyType { get; set; } //HDB, EC, Private, Affects LTV
        /// <summary>
        /// Instance variable <c>LoanTenure</c> represents the user's desired duration of repayment for their home loan.
        /// Used to determine maximum bank loan.
        /// </summary>
        [DisplayName("Loan Tenure (4 Years - 35 Years)")]
        public int LoanTenure { get; set; } //4 - 35 yrs, Affects LTV
        /// <summary>
        /// Instance variable <c>InterestRate</c> represents the anticipated interest rate of their home loan.
        /// Used to determine maximum bank loan.
        /// </summary>
        [DisplayName("Interest Rate (0.1% - 4%)")]
        public double InterestRate { get; set; } //0.1 - 4%, Affects maximum bank loan

        //Display methods
        /// <summary>
        /// Instance Method <c>DoubleDisplay</c> displays user's data in monetary format to 0 d.p.
        /// </summary>
        /// <returns>
        /// A string in the format $XXXX where XXXX is the input double rounded to 0 d.p.
        /// </returns>
        public string DoubleDisplay(double d)
        {
            return d.ToString("C0", CultureInfo.CreateSpecificCulture("en-US")); 
        }
        /// <summary>
        /// Instance Method <c>LoanTenureDisplay</c> displays user's desired loan tenure in years
        /// </summary>
        /// <returns>
        /// A string in the format XX Years where XX is the user's input loan tenure.
        /// </returns>
        public string LoanTenureDisplay()
        {
            string temp = this.LoanTenure.ToString("F0", CultureInfo.InvariantCulture);
            return (temp + " Years");
        }
        /// <summary>
        /// Instance Method <c>MSRLimitDisplay</c> displays interest rate in percentage format to 1 d.p.
        /// </summary>
        /// <returns>
        /// A string in the format XX.X% where XX.X is the user's input interest rate rounded to 1 d.p.
        /// </returns>
        public string InterestRateDisplay()
        {
            double temp = InterestRate / 100;
            return temp.ToString("P1", CultureInfo.InvariantCulture);
        }

        //Calculate financial results

        /// <summary>
        /// Instance Method <c>TDSRLimit</c> returns the user's maximum monthly repayment for their home loan for Private homes.
        /// </summary>
        /// <remarks>
        /// Set at 60% of the sum of their monthly fixed income and their monthly variable income (discounted by 30%).
        /// Used to determine maximum bank loan.
        /// </remarks>
        /// <returns>
        /// The user's Total Debt Servicing Ratio.
        /// </returns>
        public double TDSRLimit() //60%TDSR Private
        {
            return 0.6 * (this.MonthlyFixedIncome + this.MonthlyVariableIncome * 0.7);
        }

        /// <summary>
        /// Instance Method <c>MSRLimit</c> returns the user's maximum monthly repayment for their home loan for HDBs and ECs.
        /// </summary>
        /// <remarks>
        /// Set at 30% of the sum of their monthly fixed income and their monthly variable income (discounted by 30%).
        /// Used to determine maximum bank loan.
        /// </remarks>
        /// <returns>
        /// The user's Mortgage Servicing Ratio.
        /// </returns>
        public double MSRLimit() //30%MSR HDB/EC
        {
            return 0.3 * (this.MonthlyFixedIncome + this.MonthlyVariableIncome * 0.7);
        }

        /// <summary>
        /// Instance Method <c>MaxMonthlyPayment</c> returns the user's maximum monthly repayment for their home loan after taking into account expenses.
        /// </summary>
        /// <remarks>
        /// If the house being bought is private, then it is
        /// Set at TDSR Limit minus total value of debts (Credit Card Minimum Payments, Car Loan(s), Other Home Loan(s), & Other Loan(s))
        /// Otherwise, set at the lower of the calculated value and the MSR Limit.
        /// Used to determine maximum bank loan.
        /// </remarks>
        /// <returns>
        /// The user's maximum monthly repayment for their home loan.
        /// </returns>
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

        /// <summary>
        /// Instance Method <c>MaxBankLoan</c> returns the user's maximum bank loan for their home.
        /// </summary>
        /// <remarks>
        /// Determined by the maximum monthly payment, loan tenure, and interest rate.
        /// </remarks>
        /// <returns>
        /// The user's maximum bank loan for their home.
        /// </returns>
        public double MaxBankLoan()
        {
            double monthlyIR = this.InterestRate / 1200;
            double LoanTenureMonths = this.LoanTenure * 12;
            return Math.Round(this.MaxMonthlyPayment() * ((Math.Pow((1 + monthlyIR), LoanTenureMonths) - 1) /
                (monthlyIR * Math.Pow((1 + monthlyIR), LoanTenureMonths))));
        }

        /// <summary>
        /// Instance Method <c>MSRLimitDisplay</c> displays user's MSR Limit in monetary format to 0 d.p.
        /// </summary>
        /// <returns>
        /// A string in the format $XXXX where XXXX is the user's MSR Limit or '-' if MSR if not applicable.
        /// </returns>
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

        /// <summary>
        /// Instance Method <c>CalculateBSD</c> returns payable buyer stamp duty given a house price.
        /// </summary>
        /// <remarks>
        /// Determined via charging:
        /// 1% on first 180,000; 2% on second 180,000; 3% on third 640,000; 4% on rest of house price
        /// </remarks>
        /// <returns>
        /// Payable Buyer Stamp Duty on an input home price.
        /// </returns>
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

        /// <summary>
        /// Instance Method <c>CalculateABSDPercentage</c> returns payable additional buyer stamp duty as a percentage of maximum house price.
        /// </summary>
        /// <remarks>
        /// Determined from number of properties already owned.
        /// Singaporeans are charged 0% for their 1st house, 12% on their 2nd, and 15% on any additional houses.
        /// PRs are charged 5% for their 1st house, and 15% on any additional houses.
        /// Foreigners 20% on all houses.
        /// </remarks>
        /// <returns>
        /// The user's payable ABSD percentage in the form of a decimal.
        /// </returns>
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
        
        /// <summary>
        /// Instance Method <c>CalculateABSDPercentage</c> returns payable additional buyer stamp duty given a house price.
        /// </summary>
        /// <remarks>
        /// Calculated by multiplying ABSD percentage by house price.
        /// </remarks>
        /// <returns>
        /// Payable Additional Buyer Stamp Duty on an input home price.
        /// </returns>
        public double CalculateABSD(double Price)
        {
            return this.CalculateABSDPercentage() * Price;
        }

        /// <summary>
        /// Instance Method <c>CalculateLTV</c> returns user's maximum loan-to-value ratio.
        /// </summary>
        /// <remarks>
        /// Dependent upon number of current home loans outstanding, the property type desired, the loan tenure, and the age of the user.
        /// Determined via:
        ///     If HDB, 
        ///         if 0 loan:
        ///             if loan tenure and age do not exceed 65 years and loan tenure does not exceed 25 years, LTV is 75%. Otherwise, it is 55%.
        ///         if 1 loan:
        ///             if loan tenure and age do not exceed 65 years and loan tenure does not exceed 25 years, LTV is 45%. Otherwise, it is 25%.
        ///         if 2 loans or more:
        ///             if loan tenure and age do not exceed 65 years and loan tenure does not exceed 25 years, LTV is 35%. Otherwise, it is 15%.
        ///     If EC or Private, 
        ///         if 0 loan:
        ///             if loan tenure and age do not exceed 65 years and loan tenure does not exceed 30 years, LTV is 75%. Otherwise, it is 55%.
        ///         if 1 loan:
        ///             if loan tenure and age do not exceed 65 years and loan tenure does not exceed 30 years, LTV is 45%. Otherwise, it is 25%.
        ///         if 2 loans or more:
        ///             if loan tenure and age do not exceed 65 years and loan tenure does not exceed 30 years, LTV is 35%. Otherwise, it is 15%.
        /// </remarks>
        /// <returns>
        /// The user's maximum LTV ratio.
        /// </returns>
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

        /// <summary>
        /// Instance Method <c>CalculateCashDownpaymentPercentage</c> returns user's minimum required cash downpayment as a percentage of total house price.
        /// </summary>
        /// <remarks>
        /// Determined by maximum loan-to-value ratio.
        /// Determined via:
        ///     If LTV is 0.75, cash downpayment is 5%
        ///     If LTV is 0.55, cash downpayment is 10%
        ///     If LTV is any other value, cash downpayment is 25%
        /// </remarks>
        /// <returns>
        /// The user's minimum cash downpayment as a percentage of total house price.
        /// </returns>
        public double CalculateCashDownpaymentPercentage()
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

        /// <summary>
        /// Instance Method <c>LTVDisplay</c> displays user's maximum loan-to-value ratio in percentage format to 0 d.p.
        /// </summary>
        /// <returns>
        /// A string in the format XX% where XX is the user's maximum LTV ratio.
        /// </returns>
        public string LTVDisplay()
        {
            return this.CalculateLTV().ToString("P0", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Instance Method <c>MSRLimitDisplay</c> displays user's maximum affordable house price.
        /// </summary>
        /// <remarks>
        /// Dependent upon LTV, cash available, cpf available, ABSD, and maximum bank loan.
        /// </remarks>
        /// <returns>
        /// The user's maximum affordable house price.
        /// </returns>
        public double CalculateTrueMax()
        {
            double TrueMax;
            double ABSDPercent = this.CalculateABSDPercentage();
            double LTV = this.CalculateLTV();
            double CashPercent = this.CalculateCashDownpaymentPercentage();
            double both = this.Cash + this.Cpf;

            // If Cash is limiting factor
            if ((this.Cash / (0.01 + CashPercent + ABSDPercent)) < 180000)
            {
                TrueMax = this.Cash / (0.01 + CashPercent + ABSDPercent);
            }
            else if ((this.Cash + 1800) / (0.02 + CashPercent + ABSDPercent) < 360000)
            {
                TrueMax = (this.Cash + 1800) / (0.02 + CashPercent + ABSDPercent);
            }
            else if ((this.Cash + 5400) / (0.03 + CashPercent + ABSDPercent) < 1000000)
            {
                TrueMax = (this.Cash + 5400) / (0.03 + CashPercent + ABSDPercent);
            }
            else
            {
                TrueMax = (this.Cash + 15400) / (0.04 + CashPercent + ABSDPercent);
            }

            // If Cpf is limiting factor
            if (both < (1 - LTV) * TrueMax)
            {
                if ((both / (1.01 - LTV + ABSDPercent)) < 180000)
                {
                    TrueMax = both / (1.01 - LTV + ABSDPercent);
                }
                else if ((both + 1800) / (1.02 - LTV + ABSDPercent) < 360000)
                {
                    TrueMax = (both + 1800) / (1.02 - LTV + ABSDPercent);
                }
                else if ((both + 5400) / (1.03 - LTV + ABSDPercent) < 1000000)
                {
                    TrueMax = (both + 5400) / (1.03 - LTV + ABSDPercent);
                }
                else
                {
                    TrueMax = (both + 15400) / (1.04 - LTV + ABSDPercent);
                }
            }

            //if bank loan is limiting factor
            if (both + this.MaxBankLoan() < TrueMax)
            {
                TrueMax = both + this.MaxBankLoan();
            }

            return TrueMax;
        }

        /// <summary>
        /// Instance Method <c>BSD</c> returns user's payable buyer stamp duty.
        /// </summary>
        /// <remarks>
        /// Calculated based on user's maximum house price.
        /// </remarks>
        /// <returns>
        /// The user's payable BSD.
        /// </returns>
        public double BSD()
        {
            return CalculateBSD(this.CalculateTrueMax());
        }

        /// <summary>
        /// Instance Method <c>ABSD</c> returns user's payable additional buyer stamp duty.
        /// </summary>
        /// <remarks>
        /// Calculated based on user's maximum house price.
        /// </remarks>
        /// <returns>
        /// The user's payable ABSD.
        /// </returns>
        public double ABSD()
        {
            return CalculateABSD(this.CalculateTrueMax());
        }

        /// <summary>
        /// Instance Method <c>CashDownpayment</c> returns user's payable cash downpayment.
        /// </summary>
        /// <remarks>
        /// Calculated based on user's maximum house price.
        /// </remarks>
        /// <returns>
        /// The user's payable cash downpayment.
        /// </returns>
        public double CashDownpayment()
        {
            return this.CalculateTrueMax() * this.CalculateCashDownpaymentPercentage();
        }

        /// <summary>
        /// Instance Method <c>CashCpfDownpayment</c> returns user's payable cash and cpf downpayment.
        /// </summary>
        /// <remarks>
        /// Calculated based on user's maximum house price.
        /// </remarks>
        /// <returns>
        /// The user's payable deposit.
        /// </returns>
        public double CashCpfDownpayment()
        {
            return this.CalculateTrueMax() * (1 - this.CalculateLTV());
        }
    }
}
