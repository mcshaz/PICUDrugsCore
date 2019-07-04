using System;
using System.Collections.Generic;

namespace DrugClasses.PresentationClasses.Dosing
{
    public class SiPrefix
    {
        public SiPrefix(int logValue, string fullName, char? symbol)
        {
            LogValue = logValue;
            FullName = fullName;
            Symbol = symbol;
        }
        public int LogValue { get; }
        public string FullName { get; }
        public char? Symbol { get; }
    }
    public static class SiPrefixes
    {
        static SiPrefixes()
        {
            Prefixes = Array.AsReadOnly(new[]
            {
                new SiPrefix(0,string.Empty,null),
                new SiPrefix(-3,"milli",'m'),
                new SiPrefix(-6,"micro",'µ'),
                new SiPrefix(-9,"nano",'n'),
                new SiPrefix(-12, "pico",'p')
            });
        }

        public static IReadOnlyList<SiPrefix> Prefixes { get; }
        public static SiPrefix GetPrefix(int logVal) => Prefixes[logVal/-3];
    }
}
