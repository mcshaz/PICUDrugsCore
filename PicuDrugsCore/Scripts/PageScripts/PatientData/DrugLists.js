"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var vue_1 = require("vue");
var AgeHelper_1 = require("./AgeHelper");
var UKWeightData_1 = require("../../CentileData/UKWeightData");
var NumberToWords_1 = require("../../Utilities/NumberToWords");
var _wtCentiles = new UKWeightData_1.UKWeightData();
var vm = new vue_1.default({
    el: '#drug-list',
    data: createData,
    computed: {
        centileHtml: function () {
            this.calculateCentile();
            if (this.lowerCentile === null || this.upperCentile === null) {
                return null;
            }
            var lower = Math.round(this.lowerCentile);
            var lowerText = lower < 1 ? '&lt1<sup>st</sup>' : (lower + "<sup>" + NumberToWords_1.getSuffix(lower) + "</sup>");
            var upper;
            if (this.lowerCentile === this.upperCentile || (upper = Math.round(this.upperCentile)) === lower) {
                return lowerText;
            }
            else {
                return lowerText + " - " + (upper >= 99 ? '&gt' : '') + upper + "<sup>" + NumberToWords_1.getSuffix(upper) + "</sup>";
            }
        }
    },
    methods: {
        getAgeRange: function () {
            if (this.Weight === null || (this.Days === null && this.Months === null && this.Years === null)) {
                return null;
            }
            return this.p_age.TotalDaysOfAge();
        },
        calculateCentile: function () {
            var ageRng = this.getAgeRange();
            if (ageRng === null) {
                this.lowerCentile = this.upperCentile = null;
            }
            else {
                this.lowerCentile = 100 * _wtCentiles.cumSnormForAge(this.Weight, ageRng.Min, !!this.IsMale, this.Gestation);
                this.upperCentile = ageRng.NonRange && this.isMale !== null
                    ? this.lowerCentile
                    : 100 * _wtCentiles.cumSnormForAge(this.Weight, ageRng.Max, this.IsMale === false ? false : true, this.Gestation);
            }
        },
        getAgeHelper: function () {
            if (!(this.p_age instanceof AgeHelper_1.AgeHelper)) {
                this.p_age = new AgeHelper_1.AgeHelper();
            }
            return this.p_age;
        }
    }
});
function createData() {
    var data = {
        Weight: null,
        IsMale: null,
        Gestation: 40, lowerCentile: null,
        upperCentile: null,
        p_age: new AgeHelper_1.AgeHelper()
    };
    Object.defineProperties(data, {
        'Days': {
            get: function () {
                return this.p_age.Days;
            },
            set: function (newVal) {
                var numVal = parse(newVal);
                this.getAgeHelper().Days = numVal;
            },
            enumerable: true, configurable: true
        },
        'Months': {
            get: function () {
                return this.p_age.Months;
            },
            set: function (newVal) {
                var numVal = parse(newVal);
                this.getAgeHelper().Months = numVal;
            },
            enumerable: true, configurable: true
        },
        'Years': {
            get: function () {
                return this.p_age.Years;
            },
            set: function (newVal) {
                var numVal = parse(newVal);
                this.getAgeHelper().Years = numVal;
            },
            enumerable: true, configurable: true
        },
        'Dob': {
            get: function () {
                return this.p_age instanceof AgeHelper_1.AgeHelper
                    ? null
                    : this.p_age.Dob;
            },
            set: function (newVal) {
                if (!(this.p_age instanceof AgeHelper_1.DobHelper)) {
                    this.p_age = new AgeHelper_1.DobHelper();
                }
                this.p_age.Dob = newVal;
            },
            enumerable: true, configurable: true
        }
    });
    return data;
}
function parse(val) {
    var returnVar = +val;
    if (isNaN(returnVar) || !isFinite(returnVar)) {
        return null;
    }
    return Math.round(returnVar);
}
exports.default = vm;
//# sourceMappingURL=DrugLists.js.map