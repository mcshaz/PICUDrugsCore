var CentileData;
(function (CentileData) {
    var daysPerYear = 365.25;
    var msPerDay = 24 * 60 * 60 * 1000;
    var daysPerMonth = daysPerYear / 12;
    var weeksPerMonth = daysPerMonth / 7;
    var termGestation = 40;
    var ceaseCorrectingDaysOfAge = daysPerMonth * 24;
    var roundingFactor = 0.00001;
    var maximumGestationalCorrection = 43;
    var LmsLookup = /** @class */ (function () {
        function LmsLookup(values) {
            this.Min = values.indexOf(Math.min.apply(Math, values));
            this.Max = values.length - 1 + this.Min;
            this.Get = values;
        }
        return LmsLookup;
    }());
    CentileData.LmsLookup = LmsLookup;
    ;
    var GenderRange = /** @class */ (function () {
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
    var CentileDataCollection = /** @class */ (function () {
        function CentileDataCollection(gestAge, ageWeeks, ageMonths) {
            this.cumSnormForAge = function (value, daysOfAge, isMale, totalWeeksGestAtBirth) {
                return this.LmsForAge(daysOfAge, isMale, totalWeeksGestAtBirth).cumSnormfromParam(value);
            };
            this.GestAge = gestAge;
            this.AgeWeeks = ageWeeks;
            this.AgeMonths = ageMonths;
        }
        ;
        CentileDataCollection.prototype.zForAge = function (value, daysOfAge, isMale, totalWeeksGestAtBirth) {
            return this.LmsForAge(daysOfAge, isMale, totalWeeksGestAtBirth).ZFromParam(value);
        };
        ;
        CentileDataCollection.prototype.LmsForAge = function (daysOfAge, isMale, totalWeeksGestAtBirth) {
            var lookupTotalAge, lookupAge, maxVal, nextLookupAge, ageMonthsLookup, fraction;
            if (isMale && (totalWeeksGestAtBirth < this.GestAge.Male.Min) ||
                (!isMale && totalWeeksGestAtBirth < this.GestAge.Female.Min)) {
                throw new RangeError("totalWeeksGestAtBirth must be greater than GestAgeRange - check property prior to calling");
            }
            totalWeeksGestAtBirth = totalWeeksGestAtBirth || termGestation;
            if (totalWeeksGestAtBirth > maximumGestationalCorrection) {
                totalWeeksGestAtBirth = maximumGestationalCorrection;
            }
            if (daysOfAge < 0) {
                throw new RangeError("daysOfAge must be >= 0");
            }
            if (daysOfAge > ceaseCorrectingDaysOfAge) {
                totalWeeksGestAtBirth = termGestation;
            }
            lookupTotalAge = daysOfAge / 7 + totalWeeksGestAtBirth;
            lookupAge = Math.floor(lookupTotalAge + roundingFactor);
            maxVal = isMale ? this.GestAge.Male.Max : this.GestAge.Female.Max;
            if (lookupAge == maxVal) {
                nextLookupAge = lookupAge + 1;
                return this.GestAge.GetLms(lookupAge, isMale).LinearInterpolate(this.AgeWeeks.GetLms(nextLookupAge - termGestation, isMale), lookupTotalAge - lookupAge);
            }
            if (lookupAge < maxVal) {
                nextLookupAge = lookupAge + 1;
                return this.GestAge.GetLms(lookupAge, isMale).LinearInterpolate(this.GestAge.GetLms(nextLookupAge, isMale), lookupTotalAge - lookupAge);
            }
            lookupTotalAge -= termGestation;
            lookupAge = Math.floor(lookupTotalAge + roundingFactor);
            maxVal = isMale ? this.AgeWeeks.Male.Max : this.AgeWeeks.Female.Max;
            if (lookupAge == maxVal) {
                ageMonthsLookup = Math.ceil((daysOfAge + totalWeeksGestAtBirth - termGestation) / daysPerMonth);
                fraction = (lookupTotalAge - maxVal) / (ageMonthsLookup * weeksPerMonth - maxVal);
                return this.AgeWeeks.GetLms(lookupAge, isMale).LinearInterpolate(this.AgeMonths.GetLms(ageMonthsLookup, isMale), fraction);
            }
            if (lookupAge < maxVal) {
                nextLookupAge = lookupAge + 1;
                return this.AgeWeeks.GetLms(lookupAge, isMale).LinearInterpolate(this.AgeWeeks.GetLms(nextLookupAge, isMale), lookupTotalAge - lookupAge);
            }
            lookupTotalAge = (daysOfAge + totalWeeksGestAtBirth - termGestation) / daysPerMonth;
            lookupAge = Math.floor(lookupTotalAge + roundingFactor);
            maxVal = (isMale ? this.AgeMonths.Male.Max : this.AgeMonths.Female.Max);
            if (lookupAge > maxVal) {
                return this.AgeMonths.GetLms(maxVal, isMale);
            }
            nextLookupAge = lookupAge + 1;
            return this.AgeMonths.GetLms(lookupAge, isMale).LinearInterpolate(this.AgeMonths.GetLms(nextLookupAge, isMale), lookupTotalAge - lookupAge);
        };
        ;
        return CentileDataCollection;
    }());
    CentileData.CentileDataCollection = CentileDataCollection;
})(CentileData || (CentileData = {}));
//# sourceMappingURL=CentileData.js.map