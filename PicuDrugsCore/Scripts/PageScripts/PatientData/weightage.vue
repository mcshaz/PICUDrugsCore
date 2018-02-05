<!-- src/components/weightage.vue -->

<template>
    <div class="weightAge">
        <fieldset class="form-group">
            <div class="form-row">
                <legend class="col-form-label col-sm-2 pt-0">Gender</legend>
                <div class="col-sm-10">
                    <div class="form-check form-check-inline">
                        <input Id="MaleGender" type="radio" name="gender" :value="true" class="form-check-input" v-model="isMale" />
                        <label class="form-check-label" for="MaleGender">
                            Male
                        </label>
                    </div>
                    <div class="form-check form-check-inline">
                        <input Id="FemaleGender" type="radio" name="gender" :value="false" class="form-check-input" v-model="isMale" />
                        <label class="form-check-label" for="FemaleGender">
                            Female
                        </label>
                        <span asp-validation-for="MaleGender" class="text-danger"></span>
                    </div>
                </div>
            </div>
        </fieldset>
        <div class="form-group form-row">
            <label class="col-sm-2 col-form-label" for="Weight" >Weight</label>
            <div class="input-group col-sm-10">
                <input id="Weight" type=number min="0.2" max="400" class="form-control" v-model.number="weight" required />
                <div class="input-group-append">
                    <div class="input-group-text">Kg</div>
                </div>
            </div>
            <span asp-validation-for="Weight" class="text-danger"></span>
        </div>
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
                <div class="col-sm-10 form-inline">
                        <div class="input-group">
                            <input type="number" step="1" min="0" max="130" v-model.number="years" id="age-years" class="form-control" />
                            <div class="input-group-append">
                                <div class="input-group-text">years</div>
                            </div>
                        </div>
                        <div class="input-group">
                            <input type="number" step="1" min="0" max="37" v-model.number="months" id="age-months" class="form-control" />
                            <div class="input-group-append">
                                <div class="input-group-text">months</div>
                            </div>
                        </div>
                        <div class="input-group">    
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
                <input id="GestationAtBirth" type=number min="23" max="43" step="1" class="form-control" v-model="gestation" />
                <div class="input-group-append">
                    <div class="input-group-text">weeks</div>
                </div>
            </div>
            <small id="nhiHelp" class="form-text text-muted">for checking weight is correct for age</small>
            <span asp-validation-for="GestationAtBirth" class="text-danger"></span>
        </div>
    </div>
</template>

<script lang="ts">
import Vue from 'vue'
import * as moment from 'moment'
import * as ageHelper from './AgeHelper'

export default Vue.extend({
    data:function(){
        return {
            weight: null as null | number,
            isMale: null as null | boolean,
            gestation: 40, lowerCentile: null as null | number,
            today: moment().format(ageHelper.dateFormat),
            p_dob: '',
            p_years: null as null | number,
            p_months: null as null | number,
            p_days: null as null | number,
            ageDaysLb:null as null| number,
            ageDaysUb:null as null | number
        }
    },
    computed:{
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
                    this.ageDaysUb = this.ageDaysLb = ageData.totalDays;
                    this.p_years = ageData.years;
                    this.p_months = ageData.months;
                    this.p_days = ageData.days;
                }
                
            }
        }
    },
    methods:{
        setAgeBounds(){
            let bounds = ageHelper.totalDaysOfAge(this.p_years, this.p_months, this.p_months);
            if (bounds === null){
                this.ageDaysLb = this.ageDaysUb = null;
            } else {
                this.ageDaysLb = bounds.Min;
                this.ageDaysUb = bounds.Max;
            }
        }
    },
    created: function () {
        let self = this;
        ageHelper.OnNew('day', function (newDate) {
            self.today = newDate;
        })
    }
});
</script>
