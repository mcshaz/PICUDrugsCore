"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var vue_1 = require("vue");
var moment = require("moment");
var AgeHelper_1 = require("./AgeHelper");
var UKWeightData_1 = require("../../CentileData/UKWeightData");
var defaultEmpty = '';
var dateFormat = "YYYY-MM-DD";
var minYear = 1900;
var _age = new AgeHelper_1.AgeHelper();
var _wtCentiles = new UKWeightData_1.UKWeightData();
var data = { Weight: defaultEmpty, IsMale: null, Gestation: 40 };
Object.defineProperties(data, {
    'Days': {
        get: function () {
            return (_age.Days || defaultEmpty).toString();
        },
        set: function (newVal) {
            var numVal = this.Parse(newVal);
            this.GetAgeHelper().Days = numVal;
        },
        enumerable: true
    },
    'Months': {
        get: function () {
            return (_age.Months || defaultEmpty).toString();
        },
        set: function (newVal) {
            var numVal = this.Parse(newVal);
            this.GetAgeHelper().Months = numVal;
        },
        enumerable: true
    },
    'Years': {
        get: function () {
            return (_age.Years || defaultEmpty).toString();
        },
        set: function (newVal) {
            var numVal = this.Parse(newVal);
            this.GetAgeHelper().Years = numVal;
        },
        enumerable: true
    },
    'Dob': {
        get: function () {
            return _age instanceof AgeHelper_1.AgeHelper
                ? defaultEmpty
                : _age.Dob.format(dateFormat);
        },
        set: function (newVal) {
            var m = moment(newVal, dateFormat, true);
            if (m.isValid && m.year() > minYear) {
                _age = new AgeHelper_1.DobHelper(m);
            }
        },
        enumerable: true
    }
});
var vm = new vue_1.default({
    el: '#drug-list',
    data: data,
    computed: {
        LowerCentile: function () {
            var ageRng = this.GetAgeRange();
            return ageRng === null
                ? defaultEmpty
                : _wtCentiles.cumSnormForAge(this.Weight, ageRng.Min, !!this.IsMale, this.Gestation);
        },
        UpperCentile: function () {
            var ageRng = this.GetAgeRange();
            if (ageRng === null) {
                return defaultEmpty;
            }
            return ageRng.NonRange
                ? this.LowerCentile
                : _wtCentiles.cumSnormForAge(this.Weight, ageRng.Max, this.IsMale === false ? false : true, this.Gestation);
        }
    },
    methods: {
        Parse: function (val) {
            var returnVar = +val;
            if (isNaN(returnVar) || !isFinite(returnVar)) {
                return defaultEmpty;
            }
            return Math.round(returnVar);
        },
        GetAgeHelper: function () {
            if (!(_age instanceof AgeHelper_1.AgeHelper)) {
                _age = new AgeHelper_1.AgeHelper();
            }
            return _age;
        },
        GetAgeRange: function () {
            if (this.Weight == defaultEmpty || (this.Days === defaultEmpty && this.Months === defaultEmpty && this.Years === defaultEmpty)) {
                return null;
            }
            return _age.TotalDaysOfAge();
        }
    }
});
exports.default = vm;
//# sourceMappingURL=DrugLists.js.map