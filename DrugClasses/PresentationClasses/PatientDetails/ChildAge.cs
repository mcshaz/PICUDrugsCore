using DrugClasses.Utilities;
using System;

namespace DrugClasses.PresentationClasses.Patient
{
    public class ChildAge
    {
        public ChildAge(int years, int? months, int? days)
        {
            if (years < 0 || months < 0 || days < 0)
            {
                throw new Exception("years, months and days must all be positive");
            }
            Days = days;
            Months = months;
            Years = years;
            if (Days.HasValue && Days > 28)
            {
                DateTime countBack = DateTime.Today.AddMonths(-1);
                int DaysInMonth = DateTime.DaysInMonth(countBack.Year, countBack.Month);
                while (Days.Value >= DaysInMonth)
                {
                    Days -= DaysInMonth;
                    Months++;
                    countBack = countBack.AddMonths(-1);
                    DaysInMonth = DateTime.DaysInMonth(countBack.Year, countBack.Month);
                }
            }
            if (Months > 12)
            {
                Years = Years + (int)(Months / 12);
                Months = Months % 12;
            }
        }
        protected ChildAge(Tuple<int,int,int> YrMthDays)
        {
            Years = YrMthDays.Item1;
            Months = YrMthDays.Item2;
            Days = YrMthDays.Item3;
        }
        public ChildAge(string years, string months, string days) : this(int.Parse(years), ToNullableInt(months), ToNullableInt(days))
        {
            if (string.IsNullOrWhiteSpace(years) && string.IsNullOrWhiteSpace(months) && string.IsNullOrWhiteSpace(days))
            {
                throw new Exception("years, months and days cannot be all null, empty or whitepace");
            }
        }
        static int? ToNullableInt(string val)
        {
            if (int.TryParse(val, out int result))
            {
                return result;
            }
            return null;
        }
        public override string ToString()
        {
            string formatstr;
            if (!Months.HasValue) { formatstr = "{0} years"; }
            else if (!Days.HasValue) { formatstr = "{0} years {1} months"; }
            else { formatstr = "{0} years {1} months {2} days"; }
            return string.Format(formatstr, Years, Months, Days);
        }
        public string ToShortString()
        {
            return string.Format(Months.HasValue ? "{0} y {1} m" : "{0} y", Years, Months);
        }

        public int? Days { get; }
        public int? Months { get; }
        public int Years { get; }
        public int TotalMonthsEstimate
        {
            get
            {
                return (12 * Years) + (Months ?? 6);
            }
        }
        public int? TotalMonths { get { return (12 * Years) + Months; } }
        const double DaysPerYear = 365.25;
        const double DaysPerMonth = DaysPerYear / 12;
        const double DaysPerHalfMonth = DaysPerYear / 24;

        public virtual NumericRange<int> GetAgeRangeInDays()
        {
            var returnVar = new NumericRange<int>();
            if (Months.HasValue && Days.HasValue) {
                returnVar.LowerBound = returnVar.UpperBound = GetTotalDays(Years, Months.Value, Days.Value); }
            else if (Months.HasValue)
            {
                returnVar.LowerBound = GetTotalDays(Years, Months.Value, 0);
                DateTime today = DateTime.Today;
                returnVar.UpperBound = GetTotalDays(Years, Months.Value, DateTime.DaysInMonth(today.Year, today.Month));
            }
            else
            {
                returnVar.LowerBound = GetTotalDays(Years, 0, 0);
                returnVar.UpperBound = GetTotalDays(Years, 11, 31);
            }
            return returnVar;
        }
        public virtual int? GetAgeInDays()
        {
            if (!Months.HasValue || !Days.HasValue)
            {
                return null;
            }
            return GetTotalDays(Years, Months.Value, Days.Value);
        }
        static int GetTotalDays(int years, int months, int days)
        {
            return (int)(years * DaysPerYear + months * DaysPerMonth + days);
        }
    }

    public class ChildAgeFromDOB : ChildAge
    {
        public DateTime Dob { get; }
        private ITodayProvider TodayProvider {get;}
        public ChildAgeFromDOB(DateTime dob) : this(dob, new TodayProvider()) { }
        public ChildAgeFromDOB(DateTime dob, ITodayProvider todayProvider) : base(CalculateAge(dob,todayProvider))
        {
            Dob = dob.Date;
            TodayProvider = todayProvider;
        }
        private static Tuple<int, int, int> CalculateAge(DateTime dob, ITodayProvider todayProvider){
            DateTime today = todayProvider.Today;
            dob = dob.Date;
            if (dob > today) throw new ArgumentOutOfRangeException("DOB must not be AFTER current system date");
            var years = today.Year - dob.Year;
            var months = today.Month - dob.Month;
            var days = today.Day - dob.Day;
            if (months < 0)
            {
                months += 12;
            }
            if (days < 0)
            {
                days += DateTime.DaysInMonth(today.Year, today.AddMonths(-1).Month);
                if (months == 0)
                {
                    months = 11;
                    years--;
                }
                else { months--; }
            }
            if (dob > today.AddYears(-years))
            {
                years--;
            }
            return Tuple.Create(years, months, days);
        }
        public override NumericRange<int> GetAgeRangeInDays()
        {
            return new NumericRange<int>((TodayProvider.Today - Dob).Days);
        }
        public override int? GetAgeInDays()
        {
            return (TodayProvider.Today - Dob).Days;
        }
    }

    public interface ITodayProvider
    {
        DateTime Today { get; }
    }
    internal class TodayProvider : ITodayProvider
    {
        public DateTime Today { get => DateTime.Today; }
    }
}
