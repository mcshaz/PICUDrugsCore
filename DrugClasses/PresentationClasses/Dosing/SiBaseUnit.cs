using DBToJSON.RepositoryClasses.Enums;

namespace DrugClasses.PresentationClasses.Dosing
{
    public static class SiBaseUnit
    {
        static SiBaseUnit()
        { 
            _unitAbbreviations = new[]
            {
                "g", "unit", "mol", "J", "L" //note unit should perhaps be I.U., but safe prescribing should be units
            };
        }
        public static string GetAbbreviation(Unit unit) => _unitAbbreviations[(int)unit-1];
        private static readonly string[] _unitAbbreviations;
    }
}
