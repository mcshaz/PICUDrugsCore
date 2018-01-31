import Vue from 'vue'
import * as moment from 'moment'
//import { NumericRange, IntegerRange } from './NumericRange';
import { AgeHelper, Age, DobHelper } from './AgeHelper'
import { UKWeightData  } from '../../CentileData/UKWeightData'
import { IntegerRange } from './NumericRange';
//import Component from 'vue-class-component'
 
//import { Component } from 'vue-property-decorator'
const dateFormat = "YYYY-MM-DD";
const minYear = 1900;

let _age: Age = new AgeHelper(); 
let _wtCentiles = new UKWeightData();

let vm = new Vue({
    el: '#drug-list',
    data: createData,
    computed: {
        LowerCentile: function (this:any) {
            let ageRng = this.GetAgeRange();
            return ageRng === null
                ? null
                : _wtCentiles.cumSnormForAge(this.Weight, ageRng.Min, !!this.IsMale, this.Gestation);
        },
        UpperCentile: function (this:any) {
            let ageRng:IntegerRange | null = this.GetAgeRange();
            if (ageRng === null) {
                return null;
            }
            return ageRng.NonRange
                ? this.LowerCentile()
                : _wtCentiles.cumSnormForAge(this.Weight, ageRng.Max, this.IsMale === false ? false : true, this.Gestation);
        }
    },
    methods: {
        Parse: function (val: string) {
            let returnVar = +val;
            if (isNaN(returnVar) || !isFinite(returnVar)) {
                return null;
            }
            return Math.round(returnVar);
        },

        GetAgeHelper: function () {
            if (!(_age instanceof AgeHelper)) {
                _age = new AgeHelper();
            }
            return _age;
        },

        GetAgeRange(this:any) {
            if (this.Weight === null || (this.Days === null && this.Months === null && this.Years === null)) {
                return null;
            }
            return _age.TotalDaysOfAge();
        }

    }
});

function createData() {
    let data = { Weight: null, IsMale: null, Gestation: 40 };
    Object.defineProperties(data, {
        'Days': {
            get: () => _age.Days,
            set: function (newVal) {
                let numVal = vm.Parse(newVal);
                vm.GetAgeHelper().Days = numVal;
            },
            enumerable: true, configurable:true
        },
        'Months': {
            get: () => _age.Months,
            set: function (newVal: string) {
                console.log(vm);
                let numVal = vm.Parse(newVal);
                vm.GetAgeHelper().Months = numVal;
            },
            enumerable: true, configurable: true
        },
        'Years': {
            get: () => _age.Years,
            set: function (newVal: string) {
                let numVal = vm.Parse(newVal);
                vm.GetAgeHelper().Years = numVal;
            },
            enumerable: true, configurable: true
        },
        'Dob': {
            get: () => _age instanceof AgeHelper
                    ? null
                    : (_age as DobHelper).Dob.format(dateFormat),
            set: function (newVal: string) {
                let m = moment(newVal, dateFormat, true);
                if (m.isValid && m.year() > minYear) {
                    _age = new DobHelper(m);
                }
            },
            enumerable: true,configurable: true
        }
    });
    return data;
}

export default vm;
