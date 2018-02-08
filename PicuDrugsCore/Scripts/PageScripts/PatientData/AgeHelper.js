"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var moment = require("moment");
var NumericRange_1 = require("./NumericRange");
var CentileDataCollection_1 = require("../../CentileData/CentileDataCollection");
exports.dateFormat = "YYYY-MM-DD";
var minYear = 1900;
function totalDaysOfAge(years, months, days) {
    if (years === null && months === null && days === null) {
        return null;
    }
    years = Number(years);
    var min = CentileDataCollection_1.Constants.daysPerYear * years
        + CentileDataCollection_1.Constants.daysPerMonth * Number(months)
        + (days || 0);
    if (typeof months !== 'number') {
        months = days === null
            ? 11
            : 0;
    }
    var max = CentileDataCollection_1.Constants.daysPerYear * years
        + CentileDataCollection_1.Constants.daysPerMonth * months
        + (typeof days !== 'number' ? (CentileDataCollection_1.Constants.daysPerMonth - 1) : days);
    return new NumericRange_1.IntegerRange(Math.round(min), Math.round(max));
}
exports.totalDaysOfAge = totalDaysOfAge;
function daysOfAgeFromDob(newVal) {
    var m = moment(newVal, exports.dateFormat, true);
    var now;
    if (m.isValid && m.year() > minYear && (now = moment()).diff(m) > 0) {
        var rv = {
            totalDays: now.diff(m, 'days'),
            years: now.diff(m, 'years'),
        };
        m.add(rv.years, 'years');
        rv.months = now.diff(m, 'months');
        m.add(rv.months, 'months');
        rv.days = now.diff(m, 'days');
        return rv;
    }
    return null;
}
exports.daysOfAgeFromDob = daysOfAgeFromDob;
function onNew(units, onMidnight) {
    if (units === void 0) { units = 'day'; }
    setTimeout(tick, msToMidnight());
    function tick() {
        onMidnight(moment().format(exports.dateFormat));
        setTimeout(tick, msToMidnight());
    }
    function msToMidnight() {
        var m = moment();
        return m.clone().endOf(units).diff(m);
    }
}
exports.onNew = onNew;
//# sourceMappingURL=AgeHelper.js.map