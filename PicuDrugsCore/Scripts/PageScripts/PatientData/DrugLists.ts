import Vue from 'vue'
import * as moment from 'moment'
//import { NumericRange, IntegerRange } from './NumericRange';
import { AgeHelper, Age, DobHelper } from './AgeHelper'
//import Component from 'vue-class-component'
 
//import { Component } from 'vue-property-decorator'
const defaultEmpty = '';
const dateFormat = "YYYY-MM-DD";
const minYear = 1900;

let _age: Age = new AgeHelper(); 
let _wtCentiles = new CentileData.UKWeightData();
let data = { Weight: defaultEmpty, IsMale: null, Gestation: 40 };

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
        set: function (newVal: string) {
            var numVal = this.Parse(newVal);
            this.GetAgeHelper().Months = numVal;
        },
        enumerable: true
    },
    'Years': {
        get: function () {
            return (_age.Years || defaultEmpty).toString();
        },
        set: function (newVal: string) {
            var numVal = this.Parse(newVal);
            this.GetAgeHelper().Years = numVal;
        },
        enumerable: true
    },
    'Dob': {
        get: function () {
            return _age instanceof AgeHelper
                ? defaultEmpty
                : (_age as DobHelper).Dob.format(dateFormat);
        },
        set: function (newVal: string) {
            var m = moment(newVal, dateFormat, true);
            if (m.isValid && m.year() > minYear) {
                _age = new DobHelper(m);
            }
        },
        enumerable: true
    }
});

var vm = new Vue({
    el: '#drug-list',
    data: data,
    computed: {
        LowerCentile: function () {
            let ageRng = this.GetAgeRange();
            return ageRng === null
                ? defaultEmpty
                : _wtCentiles.cumSnormForAge(this.Weight, ageRng.Min, !!this.IsMale, this.Gestation);
        },
        UpperCentile: function () {
            let ageRng = this.GetAgeRange();
            if (ageRng === null) {
                return defaultEmpty;
            }
            return ageRng.NonRange
                ? this.LowerCentile
                : _wtCentiles.cumSnormForAge(this.Weight, ageRng.Max, this.IsMale === false ? false : true, this.Gestation);
        }
    },
    methods: {
        Parse: function (val: string) {
            let returnVar = +val;
            if (isNaN(returnVar) || !isFinite(returnVar)) {
                return defaultEmpty;
            }
            return Math.round(returnVar);
        },

        GetAgeHelper: function () {
            if (!(_age instanceof AgeHelper)) {
                _age = new AgeHelper();
            }
            return _age;
        },

        GetAgeRange() {
            if (this.Weight == defaultEmpty || (this.Days === defaultEmpty && this.Months === defaultEmpty && this.Years === defaultEmpty)) {
                return null;
            }
            return _age.TotalDaysOfAge();
        }

    }
});

export default vm;
