import * as moment from 'moment';
import { IntegerRange } from './NumericRange';
import { Constants } from '../../CentileData/CentileDataCollection'

export interface Age { Years: number | null, Months: number | null, Days: number | null, TotalDaysOfAge(): IntegerRange | null, IsEmpty(): boolean }

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
        var min = Constants.daysPerYear * (this.Years || 0)
            + Constants.daysPerMonth * (this._months || 0)
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
        var max = Constants.daysPerYear * (this.Years || 0)
            + Constants.daysPerMonth * months;
            + (this._days === null ? (Constants.daysPerMonth -1) : this._days);
        return new IntegerRange(min,Math.round(max));
    } 
    public IsEmpty() {
        return this.Years === null && this._months === null && this._days === null;
    }
}
export class DobHelper implements Age {
    public static readonly dateFormat = "YYYY-MM-DD";
    public static readonly minYear = 1900;
    public Years: number | null;
    public Months: number | null;
    public Days: number | null;
    private _dob: string;
    private _totalDaysOfAge: IntegerRange;
    public get Dob() {
        return this._dob;
    } 
    public set Dob(newVal: string) {
        this._dob = newVal;
        let m = moment(newVal, DobHelper.dateFormat, true);
        let now: moment.Moment;
        if (m.isValid && m.year() > DobHelper.minYear && (now = moment()).diff(m) > 0) {
            now = moment();
            this._totalDaysOfAge = new IntegerRange(now.diff(m, 'days'));
            this.Years = now.diff(m, 'years');
            m.add(this.Years, 'years');
            this.Months = now.diff(m, 'months');
            m.add(this.Months, 'months');
            this.Days = now.diff(m, 'days');
        } else {
            this.Years = this.Months = this.Days = null;
        }
    }
    TotalDaysOfAge() {
        return this._totalDaysOfAge;
    }
    IsEmpty() {
        return false;
    }
    static OnNew(units: moment.unitOfTime.StartOf = 'day', onMidnight: (date: string) => void, self?: object): void {
        setTimeout(tick, msToMidnight());

        function tick() {
            onMidnight.call(self,moment().format(DobHelper.dateFormat));
            setTimeout(tick, msToMidnight());
        }

        function msToMidnight() {
            let m = moment();
            return m.clone().endOf(units).diff(m);
        }

    }
}