// src/components/Hello.ts

import Vue from 'vue'
import { Component } from 'vue-property-decorator'

@Component

const dateFormat = "YYYY-MM-DD";
export default Vue.extend({
    template: '#drug-list',
    data() { 
        return { pDob: null, pYears: null, pMonths: null, pDays: null };
    },
    computed: {
        dob: {
            get: function () {
                return this.pDob;
            },
            set: function (newVal) {
                if (newVal !== null && newVal !== '') {
                    var dob = moment(newVal,dateFormat); 
                    if (dob.isValid() && dob.year() > 1900) {//probably still typing
                        var now = moment();
                        this.pDob = newVal;
                        this.pYears = now.diff(dob, 'years');
                        dob.add(this.pYears, 'years');
                        this.pMonths = now.diff(dob, 'months');
                        dob.add(this.pMonths, 'months');
                        this.pDays = now.diff(dob, 'days');
                        return;
                    } 
                }
                this.pDob = null;
            }
        },
        ageyears: {
            get: function () {
                return this.pYears;
            },
            set: function (newVal) {
                if (newVal !== null && newVal !== '') {
                    this.pDob = null;
                }
                this.pYears = newVal;
            }
        },
        agemonths: {
            get: function () {
                return this.pMonths;
            },
            set: function (newVal) {
                if (newVal !== null && newVal !== '') {
                    this.pDob = null;
                }
                if (newVal > 12) {
                    this.pYears += Math.floor(newVal/12);
                    newVal = newVal % 12;
                }
                this.pMonths = newVal;
            }
        },
        agedays: {
            get: function () {
                return this.pDays;
            },
            set: function (newVal) {
                if (newVal !== null && newVal !== '') {
                    this.pDob = null;
                    if (newVal > 31) {
                        this.agemonths += Math.floor(newVal / 30);
                        newVal = newVal % 30;
                    }
                }
                this.pDays = newVal;
            }
        }
    }
});
