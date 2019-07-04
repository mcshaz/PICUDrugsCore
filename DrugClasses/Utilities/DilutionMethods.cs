using DBToJSON.RepositoryClasses.Enums;

namespace DrugClasses.RepositoryClasses
{
    public class MethodLogic
    {
        public MethodLogic(bool isNeat, bool isVaryConcentration, bool isVaryVolume, bool isPerKg)
        {
            IsNeat = isNeat;
            IsVaryConcentration = isVaryConcentration;
            IsVaryVolume = isVaryVolume;
            IsPerKg = isPerKg;
        }
        public bool IsNeat { get; }
        public bool IsVaryConcentration { get; }
        public bool IsVaryVolume { get; }
        public bool IsPerKg { get; }
    }
    internal static class DilutionLogic
    {
        static DilutionLogic()
        {
            _methodLogics = new []
            {
                /*[DilutionMethod.NeatFixedFlow]=*/                  new MethodLogic(true,  false, false, false),
                /*[DilutionMethod.NeatVaryFlowByWeight]=*/           new MethodLogic(true,  false, false, true),
                /*[DilutionMethod.FixedDilutionFixedFlow]=*/         new MethodLogic(false, false, false, false),
                /*[DilutionMethod.FixedDilutionVaryFlowByWeight]=*/  new MethodLogic(false, false, false, true),
                /*[DilutionMethod.VaryDilutionVolumeFixedFlow]=*/    new MethodLogic(false, true,  true,  true),
                /*[DilutionMethod.VaryDrugFixedFlow]=*/              new MethodLogic(false, true,  false, true),
                /*[DilutionMethod.VaryDrugDilutionVolFlowByWeight]=*/new MethodLogic(false, false, true,  true)
            };
        }
        public static MethodLogic GetMethod(DilutionMethod method) => _methodLogics[(int)method - 1];
        private static readonly MethodLogic[] _methodLogics;
    }
}
