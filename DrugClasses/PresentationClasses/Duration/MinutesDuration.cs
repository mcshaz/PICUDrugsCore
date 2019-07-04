namespace DrugClasses.PresentationClasses.Duration
{
    public class MinutesDuration
    {
        public MinutesDuration() : this(0) { }
        public MinutesDuration(int totalMins)
        {
            TotalMins = totalMins;
            Hours = totalMins/60;
            Mins = totalMins - (Hours * 60);
        }
        public int TotalMins { get; }
        public int Hours { get;  }
        public int Mins { get;  }
        public override string ToString()
        {
            return Hours.ToString() + ':' + (Mins > 9 ? Mins.ToString() : ('0' + Mins.ToString()));
        }
        public string ToLongString()
        {
            if (Hours == 0)
            {
                return Mins.ToString() + " mins";
            }
            if (Mins == 0)
            {
                return Hours.ToString() + " hours";
            }
            return $"{Hours} hours {Mins} mins";
        }

        public static MinutesDuration operator -(MinutesDuration a, MinutesDuration b)
            => new MinutesDuration(a.TotalMins - b.TotalMins);
        public static MinutesDuration operator -(MinutesDuration a, int b)
            => new MinutesDuration(a.TotalMins - b);
        public static MinutesDuration operator -(int a, MinutesDuration b)
            => new MinutesDuration(a - b.TotalMins);
        public static MinutesDuration operator +(MinutesDuration a, MinutesDuration b)
            => new MinutesDuration(a.TotalMins +b.TotalMins);
        public static MinutesDuration operator +(MinutesDuration a, int b)
            => new MinutesDuration(a.TotalMins + b);
        public static MinutesDuration operator +(int a, MinutesDuration b)
            => new MinutesDuration(a + b.TotalMins);
        public static MinutesDuration operator *(int a, MinutesDuration b)
            => new MinutesDuration(a * b.TotalMins);
        public static MinutesDuration operator *(MinutesDuration a, int b)
            => new MinutesDuration(a.TotalMins * b);
        public static MinutesDuration operator *(double a, MinutesDuration b)
            => new MinutesDuration((int)(a * b.TotalMins));
        public static MinutesDuration operator *(MinutesDuration a, double b)
            => new MinutesDuration((int)(a.TotalMins * b));
    }
}
