"use strict";
var CentileData;
(function (CentileData) {
    var LmsLookup = (function () {
        function LmsLookup(values) {
            var min = -1;
            values.some(function (_e, i) {
                min = i;
                return true;
            });
            if (min === -1) {
                throw new Error("LmsLookup cannot be instantiated with an empty array");
            }
            this.Min = min;
            this.Get = values;
        }
        Object.defineProperty(LmsLookup.prototype, "Max", {
            get: function () { return this.Get.length; },
            enumerable: true,
            configurable: true
        });
        ;
        return LmsLookup;
    }());
    CentileData.LmsLookup = LmsLookup;
    ;
    var GenderRange = (function () {
        function GenderRange(male, female) {
            this.Male = male;
            this.Female = female;
        }
        GenderRange.prototype.GetLms = function (lookupAge, isMale) {
            return isMale
                ? this.Male.Get[lookupAge]
                : this.Female.Get[lookupAge];
        };
        return GenderRange;
    }());
    CentileData.GenderRange = GenderRange;
    ;
    var Constants = (function () {
        function Constants() {
        }
        Constants.daysPerYear = 365.25;
        Constants.daysPerMonth = Constants.daysPerYear / 12;
        Constants.daysPerWeek = 7;
        Constants.weeksPerMonth = Constants.daysPerMonth / Constants.daysPerWeek;
        Constants.termGestation = 40;
        Constants.ceaseCorrectingDaysOfAge = Constants.daysPerYear * 2;
        Constants.roundingFactor = 0.00001;
        Constants.maximumWeeksGestation = 43;
        return Constants;
    }());
    CentileData.Constants = Constants;
    var CentileDataCollection = (function () {
        function CentileDataCollection(gestAge, ageWeeks, ageMonths) {
            this.GestAge = gestAge;
            this.AgeWeeks = ageWeeks;
            this.AgeMonths = ageMonths;
        }
        ;
        CentileDataCollection.prototype.zForAge = function (value, daysOfAge, isMale, totalWeeksGestAtBirth) {
            return this.LmsForAge(daysOfAge, isMale, totalWeeksGestAtBirth).ZFromParam(value);
        };
        ;
        CentileDataCollection.prototype.cumSnormForAge = function (value, daysOfAge, isMale, totalWeeksGestAtBirth) {
            return this.LmsForAge(daysOfAge, isMale, totalWeeksGestAtBirth).CumSnormfromParam(value);
        };
        ;
        CentileDataCollection.prototype.LmsForAge = function (daysOfAge, isMale, totalWeeksGestAtBirth) {
            if (totalWeeksGestAtBirth === void 0) { totalWeeksGestAtBirth = Constants.termGestation; }
            var lookupTotalAge, lookupAge, maxVal, nextLookupAge, ageMonthsLookup, fraction;
            if (isMale && (totalWeeksGestAtBirth < this.GestAge.Male.Min) ||
                (!isMale && totalWeeksGestAtBirth < this.GestAge.Female.Min)) {
                throw new RangeError("totalWeeksGestAtBirth must be greater than GestAgeRange - check property prior to calling");
            }
            if (totalWeeksGestAtBirth > Constants.maximumWeeksGestation) {
                totalWeeksGestAtBirth = Constants.maximumWeeksGestation;
            }
            if (daysOfAge < 0) {
                throw new RangeError("daysOfAge must be >= 0");
            }
            if (daysOfAge > Constants.ceaseCorrectingDaysOfAge) {
                totalWeeksGestAtBirth = Constants.termGestation;
            }
            lookupTotalAge = daysOfAge / 7 + totalWeeksGestAtBirth;
            lookupAge = Math.floor(lookupTotalAge + Constants.roundingFactor);
            maxVal = isMale ? this.GestAge.Male.Max : this.GestAge.Female.Max;
            if (lookupAge == maxVal) {
                nextLookupAge = lookupAge + 1;
                return this.GestAge.GetLms(lookupAge, isMale).LinearInterpolate(this.AgeWeeks.GetLms(nextLookupAge - Constants.termGestation, isMale), lookupTotalAge - lookupAge);
            }
            if (lookupAge < maxVal) {
                nextLookupAge = lookupAge + 1;
                return this.GestAge.GetLms(lookupAge, isMale).LinearInterpolate(this.GestAge.GetLms(nextLookupAge, isMale), lookupTotalAge - lookupAge);
            }
            lookupTotalAge -= Constants.termGestation;
            lookupAge = Math.floor(lookupTotalAge + Constants.roundingFactor);
            maxVal = isMale ? this.AgeWeeks.Male.Max : this.AgeWeeks.Female.Max;
            if (lookupAge == maxVal) {
                ageMonthsLookup = Math.ceil((daysOfAge + totalWeeksGestAtBirth - Constants.termGestation) / Constants.daysPerMonth);
                fraction = (lookupTotalAge - maxVal) / (ageMonthsLookup * Constants.weeksPerMonth - maxVal);
                return this.AgeWeeks.GetLms(lookupAge, isMale).LinearInterpolate(this.AgeMonths.GetLms(ageMonthsLookup, isMale), fraction);
            }
            if (lookupAge < maxVal) {
                nextLookupAge = lookupAge + 1;
                return this.AgeWeeks.GetLms(lookupAge, isMale).LinearInterpolate(this.AgeWeeks.GetLms(nextLookupAge, isMale), lookupTotalAge - lookupAge);
            }
            lookupTotalAge = (daysOfAge + totalWeeksGestAtBirth - Constants.termGestation) / Constants.daysPerMonth;
            lookupAge = Math.floor(lookupTotalAge + Constants.roundingFactor);
            maxVal = (isMale ? this.AgeMonths.Male.Max : this.AgeMonths.Female.Max);
            if (lookupAge > maxVal) {
                return this.AgeMonths.GetLms(maxVal, isMale);
            }
            nextLookupAge = lookupAge + 1;
            return this.AgeMonths.GetLms(lookupAge, isMale).LinearInterpolate(this.AgeMonths.GetLms(nextLookupAge, isMale), lookupTotalAge - lookupAge);
        };
        ;
        CentileDataCollection.prototype.AgeDaysForMedian = function (median, isMale) {
            var lookup = isMale
                ? this.AgeMonths.Male
                : this.AgeMonths.Female;
            var multiplier = Constants.daysPerMonth;
            if (lookup.Get[lookup.Min].M > median) {
                lookup = isMale
                    ? this.AgeWeeks.Male
                    : this.AgeWeeks.Female;
                multiplier = Constants.daysPerWeek;
                if (lookup.Get[lookup.Min].M > median) {
                    return NaN;
                }
            }
            var i = lookup.Max;
            while (i >= lookup.Min && lookup.Get[i].M > median) {
                --i;
            }
            if (i < lookup.Max) {
                i += (median - lookup.Get[i].M) / (lookup.Get[i + 1].M - lookup.Get[i].M);
            }
            return Math.round(i * multiplier);
        };
        return CentileDataCollection;
    }());
    CentileData.CentileDataCollection = CentileDataCollection;
})(CentileData || (CentileData = {}));
//# sourceMappingURL=CentileDataCollection.js.map