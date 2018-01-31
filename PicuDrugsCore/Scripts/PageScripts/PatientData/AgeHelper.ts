import * as moment from 'moment'
import { IntegerRange } from './NumericRange';

export interface Age { Years: number | null, Months: number | null, Days: number | null, TotalDaysOfAge(): IntegerRange | null, IsEmpty(): boolean }

//const defaultEmpty = '';

export class AgeHelper implements Age {

    private _months: number | null;
    private _days: number | null;
    public Years: number | null;

    public get Months() { return this._months; }
    public set Months(newVal: number | null) {
        this._months = newVal;
        if (newVal !== null && this.Years === null) { this.Years = 0; }
    }
    public get Days() { return this._days; }
    public set Days(newVal: number | null) {
        this._days = newVal;
        if (newVal !== null) {
            if (this.Years === null) { this.Years = 0; }
            if (this.Months === null) { this.Months = 0; }
        }
    }
    public TotalDaysOfAge() {
        if (this.IsEmpty()) {
            return null;
        }
        var min = CentileData.Constants.daysPerYear * (this.Years || 0)
            + CentileData.Constants.daysPerMonth * (this._months || 0)
            + (this._days || 0);
        // x  , null, null -> x, 11, 30
        // x  , x   , null -> x,  x, 30
        // x  , x   , x    -> x,  x, x
        //null, x   , x    -> 0,  x, x
        //null, x   , null -> 0,  x, 30
        //null, null, x    -> 0,  0, x
        // x  , null, x    -> x,  0, x
        var months = this._months === null
            ? this._days === null
                ? 11
                : 0
            : this._months;
        var max = CentileData.Constants.daysPerYear * (this.Years || 0)
            + CentileData.Constants.daysPerMonth * months;
            + (this._days === null ? (CentileData.Constants.daysPerMonth -1) : this._days);
        return new IntegerRange(min,Math.round(max));
    } 
    public IsEmpty() {
        return this.Years === null && this._months === null && this._days === null;
    }
}
export class DobHelper implements Age {
    public readonly Dob: moment.Moment;
    public readonly Years: number;
    public readonly Months: number;
    public readonly Days: number;
    private readonly _totalDaysOfAge: IntegerRange;
    TotalDaysOfAge() {
        return this._totalDaysOfAge;
    }
    IsEmpty() {
        return false;
    }
    constructor(dob: moment.Moment) {
        this.Dob = dob;
        var now = moment();
        this._totalDaysOfAge = new IntegerRange(now.diff(dob, 'days'));
        this.Years = now.diff(dob, 'years');
        dob.add(this.Years, 'years');
        this.Months = now.diff(dob, 'months');
        dob.add(this.Months, 'months');
        this.Days = now.diff(dob, 'days');
    }

}