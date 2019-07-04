using DBToJSON.RepositoryClasses.Enums;
using DrugClasses.RepositoryClasses;
using System;

namespace DrugClasses.PresentationClasses.Dosing
{
    /// <summary>
    /// Drug doses either in absolute SI units e.g. mg or mg/Kg
    /// </summary>
    public class DrugDoseUnit : SiUnitMeasure
    {
        public DrugDoseUnit(int logSi, Unit unit, bool isPerKg) : base(logSi, unit)
        {
            IsPerKg = isPerKg;
        }
        public DrugDoseUnit(int logSi, string unit, bool isPerKg) : this(logSi, ParseUnit(unit), isPerKg)
        {
        }
        internal const string _defaultPer = "/";
        public bool IsPerKg { get; }
        public string PerSeperator { get; set; } = _defaultPer;

        private string PerString() => (IsPerKg ? (PerSeperator + "kg") : string.Empty); 

        public new string ToString() => base.ToString() + PerString();

        public new string ToShortString() => base.ToShortString() + PerString();
        /// <summary>
        /// only milli (log SI unit -3) will use the abbreviation m, all others will be in full
        /// </summary>
        /// <returns></returns>
        public new string ToShortUserSafeString() => IsUserSafePrefix
            ?ToShortString()
            :ToString();
    }
}
