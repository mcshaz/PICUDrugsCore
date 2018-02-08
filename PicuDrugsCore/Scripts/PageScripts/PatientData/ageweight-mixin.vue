<script lang="ts">

import * as moment from 'moment'
import * as ageHelper from './AgeHelper'
import { UKWeightData } from '../../CentileData/UkWeightData'
import './centilerange.vue'

const _wtCentiles = new UKWeightData(); 

export default {
    data:function(){
        return {
            p_weight: null as null | number,
            p_isMale: null as null | boolean,
            p_gestation: 40,
            today: moment().format(ageHelper.dateFormat),
            p_dob: '',
            p_years: null as null | number,
            p_months: null as null | number,
            p_days: null as null | number,
            ageDaysLb:null as null| number,
            ageDaysUb:null as null | number,
            lowerCentile:null as null | number,
            upperCentile: null as null | number,
            isValid:false
        }
    }, 
    //components:{centilerange},
    computed:{
        'weight': {
            get: function (this:any) {
                return this.p_weight;
            },
            set: function (newVal: any) {
                this.p_weight = newVal || newVal === 0
                    ?newVal as number
                    :null;
                this.setCentiles();
            }
        },
        'gestation': {
            get: function (this:any) {
                return this.p_gestation;
            },
            set: function (newVal: number) {
                this.p_gestation = newVal;
                this.setCentiles();
            }
        },
        'isMale': {
            get: function (this:any) {
                return this.p_isMale;
            },
            set: function (newVal: any) {
                this.p_isMale = typeof newVal  === 'boolean'
                    ?newVal
                    :null;
                this.setCentiles();
            }
        },
        'days': {
            get: function (this:any) {
                return this.p_days;
            },
            set: function (newVal: any) {
                this.p_days = newVal || newVal === 0
                    ?newVal as number
                    :null;
                this.setAgeBounds();
            }
        },
        'months': {
            get: function (this:any) {
                return this.p_months;
            },
            set: function (newVal: number | string) {
                this.p_months = newVal || newVal === 0
                    ?newVal as number
                    :null;
                this.setAgeBounds();
            }
        },
        'years': {
            get: function (this:any) {
                return this.p_years;
            },
            set: function (newVal: number | string) {
                this.p_years = newVal || newVal === 0
                    ?newVal as number
                    :null;
                this.setAgeBounds();
            }
        },
        'dob': {
            get: function (this:any) {
                return this.p_dob;
            },
            set: function (newVal: string) {
                this.p_dob = newVal;
                const ageData = ageHelper.daysOfAgeFromDob(newVal);
                if (ageData){
                    this.p_years = ageData.years;
                    this.p_months = ageData.months;
                    this.p_days = ageData.days;
                    this.ageDaysUb = this.ageDaysLb = ageData.totalDays;
                }
                this.setCentiles();
            }
        }
    },
    methods:{
        setAgeBounds(){
            let bounds = ageHelper.totalDaysOfAge(this.p_years, this.p_months, this.p_days);
            if (bounds === null){
                this.ageDaysLb = this.ageDaysUb = null;
            } else {
                this.ageDaysLb = bounds.Min;
                this.ageDaysUb = bounds.Max;
            }
            this.setCentiles();
        },
        setCentiles(){
            if (!this.p_weight || this.ageDaysLb===null){
                this.lowerCentile = this.upperCentile = null;
            } else {
                this.lowerCentile = 100 * _wtCentiles.cumSnormForAge(this.p_weight, this.ageDaysUb as number, this.p_isMale === false ? false : true, this.p_gestation);
                this.upperCentile = this.ageDaysUb === this.ageDaysLb && this.p_isMale !== null
                    ? this.lowerCentile
                    : 100 * _wtCentiles.cumSnormForAge(this.p_weight, this.ageDaysLb, !!this.p_isMale, this.p_gestation);
            }
        },
        setCentileValidity(isValid : boolean){
            this.isValid = isValid
        }
    },
    created: function () {
        let self = this;
        ageHelper.onNew('day', function (newDate) {
            self.today = newDate;
        })
    }
}
</script>