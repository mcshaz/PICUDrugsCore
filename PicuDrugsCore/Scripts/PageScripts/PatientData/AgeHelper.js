"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var moment = require("moment");
var NumericRange_1 = require("./NumericRange");
var CentileDataCollection_1 = require("../../CentileData/CentileDataCollection");
var dateFormat = "YYYY-MM-DD";
var minYear = 1900;
var AgeHelper = (function () {
    function AgeHelper() {
    }
    Object.defineProperty(AgeHelper.prototype, "Months", {
        get: function () { return this._months; },
        set: function (newVal) {
            this._months = newVal;
            if (newVal !== null && this.Years === null) {
                this.Years = 0;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AgeHelper.prototype, "Days", {
        get: function () { return this._days; },
        set: function (newVal) {
            this._days = newVal;
            if (newVal !== null) {
                if (this.Years === null) {
                    this.Years = 0;
                }
                if (this.Months === null) {
                    this.Months = 0;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    AgeHelper.prototype.TotalDaysOfAge = function () {
        if (this.IsEmpty()) {
            return null;
        }
        var min = CentileDataCollection_1.Constants.daysPerYear * (this.Years || 0)
            + CentileDataCollection_1.Constants.daysPerMonth * (this._months || 0)
            + (this._days || 0);
        var months = this._months === null
            ? this._days === null
                ? 11
                : 0
            : this._months;
        var max = CentileDataCollection_1.Constants.daysPerYear * (this.Years || 0)
            + CentileDataCollection_1.Constants.daysPerMonth * months;
        +(this._days === null ? (CentileDataCollection_1.Constants.daysPerMonth - 1) : this._days);
        return new NumericRange_1.IntegerRange(min, Math.round(max));
    };
    AgeHelper.prototype.IsEmpty = function () {
        return this.Years === null && this._months === null && this._days === null;
    };
    return AgeHelper;
}());
exports.AgeHelper = AgeHelper;
var DobHelper = (function () {
    function DobHelper() {
    }
    Object.defineProperty(DobHelper.prototype, "Dob", {
        get: function () {
            return this._dob;
        },
        set: function (newVal) {
            this._dob = newVal;
            var m = moment(newVal, dateFormat, true);
            var now;
            if (m.isValid && m.year() > minYear && (now = moment()).diff(m) > 0) {
                now = moment();
                this._totalDaysOfAge = new NumericRange_1.IntegerRange(now.diff(m, 'days'));
                this.Years = now.diff(m, 'years');
                m.add(this.Years, 'years');
                this.Months = now.diff(m, 'months');
                m.add(this.Months, 'months');
                this.Days = now.diff(m, 'days');
            }
            else {
                this.Years = this.Months = this.Days = null;
            }
        },
        enumerable: true,
        configurable: true
    });
    DobHelper.prototype.TotalDaysOfAge = function () {
        return this._totalDaysOfAge;
    };
    DobHelper.prototype.IsEmpty = function () {
        return false;
    };
    return DobHelper;
}());
exports.DobHelper = DobHelper;
//# sourceMappingURL=AgeHelper.js.map