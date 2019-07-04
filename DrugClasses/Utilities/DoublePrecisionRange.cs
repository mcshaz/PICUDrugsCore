using System;
using System.Collections.Generic;
using System.Text;

namespace DrugClasses.Utilities
{
    public enum RoundingMethod { FixedDecimalPlaces, ToPrecision }
    public class DoublePrecisionRange : NumericRange<Double>
    {
        public DoublePrecisionRange() : base() { }
        public DoublePrecisionRange(double val) : base(val) { }
        public DoublePrecisionRange(double val1, double val2) : base(val1, val2) { }
        public RoundingMethod Rounding { get; set; } = RoundingMethod.ToPrecision;
        private int _precision = 2;
        /// <summary>
        /// if ToPrecision, the number of significant figures, and if FixedDecimalPlaaces, the number of decimal places
        /// </summary>
        public int Precision {
            get { return _precision; }
            set {
                if (value <= 0 || value > 8)
                {
                    throw new ArgumentOutOfRangeException("Precision must be between 1 - 8");
                }
                _precision = value;
            }
        }

        public override string ToString() //  + u200A hairwidth space ' '
        {
            if (!_lowerIsSet) return string.Empty;
            string lb;
            string ub;
            if (Rounding == RoundingMethod.ToPrecision)
            {

                lb = _lowerBound.ToPrecision(Precision).ToString(); //.(fixedString);
                ub = (_lowerBound == _upperBound) ? lb : _upperBound.ToPrecision(Precision).ToString();
                //return _lowerBound.Value.ToString(fixedString) + seperator + _upperBound.Value.ToString(fixedString);
            }
            else //fixed decimal places
            {
                string fixedString = "0." + new string('0', Precision);
                lb = _lowerBound.ToString(fixedString);
                ub = (_lowerBound == _upperBound) ? lb : _upperBound.ToString(fixedString);
            }
            if (lb == ub) { return lb; }
            return lb + Separator + ub;
            // return string.Format("{0:0.####} - {1:0.####}", _minRate, _maxRate);
        }

        public string AsDrawingUpVolume()
        {
            if (!_lowerIsSet) return string.Empty;
            double lb = _lowerBound.AsDrawingUpVolume();
            double ub = (!_upperIsSet || _lowerBound == _upperBound) 
                ?lb
                : _upperBound.AsDrawingUpVolume();
            return (lb==ub)
                ?lb.ToString()
                :(lb + Separator + ub);
        }

        public static DoublePrecisionRange operator *(DoublePrecisionRange rng, double multiplier)
        {
            return new DoublePrecisionRange(
                rng.LowerBound * multiplier,
                rng.UpperBound * multiplier);
        }
        public static DoublePrecisionRange operator /(DoublePrecisionRange rng, double divisor)
        {
            return new DoublePrecisionRange(
                rng.LowerBound / divisor,
                rng.UpperBound / divisor);
        }
    }
    public class RangeFactory
    {
        public int Precision { get; set; } = 2;
        public RoundingMethod Rounding { get; set; } = RoundingMethod.ToPrecision;
        public DoublePrecisionRange Create() {
            return new DoublePrecisionRange()
            {
                Precision = Precision,
                Rounding = Rounding
            };
        }
        public DoublePrecisionRange Create(double val)
        {
            return new DoublePrecisionRange(val)
            {
                Precision = Precision,
                Rounding = Rounding
            };
        }
        public DoublePrecisionRange Create(double val1, double val2)
        {
            return new DoublePrecisionRange(val1, val2)
            {
                Precision = Precision,
                Rounding = Rounding
            };
        }
    }
}
