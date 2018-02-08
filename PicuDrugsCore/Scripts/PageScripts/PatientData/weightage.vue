<!-- src/components/weightage.vue -->

<template>
    <div class="weightAge">
        <fieldset class="form-group">
            <div class="form-row">
                <legend class="col-form-label col-sm-2 pt-0">Gender</legend>
                <div class="col-sm-10 gender">
                    <div class="form-check form-check-inline" id="male">
                        <input type="radio" name="gender" id="maleRadio" :value="true" class="form-check-input" v-model="isMale" />
                        <label class="form-check-label" for="maleRadio">
                            Male
                        </label>
                    </div>
                    <div class="form-check form-check-inline" id="female">
                        <input type="radio" name="gender" id="femaleRadio" :value="false" class="form-check-input" v-model="isMale" />
                        <label class="form-check-label" for="femaleRadio">
                            Female
                        </label>
                    </div>
                </div>
            </div>
        </fieldset>
        <div class="form-group form-row">
            <label class="col-sm-2 col-form-label" for="Weight" >Weight</label>
            <div class="input-group col-sm-10">
                <input id="Weight" type=number min="0.2" max="400" class="form-control" v-model.number="weight" step="0.1" required />
                <div class="input-group-append">
                    <div class="input-group-text">Kg</div>
                </div>
            </div>
        </div>
        <CentileRange :centiles="centiles" v-on:validCentile="setCentileValidity"/>
        <div class="form-group form-row">
            <label class="col-sm-2 col-form-label" for="dob">Date of Birth</label>
            <div class="col-sm-10">
                <input class="form-control" type="date" :max="today" v-model="dob" id="dob" />
            </div>
            <span class="text-danger"></span>
        </div>
        <fieldset class="form-group">
            <div class="form-row">
                <legend class="col-form-label col-sm-2 pt-0">Age</legend>
                <div class="col-sm-10 age form-inline">
                        <div class="input-group mb-1">
                            <input type="number" step="1" min="0" max="130" v-model.number="years" id="age-years" class="form-control" />
                            <div class="input-group-append">
                                <div class="input-group-text">years</div>
                            </div>
                        </div>
                        <div class="input-group mb-1">
                            <input type="number" step="1" min="0" max="37" v-model.number="months" id="age-months" class="form-control" />
                            <div class="input-group-append">
                                <div class="input-group-text">months</div>
                            </div>
                        </div>
                        <div class="input-group mb-1">    
                            <input type="number" step="1" min="0" max="90" v-model.number="days" id="age-days" class="form-control" />
                            <div class="input-group-append">
                                <div class="input-group-text">days</div>
                            </div>
                        </div>
                </div>
            </div>
        </fieldset>
        <div class="form-group form-row">
            <label class="col-sm-2 col-form-label" for="GestationAtBirth" >Birth Gestation</label>
            <div class="input-group col-sm-10">
                <input id="GestationAtBirth" type=number min="23" max="43" step="1" class="form-control" v-model="gestation" required/>
                <div class="input-group-append">
                    <div class="input-group-text">weeks</div>
                </div>
            </div>
            <small id="nhiHelp" class="form-text text-muted">for checking weight is correct for age</small>
        </div>
    </div>
</template>

<script lang="ts">
import Vue from 'vue'
import * as moment from 'moment'
import * as ageHelper from './AgeHelper'
import { UKWeightData } from '../../CentileData/UkWeightData'
import './CentileRange.vue'
import { NumericRange, IntegerRange } from './NumericRange';

const _wtCentiles = new UKWeightData(); 
export default Vue.extend({
    data(){
        return {
            p_weight: null as null | number,
            p_isMale: null as null | boolean,
            p_gestation: 40,
            today: moment().format(ageHelper.dateFormat),
            p_dob: '',
            p_years: null as null | number,
            p_months: null as null | number,
            p_days: null as null | number,
            centiles:null as null | NumericRange,
            isValid:false,
            //p_ageDays:null as null | NumericRange
        };
    }, 
    //components:{centilerange},
    computed:{
        'weight': {
            get(this:any) {
                return this.p_weight;
            },
            set(newVal: any) {
                this.p_weight = newVal || newVal === 0
                    ?newVal as number
                    :null;
                this.setCentiles();
            }
        },
        'gestation': {
            get(this:any) {
                return this.p_gestation;
            },
            set(newVal: number) {
                this.p_gestation = newVal;
                this.setCentiles();
            }
        },
        'isMale': {
            get(this:any) {
                return this.p_isMale;
            },
            set(newVal: any) {
                this.p_isMale = typeof newVal  === 'boolean'
                    ?newVal
                    :null;
                this.setCentiles();
            }
        },
        'days': {
            get(this:any) {
                return this.p_days;
            },
            set(newVal: any) {
                this.p_days = newVal || newVal === 0
                    ?newVal as number
                    :null;
                this.setAgeBounds();
            }
        },
        'months': {
            get(this:any) {
                return this.p_months;
            },
            set(newVal: number | string) {
                this.p_months = newVal || newVal === 0
                    ?newVal as number
                    :null;
                this.setAgeBounds();
            }
        },
        'years': {
            get(this:any) {
                return this.p_years;
            },
            set(newVal: number | string) {
                this.p_years = newVal || newVal === 0
                    ?newVal as number
                    :null;
                this.setAgeBounds();
            }
        },
        'dob': {
            get(this:any) {
                return this.p_dob;
            },
            set(newVal: string) {
                this.p_dob = newVal;
                const ageData = ageHelper.daysOfAgeFromDob(newVal);
                if (ageData){
                    this.p_years = ageData.years;
                    this.p_months = ageData.months;
                    this.p_days = ageData.days;
                    this.$data.$_ageDays = new IntegerRange(ageData.totalDays);
                }
                this.setCentiles();
            }
        }
    },
    methods:{
        setAgeBounds(){
            this.$data.$_ageDays = ageHelper.totalDaysOfAge(this.p_years, this.p_months, this.p_days);
            this.setCentiles();
        },
        setCentiles(){
            if (!this.p_weight || !this.$data.$_ageDays){
                this.centiles = null;
            } else {
                const lowerCentile = 100 * _wtCentiles.cumSnormForAge(this.p_weight, this.$data.$_ageDays.max as number, this.p_isMale === false ? false : true, this.p_gestation);
                this.centiles = this.$data.$_ageDays.nonRange && this.p_isMale !== null
                    ? new NumericRange(lowerCentile)
                    : new NumericRange(lowerCentile, 100 * _wtCentiles.cumSnormForAge(this.p_weight, this.$data.$_ageDays.min, !!this.p_isMale, this.p_gestation));
            }
        },
        setCentileValidity(isValid : boolean){
            this.isValid = isValid
        }
    },
    created() {
        let self = this;
        ageHelper.onNew('day', function (newDate) {
            self.today = newDate;
        });
    }
});
</script>

<style scoped>
    .gender > .form-check{
        border-width: 1px;
        border-style: solid;
        padding:0.375rem 0.75rem;
        border-radius: 0.25rem;
    }
    #male{
        color: navy;
        border-color: blue;
    }
    #female{
        color: deeppink;
        border-color:pink;
    }
    .age > div{
        padding-right: 0.375rem;
    }
    .age >div:last-child{
        padding-right: 0;
    }
</style>
