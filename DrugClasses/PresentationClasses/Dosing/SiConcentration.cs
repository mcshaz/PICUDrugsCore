using DBToJSON.RepositoryClasses.Enums;

namespace DrugClasses.PresentationClasses.Dosing
{
    /// <summary>
    /// any SI  unit of measurement per mL
    /// </summary>
    public class SiConcentration : SiUnitMeasure
    {
        public SiConcentration(SiUnitMeasure measure) : base(measure.LogSi, measure.Unit) { }
        public SiConcentration(int logSi, Unit unit) : base(logSi, unit) { }
        public SiConcentration(int logSi, string unit) : base(logSi, unit) { }
        public string PerSeperator { get; set; } = DrugDoseUnit._defaultPer;
        private string Rate () => PerSeperator + "mL"; 
        public new string ToString() => base.ToString() + Rate();
        public override string ToShortUserSafeString() => IsUserSafePrefix
            ?ToShortString()
            :ToString();
        public override string ToShortString() => base.ToShortString() + Rate();
    }
}
