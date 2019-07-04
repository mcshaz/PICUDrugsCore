using DBToJSON.RepositoryClasses.Enums;
using DrugClasses.RepositoryClasses;
using System;
using System.Collections.Generic;
using System.Text;

namespace DrugClasses.PresentationClasses.Dosing
{
    /// <summary>
    /// SI units inlude milligrams, joules and (international) units
    /// </summary>
    public class SiUnitMeasure
    {
        public SiUnitMeasure(int logSi, Unit unit) 
        {
            if (logSi > 0 || logSi % 3 != 0)
            {
                throw new ArgumentOutOfRangeException("logSi", "must be between -12 and 0, in intervals of 3");
            }
            LogSi = logSi;
            Unit = unit;
        }
        public SiUnitMeasure(int logSi, string unit) : this(logSi, ParseUnit(unit)) { }
        public int LogSi { get; }
        public Unit Unit { get; }
        public bool Pleuralise { get; set; } = true;
        protected bool IsUserSafePrefix { get => LogSi == -3; }
        protected static Unit ParseUnit(string unit)
        {
            if (Enum.TryParse(unit, false, out Unit enumUnit))
            { //case sensitive (ignorecase:=false) for now
                return enumUnit;
            }
            throw new ArgumentOutOfRangeException("unit must be exactly one of " +
                string.Join("|", Enum.GetNames(typeof(Unit))));
        }
        public string UnitString(bool abbreviate = false)
        {
            if (abbreviate)
            {
                return SiBaseUnit.GetAbbreviation(Unit);
            }
            return Unit.ToString() + (Pleuralise ? "s" : string.Empty);
        }
        public static string LogSiToString(int logVal) => SiPrefixes.GetPrefix(logVal).FullName;

        public static char? LogSiToChar(int logVal) => SiPrefixes.GetPrefix(logVal).Symbol;

        public override string ToString() => LogSiToString(LogSi) + UnitString();

        public virtual string ToShortString() => LogSiToChar(LogSi) + UnitString(true);
        /// <summary>
        /// only milli (log SI unit -3) will use the abbreviation m, all others will be in full
        /// </summary>
        /// <returns></returns>
        public virtual string ToShortUserSafeString() => IsUserSafePrefix
            ? ToShortString()
            : ToString();
    }
}
