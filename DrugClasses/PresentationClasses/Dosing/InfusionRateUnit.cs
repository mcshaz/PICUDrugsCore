using DBToJSON.RepositoryClasses.Enums;

namespace DrugClasses.PresentationClasses.Dosing
{
    /// <summary>
    /// Rate in SI units e.g. units/min or micrograms/kg/hr
    /// </summary>
    public class InfusionRateUnit : DrugDoseUnit
    {
        /// <summary>
        /// Rate in si unit e.g. units/min or micrograms/kg/hr
        /// </summary>
        /// <param name="logSi"></param>
        /// <param name="unit"></param>
        /// <param name="isPerKg"></param>
        /// <param name="isPerMin">alternative is per hour</param>
        public InfusionRateUnit(int logSi, Unit unit, bool isPerKg, bool isPerMin) : base(logSi, unit, isPerKg)
        {
            IsPerMin = isPerMin;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logSi"></param>
        /// <param name="unit"></param>
        /// <param name="isPerKg"></param>
        /// <param name="isPerMin">alternative is per hour</param>
        public InfusionRateUnit(int logSi, string unit, bool isPerKg, bool isPerMin) : base(logSi, unit, isPerKg)
        {
            IsPerMin = isPerMin;
        }
        /// <summary>
        /// Alternative is per hour
        /// </summary>
        public bool IsPerMin { get; }

        private string ShortRate() => PerSeperator + (IsPerMin ? "min" : "hr");
        private string LongRate() => PerSeperator + (IsPerMin ? "minute" : "hour");

        public new string ToString() => base.ToString() + LongRate();

        public new string ToShortString() => base.ToShortString() + ShortRate();
        /// <summary>
        /// only milli (log SI unit -3) will use the abbreviation m, all others will be in full
        /// </summary>
        /// <returns></returns>
        public new string ToShortUserSafeString() => IsUserSafePrefix
            ? ToShortString()
            : (base.ToString() + ShortRate());
    }
}
