import * as moment from 'moment';
import { IntegerRange } from './NumericRange';
import { Constants } from '../../CentileData/CentileDataCollection'


    export const dateFormat = "YYYY-MM-DD";
    const minYear = 1900;

    export function totalDaysOfAge(years: number | null, months: number | null, days: number | null) {
        if (years === null && months === null && days ===null) {
            return null;
        }
        const min = Constants.daysPerYear * (years || 0)
            + Constants.daysPerMonth * (months || 0)
            + (days || 0);
        // x  , null, null -> x, 11, 30
        // x  , x   , null -> x,  x, 30
        // x  , x   , x    -> x,  x, x
        //null, x   , x    -> 0,  x, x
        //null, x   , null -> 0,  x, 30
        //null, null, x    -> 0,  0, x
        // x  , null, x    -> x,  0, x
        if (months === null){
            months = days === null
                ? 11
                : 0;
        }

        const max = Constants.daysPerYear * (years || 0)
            + Constants.daysPerMonth * months
            + (days === null ? (Constants.daysPerMonth -1) : days);
        return new IntegerRange(Math.round(min),Math.round(max));
    } 
    interface age {years:number,months:number,days:number,totalDays:number}
    export function daysOfAgeFromDob(newVal: string) : age | null {
        const m = moment(newVal, dateFormat, true);
        let now: moment.Moment;
        if (m.isValid && m.year() > minYear && (now = moment()).diff(m) > 0) {
            let rv = {
                totalDays : now.diff(m, 'days'),
                years: now.diff(m, 'years'),

            } as any;
            m.add(rv.years, 'years');
            rv.months = now.diff(m, 'months');
            m.add(rv.months, 'months');
            rv.days = now.diff(m, 'days');
            return rv;
        } 
        return null;
    }
    
    export function onNew(units: moment.unitOfTime.StartOf = 'day', onMidnight: (date: string) => void): void {
        setTimeout(tick, msToMidnight());

        function tick() {
            onMidnight(moment().format(dateFormat));
            setTimeout(tick, msToMidnight());
        }

        function msToMidnight() {
            let m = moment();
            return m.clone().endOf(units).diff(m);
        }

    }
