using System;

namespace DrugClasses.Utilities
{
    static class Precision
    {
        //http://stackoverflow.com/questions/158172/formatting-numbers-with-significant-figures-in-c-sharp#answer-1987721
        private static double ToPrecision(double d, int sigFigures, out int roundingPosition)
        {
            // this Method will return a rounded double value at a number of signifigant figures.
            // the sigFigures parameter must be between 0 and 15, exclusive.

            roundingPosition = 0;

            // ToDo: Might want to compare with epsilon here
            if (d == 0.0)
            {
                return d;
            }

            if (double.IsNaN(d))
            {
                return double.NaN;
            }

            if (double.IsPositiveInfinity(d))
            {
                return double.PositiveInfinity;
            }

            if (double.IsNegativeInfinity(d))
            {
                return double.NegativeInfinity;
            }

            if (sigFigures < 1 || sigFigures > 14)
            {
                throw new ArgumentOutOfRangeException("sigFigures", d, "The sigFigures argument must be between 0 and 15 exclusive.");
            }

            // The resulting rounding position will be negative for rounding at whole numbers, and positive for decimal places.
            roundingPosition = sigFigures - 1 - (int)(Math.Floor(Math.Log10(Math.Abs(d))));

            // try to use a rounding position directly, if no scale is needed.
            // this is because the scale mutliplication after the rounding can introduce error, although 
            // this only happens when you're dealing with really tiny numbers, i.e 9.9e-14.
            if (roundingPosition > 0 && roundingPosition < 15)
            {
                return Math.Round(d, roundingPosition, MidpointRounding.AwayFromZero);
            }

            // Shouldn't get here unless we need to scale it.
            // Set the scaling value, for rounding whole numbers or decimals past 15 places
            double scale = Math.Pow(10, Math.Ceiling(Math.Log10(Math.Abs(d))));

            return Math.Round(d / scale, sigFigures, MidpointRounding.AwayFromZero) * scale;
        }

        public static double ToPrecision(this double d, int sigFigures)
        {
            return ToPrecision(d, sigFigures, out int unneededRoundingPosition);
        }
        /// <summary>
        /// As per ToPrecison, but will round the next digit after The precision to 0.5 if it lies >0.25 & <0.75
        /// </summary>
        /// <param name="value"></param>
        /// <param name="sigFigures"></param>
        /// <returns></returns>
        public static double ToPrecisionAndHalf(this double d, int sigFigures)
        {
            var newVal = ToPrecision(d, sigFigures, out int roundingPosition);
            var accuracy = (d - newVal) * Math.Pow(10, roundingPosition);
            if (accuracy > 0.25) { newVal += 0.5 * Math.Pow(10, -roundingPosition); }
            else if (accuracy < -0.25) { newVal -= 0.5 * Math.Pow(10, -roundingPosition); }
            return newVal;
        }
        public static double AsDrawingUpVolume(this double d)
        {
            if (d <= 0.1) { return ToPrecision(d, 2); }
            if (d <= 1) { return Math.Round(d, 2); }
            // if (d <= 3) 
            return Math.Round(d, 1);
            //if (d <= 10) { return Math.Round(d * 5) / 5; } //nearest 0.2
            //return Math.Round(d);
        }
    }
}
