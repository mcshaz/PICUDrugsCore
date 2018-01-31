"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var vue_1 = require("vue");
var moment = require("moment");
var AgeHelper_1 = require("./AgeHelper");
var UKWeightData_1 = require("../../CentileData/UKWeightData");
var dateFormat = "YYYY-MM-DD";
var minYear = 1900;
var _age = new AgeHelper_1.AgeHelper();
var _wtCentiles = new UKWeightData_1.UKWeightData();
var vm = new vue_1.default({
    el: '#drug-list',
    data: createData,
    computed: {
        LowerCentile: function () {
            var ageRng = this.GetAgeRange();
            return ageRng === null
                ? null
                : _wtCentiles.cumSnormForAge(this.Weight, ageRng.Min, !!this.IsMale, this.Gestation);
        },
        UpperCentile: function () {
            var ageRng = this.GetAgeRange();
            if (ageRng === null) {
                return null;
            }
            return ageRng.NonRange
                ? this.LowerCentile()
                : _wtCentiles.cumSnormForAge(this.Weight, ageRng.Max, this.IsMale === false ? false : true, this.Gestation);
        }
    },
    methods: {
        Parse: function (val) {
            var returnVar = +val;
            if (isNaN(returnVar) || !isFinite(returnVar)) {
                return null;
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
            if (this.Weight === null || (this.Days === null && this.Months === null && this.Years === null)) {
                return null;
            }
            return _age.TotalDaysOfAge();
        }
    }
});
function createData() {
    var data = { Weight: null, IsMale: null, Gestation: 40 };
    Object.defineProperties(data, {
        'Days': {
            get: function () { return _age.Days; },
            set: function (newVal) {
                var numVal = vm.Parse(newVal);
                vm.GetAgeHelper().Days = numVal;
            },
            enumerable: true, configurable: true
        },
        'Months': {
            get: function () { return _age.Months; },
            set: function (newVal) {
                console.log(vm);
                var numVal = vm.Parse(newVal);
                vm.GetAgeHelper().Months = numVal;
            },
            enumerable: true, configurable: true
        },
        'Years': {
            get: function () { return _age.Years; },
            set: function (newVal) {
                var numVal = vm.Parse(newVal);
                vm.GetAgeHelper().Years = numVal;
            },
            enumerable: true, configurable: true
        },
        'Dob': {
            get: function () { return _age instanceof AgeHelper_1.AgeHelper
                ? null
                : _age.Dob.format(dateFormat); },
            set: function (newVal) {
                var m = moment(newVal, dateFormat, true);
                if (m.isValid && m.year() > minYear) {
                    _age = new AgeHelper_1.DobHelper(m);
                }
            },
            enumerable: true, configurable: true
        }
    });
    return data;
}
exports.default = vm;
//# sourceMappingURL=DrugLists.js.map