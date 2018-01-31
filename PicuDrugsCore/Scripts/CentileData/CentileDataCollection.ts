import { Lms } from './Lms'

export class LmsLookup {
    readonly Min: number;
    readonly Get: ReadonlyArray<Lms>;
    get Max(){ return this.Get.length };
    constructor(values: Lms[]) {
        let min: number = -1;
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
};

export class GenderRange {
    readonly Male: LmsLookup;
    readonly Female: LmsLookup;
    constructor(male:LmsLookup, female: LmsLookup) {
        this.Male = male;
        this.Female = female; 
    }
    GetLms(lookupAge: number, isMale: boolean) {
        return isMale
            ? this.Male.Get[lookupAge]
            : this.Female.Get[lookupAge];
    }
};

export class Constants {
    public static readonly daysPerYear = 365.25;
    public static readonly daysPerMonth = Constants.daysPerYear / 12;
    public static readonly daysPerWeek = 7;
    public static readonly weeksPerMonth = Constants.daysPerMonth / Constants.daysPerWeek;
    public static readonly termGestation = 40;
    public static readonly ceaseCorrectingDaysOfAge = Constants.daysPerYear * 2;
    public static readonly roundingFactor = 0.00001;
    public static readonly maximumWeeksGestation = 43;
}

export abstract class CentileDataCollection {
    protected readonly GestAge: GenderRange;
    protected readonly AgeWeeks: GenderRange;
    protected readonly AgeMonths: GenderRange;

    constructor(gestAge: GenderRange, ageWeeks: GenderRange, ageMonths:GenderRange) {
        this.GestAge = gestAge;
        this.AgeWeeks = ageWeeks;
        this.AgeMonths = ageMonths; 
    };

    zForAge(value: number, daysOfAge: number, isMale: boolean, totalWeeksGestAtBirth:number) {
        return this.LmsForAge(daysOfAge, isMale, totalWeeksGestAtBirth).ZFromParam(value);
    };

    cumSnormForAge(value:number, daysOfAge:number, isMale:boolean, totalWeeksGestAtBirth:number) {
        return this.LmsForAge(daysOfAge, isMale, totalWeeksGestAtBirth).CumSnormfromParam(value);
    };

    LmsForAge(daysOfAge: number, isMale:boolean, totalWeeksGestAtBirth:number = Constants.termGestation) {
        let lookupTotalAge: number, lookupAge: number, maxVal: number, nextLookupAge: number, ageMonthsLookup: number, fraction: number;
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
            return this.GestAge.GetLms(lookupAge, isMale).LinearInterpolate(
                this.GestAge.GetLms(nextLookupAge, isMale),
                lookupTotalAge - lookupAge);
        }
        lookupTotalAge -= Constants.termGestation;
        lookupAge = Math.floor(lookupTotalAge + Constants.roundingFactor);
        maxVal = isMale ? this.AgeWeeks.Male.Max : this.AgeWeeks.Female.Max;
        if (lookupAge == maxVal) {
            ageMonthsLookup = Math.ceil((daysOfAge + totalWeeksGestAtBirth - Constants.termGestation) / Constants.daysPerMonth);
            fraction = (lookupTotalAge - maxVal) / (ageMonthsLookup * Constants.weeksPerMonth - maxVal);
            return this.AgeWeeks.GetLms(lookupAge, isMale).LinearInterpolate(
                this.AgeMonths.GetLms(ageMonthsLookup, isMale),
                fraction);
        }
        if (lookupAge < maxVal) {
            nextLookupAge = lookupAge + 1;
            return this.AgeWeeks.GetLms(lookupAge, isMale).LinearInterpolate(
                this.AgeWeeks.GetLms(nextLookupAge, isMale),
                lookupTotalAge - lookupAge);
        }
        lookupTotalAge = (daysOfAge + totalWeeksGestAtBirth - Constants.termGestation) / Constants.daysPerMonth;
        lookupAge = Math.floor(lookupTotalAge + Constants.roundingFactor);
        maxVal = (isMale ? this.AgeMonths.Male.Max : this.AgeMonths.Female.Max);
        if (lookupAge > maxVal) {
            return this.AgeMonths.GetLms(maxVal, isMale);
        }
        nextLookupAge = lookupAge + 1;
        return this.AgeMonths.GetLms(lookupAge, isMale).LinearInterpolate(
            this.AgeMonths.GetLms(nextLookupAge, isMale),
            lookupTotalAge - lookupAge);
    };

    AgeDaysForMedian(median: number, isMale: boolean) {
        let lookup = isMale
            ? this.AgeMonths.Male
            : this.AgeMonths.Female;
        let multiplier = Constants.daysPerMonth;
            
        if (lookup.Get[lookup.Min].M > median) {
            lookup = isMale
                ? this.AgeWeeks.Male
                : this.AgeWeeks.Female;
            multiplier = Constants.daysPerWeek;
            if (lookup.Get[lookup.Min].M > median) {
                //for now Nan - could return -ve number for prior to delivery?
                return NaN;
            }
        }
        let i = lookup.Max;
        while (i >= lookup.Min && lookup.Get[i].M > median) {
            --i;
        }
        if (i < lookup.Max) {
            i += (median - lookup.Get[i].M) / (lookup.Get[i + 1].M - lookup.Get[i].M);
        }
        return Math.round(i * multiplier);
    }
}