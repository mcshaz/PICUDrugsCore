using System;
// using System.Runtime.CompilerServices;


namespace DBToJSON.SqlEntities
{
    internal static class DateUtilities
    {
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static DateTime AsUtc(this DateTime value)
        {
            switch (value.Kind)
            {
                case DateTimeKind.Utc:
                    return value;
                case DateTimeKind.Unspecified:
                    return DateTime.SpecifyKind(value, DateTimeKind.Utc);
                default: // DateTimeKind.Local
                    throw new ArgumentException("the DateTime must not be a local time");
            }
        }
    }
}
