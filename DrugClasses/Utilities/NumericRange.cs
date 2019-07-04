using System;
using System.Collections.Generic;
using System.Text;

namespace DrugClasses.Utilities
{
    public class NumericRange<T> where T : IComparable<T>
    {
        public NumericRange() { }
        /// <summary>
        /// will sort 2 values & place appropriately into lower & upper bound
        /// </summary>
        /// <param name="val1"></param>
        /// <param name="val2"></param>
        public NumericRange(T val1, T val2)
        {
            if (val1.CompareTo(val2) == 1) //val 1 > val2
            {
                LowerBound = val2;
                UpperBound = val1;
            }
            else
            {
                LowerBound = val1;
                UpperBound = val2;
            }
        }
        /// <summary>
        /// both upper & lower bound will have the same value
        /// </summary>
        /// <param name="val"></param>
        public NumericRange(T val)
        {
            UpperBound = LowerBound = val;
        }
        protected bool _lowerIsSet;
        protected T _lowerBound;
        /// <summary>
        /// mult be less than upperbound (if set)
        /// </summary>
        public T LowerBound
        {
            get { return _lowerBound; }
            set
            {
                if (_upperIsSet && value.CompareTo(_upperBound) == 1) { throw new ArgumentOutOfRangeException("lowerBound must be less than upperBound"); }
                _lowerBound = value;
                _lowerIsSet = true;
            }
        }
        protected bool _upperIsSet;
        protected T _upperBound;
        /// <summary>
        /// must be greater than lowerbound (if set)
        /// </summary>
        public T UpperBound
        {
            get { return _upperBound; }
            set
            {
                if (_lowerIsSet && value.CompareTo(_lowerBound) == -1)
                {
                    throw new ArgumentOutOfRangeException("upperBound must be greater than lowerBound");
                }
                _upperBound = value;
                _upperIsSet = true;
            }
        }
        public string Separator { get; set; } = "–";//en-dash
        public override string ToString()//this overide exists because default value confuses string.format Method
        {
            if (_upperIsSet)
            {
                if (!_lowerIsSet || (_lowerIsSet && _lowerBound.CompareTo(_upperBound) == 0))
                {
                    return _upperBound.ToString();
                }
                return _lowerBound.ToString() + Separator + _upperBound.ToString();
            }
            else if (_lowerIsSet)
            {
                return _lowerBound.ToString();
            }
            return string.Empty;
        }
    }
}
