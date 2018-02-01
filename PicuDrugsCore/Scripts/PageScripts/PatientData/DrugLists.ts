import Vue from 'vue'
//import { NumericRange, IntegerRange } from './NumericRange';
import { AgeHelper, DobHelper } from './AgeHelper'
import { UKWeightData  } from '../../CentileData/UKWeightData'
import { IntegerRange } from './NumericRange';
import { getSuffix } from '../../Utilities/NumberToWords';
import * as moment from 'moment';
//import Component from 'vue-class-component'
 
//import { Component } from 'vue-property-decorator'

let _wtCentiles = new UKWeightData();

let vm = new Vue({
    el: '#drug-list',
    data: createData,
    computed: {
        centileHtml: function () {
            this.calculateCentile();
            if (this.lowerCentile === null || this.upperCentile === null) {
                return null;
            }
            let lower = Math.round(this.lowerCentile);
            let lowerText = lower < 1 ? '&lt1<sup>st</sup>' : (`${lower}<sup>${getSuffix(lower)}</sup>`);
            let upper: number;
            if (this.lowerCentile === this.upperCentile || (upper = Math.round(this.upperCentile)) === lower) {
                return lowerText;
            } else {
                return `${lowerText} - ${(upper >= 99 ? '&gt' : '')}${upper}<sup>${getSuffix(upper)}</sup>`;
            }
        }
    },
    methods: {
        getAgeRange: function (this: any) {
            if(this.Weight === null || (this.Days === null && this.Months === null && this.Years === null)) {
                return null;
            }
            return this.p_age.TotalDaysOfAge();
        },
        calculateCentile: function (this: any) {
            let ageRng: IntegerRange | null = this.getAgeRange();
            if (ageRng === null) {
                this.lowerCentile = this.upperCentile = null;
            } else {
                this.lowerCentile = 100 * _wtCentiles.cumSnormForAge(this.Weight, ageRng.Min, this.IsMale === false ? false : true, this.Gestation);
                this.upperCentile = ageRng.NonRange && this.IsMale !== null
                    ? this.lowerCentile
                    : 100 * _wtCentiles.cumSnormForAge(this.Weight, ageRng.Max, !!this.IsMale, this.Gestation);
            }
        },
        getAgeHelper: function() {
            if(!(this.p_age instanceof AgeHelper)) {
                this.p_age = new AgeHelper();
            }
            return this.p_age;
        }
    },
    created: function () {
        DobHelper.OnNew('day', function (this: any, newDate) {
            this.today = newDate;
        }, this)
    }
});



function createData() {
    let data = {
        Weight: null as null | number,
        IsMale: null as null | boolean,
        Gestation: 40, lowerCentile: null as null | number,
        upperCentile: null as null | number,
        today: moment().format(DobHelper.dateFormat),
        p_age: new AgeHelper()
        
    };
    Object.defineProperties(data, {
        'Days': {
            get: function () {
                return this.p_age.Days;
            },
            set: function (newVal: number) {
                this.getAgeHelper().Days = newVal;
            },
            enumerable: true, configurable:true
        },
        'Months': {
            get: function () {
                return this.p_age.Months;
            },
            set: function (newVal: number) {
                this.getAgeHelper().Months = newVal;
            },
            enumerable: true, configurable: true
        },
        'Years': {
            get: function () {
                return this.p_age.Years;
            },
            set: function (newVal: number) {
                this.getAgeHelper().Years = newVal;
            },
            enumerable: true, configurable: true
        },
        'Dob': {
            get: function () {
                return this.p_age instanceof AgeHelper
                    ? null
                    : (this.p_age as DobHelper).Dob;
            },
            set: function (newVal: string) {
                if (!(this.p_age instanceof DobHelper)) {
                    this.p_age = new DobHelper();
                }
                (this.p_age as DobHelper).Dob = newVal;
            },
            enumerable: true,configurable: true
        }
    });
    return data;
}

export default vm;
